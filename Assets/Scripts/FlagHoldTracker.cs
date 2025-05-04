using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class FlagHoldTracker : MonoBehaviour {

    [SerializeField] private List<TeamData> teams;

    [SerializeField] private float scorePerSecond = 1f;
    [SerializeField] private float winScore = 100f;
    [SerializeField] private int roundsToWin = 2;

    [SerializeField] private float roundDuration = 300f; // 5 minutes
    private float roundTimer;

    [SerializeField] private GameUIController gameUI;

    [SerializeField, NonSerialized]
    private List<Flag> allFlags = new();

    private Dictionary<TeamData, int> teamRoundsWon = new();
    private Dictionary<TeamData, float> teamScores = new();
    private Dictionary<TeamData, bool> isHoldingFlag = new();

    private Dictionary<TeamData, List<Transform>> spawnPoints = new();

    private bool roundEnded = false;

    private void Awake() {
        foreach (var team in teams) {
            teamScores[team] = 0f;
            isHoldingFlag[team] = false;
            teamRoundsWon[team] = 0;
        }

        foreach (var point in FindObjectsOfType<TeamSpawnPoint>()) {
            if (!spawnPoints.ContainsKey(point.Team))
                spawnPoints[point.Team] = new List<Transform>();
            spawnPoints[point.Team].Add(point.transform);
        }
    }

    private void Start() {
        StartCoroutine(WaitAndFetchFlags());
        roundTimer = roundDuration;
    }

    private IEnumerator WaitAndFetchFlags() {
        yield return new WaitForSeconds(0.1f); // wait one frame or adjust if needed

        allFlags = new List<Flag>(FindObjectsOfType<Flag>());
        Debug.Log($"[FlagHoldTracker] Found {allFlags.Count} flag(s).");

        foreach (var flag in allFlags) {
            Debug.Log($" - {flag.name} at {flag.transform.position}");
        }
    }

    private void OnEnable() {
        FlagEvents.OnFlagPickedUp += HandlePickup;
        FlagEvents.OnFlagDropped += HandleDrop;
        FlagEvents.OnFlagReturned += HandleReturn;
    }

    private void OnDisable() {
        FlagEvents.OnFlagPickedUp -= HandlePickup;
        FlagEvents.OnFlagDropped -= HandleDrop;
        FlagEvents.OnFlagReturned -= HandleReturn;
    }

    private void Update() {
        if (!roundEnded) {
            roundTimer -= Time.deltaTime;
            roundTimer = Mathf.Max(0, roundTimer); // prevent negatives

            // Update UI timer
            if (gameUI != null) {
                gameUI.UpdateRoundClock(roundTimer);
            }

            // Check if timer hit zero
            if (roundTimer <= 0f) {
                roundEnded = true;

                // Determine which team has the highest score
                TeamData winner = null;
                float topScore = -1f;

                foreach (var kvp in teamScores) {
                    if (kvp.Value > topScore) {
                        winner = kvp.Key;
                        topScore = kvp.Value;
                    }
                }

                if (winner != null) {
                    teamRoundsWon[winner]++;
                    Debug.Log($"{winner.teamName} won the round (timeout)!");

                    if (teamRoundsWon[winner] >= roundsToWin) {
                        Debug.Log($"{winner.teamName} wins the MATCH!");
                        // TODO: trigger final win screen
                    } else {
                        StartCoroutine(ResetRound());
                    }
                }
            }
        }

        // Also handle normal flag scoring...
        foreach (var kvp in isHoldingFlag) {
            if (kvp.Value) {
                teamScores[kvp.Key] += scorePerSecond * Time.deltaTime;
                if (!roundEnded && teamScores[kvp.Key] >= winScore) {
                    roundEnded = true;
                    teamRoundsWon[kvp.Key]++;
                    Debug.Log($"{kvp.Key.teamName} won the round!");

                    if (teamRoundsWon[kvp.Key] >= roundsToWin) {
                        Debug.Log($"{kvp.Key.teamName} wins the MATCH!");
                    } else {
                        StartCoroutine(ResetRound());
                    }
                }
            }
        }

        // Debug prints if needed
    }


    private void HandlePickup(Flag flag, PlayerFlagCarrier carrier) {
        TeamData flagOwner = flag.GetComponent<TeamIdentifier>()?.Team;
        TeamData carrierTeam = carrier.Team;

        if (flagOwner != null && carrierTeam != null && flagOwner != carrierTeam) {
            isHoldingFlag[carrierTeam] = true;
        }
    }

    private void HandleDrop(Flag flag, PlayerFlagCarrier carrier) {
        TeamData carrierTeam = carrier.Team;
        if (carrierTeam != null)
            isHoldingFlag[carrierTeam] = false;
    }

    private void HandleReturn(Flag flag) {
        // Stop everyone from scoring with that flag
        foreach (var team in isHoldingFlag.Keys)
            isHoldingFlag[team] = false;
    }

    public float GetScore(TeamData team) => teamScores.ContainsKey(team) ? teamScores[team] : 0f;

    private IEnumerator ResetRound() {
        Debug.Log("Next round starting in 3 seconds...");
        yield return new WaitForSeconds(3f);

        roundTimer = roundDuration;

        if (gameUI != null)
            gameUI.UpdateRoundClock(roundTimer);

        foreach (var team in teams) {
            teamScores[team] = 0f;
            isHoldingFlag[team] = false;
        }

        foreach (var flag in allFlags) {
            flag.ForceResetToBase();
        }

        RebuildSpawnPointMap();
        foreach (var kvp in spawnPoints) {
            Debug.Log($"SpawnPoints[{kvp.Key.teamName}] = {kvp.Value.Count} entries");
        }

        foreach (var playerObj in PlayerSpawner.AllPlayers) {
            if (playerObj == null) {
                Debug.LogWarning("[ResetRound] Found null player object in list.");
                continue;
            }

            Debug.Log($"[ResetRound] Attempting to move: {playerObj.name}");

            var carrier = playerObj.GetComponent<PlayerFlagCarrier>();
            if (carrier == null) {
                Debug.LogWarning($"[ResetRound] Player {playerObj.name} missing PlayerFlagCarrier.");
                continue;
            }

            var team = carrier.Team;
            if (!spawnPoints.ContainsKey(team) || spawnPoints[team].Count == 0) {
                Debug.LogWarning($"[ResetRound] No spawn points for team {team.teamName}.");
                continue;
            }

            var spawn = spawnPoints[team][0];
            Debug.Log($"[ResetRound] Moving {playerObj.name} to {spawn.position}");

            var controller = playerObj.GetComponent<CharacterController>();
            if (controller != null) controller.enabled = false;

            playerObj.transform.position = spawn.position;
            playerObj.transform.rotation = spawn.rotation;

            if (controller != null) controller.enabled = true;
        }



        allFlags.Clear(); // if you re-spawn flags each round
        PlayerSpawner.AllPlayers.Clear();
        roundEnded = false;
    }

    private void RebuildSpawnPointMap() {
        spawnPoints.Clear();

        foreach (var point in FindObjectsOfType<TeamSpawnPoint>()) {
            if (!spawnPoints.ContainsKey(point.Team))
                spawnPoints[point.Team] = new List<Transform>();

            spawnPoints[point.Team].Add(point.transform);
        }

        Debug.Log($"[FlagHoldTracker] Updated spawn point map with {spawnPoints.Count} team(s).");
    }

    /* public void ShowRoundWinMessage(string message) {
         roundMessageText.text = message;
         roundMessageText.gameObject.SetActive(true);
         StartCoroutine(HideMessageAfterSeconds(2f));
     }

     private IEnumerator HideMessageAfterSeconds(float t) {
         yield return new WaitForSeconds(t);
         roundMessageText.gameObject.SetActive(false);
     }*/

}