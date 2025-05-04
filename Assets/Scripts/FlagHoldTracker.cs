using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class FlagHoldTracker : MonoBehaviour
{

    [SerializeField] private List<TeamData> teams;
    [SerializeField] private float scorePerSecond = 1f;
    [SerializeField] private float winScore = 100f;
    [SerializeField] private int roundsToWin = 2;
    [SerializeField] private float roundDuration = 300f;

    [SerializeField] private MessageDisplayer messageDisplayer;
    public TeamData LeftTeam => leftTeam;
    public TeamData RightTeam => rightTeam;

    [SerializeField] private TeamData leftTeam;
    [SerializeField] private TeamData rightTeam;

    private float roundTimer;
    private float leftHoldTime = 0f;
    private float rightHoldTime = 0f;

    private Dictionary<TeamData, int> teamRoundsWon = new();
    private Dictionary<TeamData, float> teamScores = new();
    private Dictionary<TeamData, bool> isHoldingFlag = new();
    private Dictionary<TeamData, List<Transform>> spawnPoints = new();

    [SerializeField, NonSerialized]
    private List<Flag> allFlags = new();

    private bool roundEnded = false;
    private bool isResetting = false;

    private void Awake()
    {
        foreach (var team in teams)
        {
            teamScores[team] = 0f;
            isHoldingFlag[team] = false;
            teamRoundsWon[team] = 0;
        }

        foreach (var point in FindObjectsOfType<TeamSpawnPoint>())
        {
            if (!spawnPoints.ContainsKey(point.Team))
                spawnPoints[point.Team] = new List<Transform>();
            spawnPoints[point.Team].Add(point.transform);
        }
    }

    private void Start()
    {
        StartCoroutine(WaitAndFetchFlags());
        roundTimer = roundDuration;

        if (messageDisplayer != null)
            messageDisplayer.SetTeamUI(leftTeam, rightTeam);
    }

    private IEnumerator WaitAndFetchFlags()
    {
        yield return new WaitForSeconds(0.1f);
        allFlags = new List<Flag>(FindObjectsOfType<Flag>());
    }

    private void OnEnable()
    {
        FlagEvents.OnFlagPickedUp += HandlePickup;
        FlagEvents.OnFlagDropped += HandleDrop;
        FlagEvents.OnFlagReturned += HandleReturn;
    }

    private void OnDisable()
    {
        FlagEvents.OnFlagPickedUp -= HandlePickup;
        FlagEvents.OnFlagDropped -= HandleDrop;
        FlagEvents.OnFlagReturned -= HandleReturn;
    }

    private void Update()
    {
        if (!roundEnded)
        {
            roundTimer -= Time.deltaTime;

            foreach (var kvp in isHoldingFlag)
            {
                if (kvp.Value)
                {
                    teamScores[kvp.Key] += scorePerSecond * Time.deltaTime;

                    if (teamScores[kvp.Key] >= winScore && !roundEnded)
                    {
                        roundEnded = true;
                        teamRoundsWon[kvp.Key]++;
                        Debug.Log($"{kvp.Key.teamName} won the round!");

                        bool isMatchWin = teamRoundsWon[kvp.Key] >= roundsToWin;

                        if (messageDisplayer != null)
                        {
                            if (isMatchWin)
                            {
                                messageDisplayer.UpdateRoundsWon(teamRoundsWon[leftTeam], teamRoundsWon[rightTeam]);
                                StartCoroutine(messageDisplayer.ShowFinalMatchWinner(kvp.Key.displayName));
                            }
                            else
                            {
                                StartCoroutine(messageDisplayer.ShowRoundWinner(kvp.Key.displayName));
                            }
                        }

                        if (isMatchWin)
                        {
                            Debug.Log($"{kvp.Key.teamName} wins the MATCH!");
                            enabled = false;
                            return;
                        }

                        StartCoroutine(ResetRound());
                        return;
                    }
                }
            }

            if (roundTimer <= 0f && !roundEnded)
            {
                roundEnded = true;

                TeamData winner = null;
                float topScore = -1f;
                foreach (var kvp in teamScores)
                {
                    if (kvp.Value > topScore)
                    {
                        winner = kvp.Key;
                        topScore = kvp.Value;
                    }
                }

                if (winner != null)
                {
                    teamRoundsWon[winner]++;
                    Debug.Log($"{winner.teamName} won the round (timeout)!");

                    bool isMatchWin = teamRoundsWon[winner] >= roundsToWin;

                    if (messageDisplayer != null)
                    {
                        if (isMatchWin)
                        {
                            messageDisplayer.UpdateRoundsWon(teamRoundsWon[leftTeam], teamRoundsWon[rightTeam]);
                            StartCoroutine(messageDisplayer.ShowFinalMatchWinner(winner.displayName));
                        }
                        else
                        {
                            StartCoroutine(messageDisplayer.ShowRoundWinner(winner.displayName));
                        }
                    }

                    if (isMatchWin)
                    {
                        Debug.Log($"{winner.teamName} wins the MATCH!");
                        enabled = false;
                        return;
                    }

                    StartCoroutine(ResetRound());
                }
            }
        }

        // Track hold times
        if (isHoldingFlag[leftTeam]) leftHoldTime += Time.deltaTime;
        if (isHoldingFlag[rightTeam]) rightHoldTime += Time.deltaTime;

        // Update new UI
        if (messageDisplayer != null)
        {
            float leftScore = teamScores[leftTeam];
            float rightScore = teamScores[rightTeam];
            bool leftLeads = leftScore > rightScore;
            bool scoresAreEqual = Mathf.Approximately(leftScore, rightScore);

            messageDisplayer.UpdateScoreUI(
                leftScore,
                rightScore,
                leftHoldTime,
                rightHoldTime,
                roundTimer,
                !scoresAreEqual && leftLeads
            );

            messageDisplayer.UpdateRoundsWon(teamRoundsWon[leftTeam], teamRoundsWon[rightTeam]);
        }
    }


    private void HandlePickup(Flag flag, PlayerFlagCarrier carrier)
    {
        TeamData flagOwner = flag.GetComponent<TeamIdentifier>()?.Team;
        TeamData carrierTeam = carrier.Team;

        if (flagOwner != null && carrierTeam != null && flagOwner != carrierTeam)
        {
            isHoldingFlag[carrierTeam] = true;
        }
    }

    private void HandleDrop(Flag flag, PlayerFlagCarrier carrier)
    {
        if (carrier.Team != null)
            isHoldingFlag[carrier.Team] = false;
    }

    private void HandleReturn(Flag flag)
    {
        foreach (var team in isHoldingFlag.Keys)
            isHoldingFlag[team] = false;
    }

    private IEnumerator ResetRound()
    {
        if (isResetting) yield break; // prevents multiple triggers
        isResetting = true;

        Debug.Log("Next round starting in 3 seconds...");

        // Show "Next round in 3…" or similar
        if (messageDisplayer != null)
            yield return StartCoroutine(messageDisplayer.ShowRoundStartCountdown());

        // Reset states
        roundEnded = false;
        roundTimer = roundDuration;
        leftHoldTime = 0f;
        rightHoldTime = 0f;

        foreach (var team in teams)
        {
            teamScores[team] = 0f;
            isHoldingFlag[team] = false;
        }

        foreach (var flag in allFlags)
            flag.ForceResetToBase();

        RebuildSpawnPointMap();

        foreach (var playerObj in PlayerSpawner.AllPlayers)
        {
            if (playerObj == null) continue;

            var carrier = playerObj.GetComponent<PlayerFlagCarrier>();
            if (carrier == null) continue;

            var team = carrier.Team;
            if (!spawnPoints.ContainsKey(team) || spawnPoints[team].Count == 0) continue;

            var spawn = spawnPoints[team][0];
            var controller = playerObj.GetComponent<CharacterController>();
            if (controller != null) controller.enabled = false;

            playerObj.transform.position = spawn.position;
            playerObj.transform.rotation = spawn.rotation;

            if (controller != null) controller.enabled = true;
        }

        allFlags.Clear();
        PlayerSpawner.AllPlayers.Clear();
        isResetting = false;
    }


    private void RebuildSpawnPointMap()
    {
        spawnPoints.Clear();

        foreach (var point in FindObjectsOfType<TeamSpawnPoint>())
        {
            if (!spawnPoints.ContainsKey(point.Team))
                spawnPoints[point.Team] = new List<Transform>();
            spawnPoints[point.Team].Add(point.transform);
        }
    }
}
