using UnityEngine;
using System.Collections.Generic;

public class FlagHoldTracker : MonoBehaviour {
    [SerializeField] private List<TeamData> teams;
    [SerializeField] private float scorePerSecond = 1f;
    [SerializeField] private float winScore = 500f;

    private Dictionary<TeamData, float> teamScores = new();
    private Dictionary<TeamData, bool> isHoldingFlag = new();

    private void Awake() {
        foreach (var team in teams) {
            teamScores[team] = 0f;
            isHoldingFlag[team] = false;
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
        foreach (var kvp in isHoldingFlag) {
            if (kvp.Value) {
                teamScores[kvp.Key] += scorePerSecond * Time.deltaTime;
                if (teamScores[kvp.Key] >= winScore) {
                    Debug.Log($"{kvp.Key.teamName} wins!");
                    // TODO: trigger end-game
                }
            }
        }
        foreach (var team in teams) {
            //Debug.Log($"{team.teamName}: {teamScores[team]:0.0}s");
        }
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
}
