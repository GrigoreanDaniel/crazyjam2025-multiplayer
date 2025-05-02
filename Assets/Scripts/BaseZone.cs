using System.Drawing;
using UnityEngine;

public class BaseZone : MonoBehaviour {
    [Header("Team Ownership")]
    [SerializeField] private string teamTag = "TeamA"; // This base belongs to Team A

    [Header("Settings")]
    [SerializeField] private bool debugLogs = true;

    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player")) return;

        FlagPickupHandler flagHandler = other.GetComponentInChildren<FlagPickupHandler>();
        if (flagHandler == null || !flagHandler.IsFlagHeld()) return;

        var playerTeam = other.GetComponent<TeamIdentifier>()?.TeamTag;
        var flagTeam = flagHandler.GetComponent<TeamIdentifier>()?.TeamTag;

        if (playerTeam != this.teamTag) {
            // Only capture if it's an enemy flag
            HandleFlagCapture(flagHandler);
        } else {
            Debug.Log("Player returned their own flag — no capture triggered.");
        }
    }


    private void HandleFlagCapture(FlagPickupHandler flagHandler) {
        FlagUIFeedbackManager.Instance.ShowMessage("Flag captured at base!"); // maybe add color ?
        FlagUIFeedbackManager.Instance.DisableFlagIcon();


        if (debugLogs) Debug.Log($"Flag captured at base: {teamTag}");

        // Optional: reset flag to spawn point
        flagHandler.DropFlag(flagHandler.GetSpawnPosition(), gameObject);
    }

    public void SetTeamTag(string tag) {
        teamTag = tag;
    }

}
