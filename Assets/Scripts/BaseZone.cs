using UnityEngine;

public class BaseZone : MonoBehaviour {
    [Header("Team Ownership")]
    [SerializeField] private string teamTag = "TeamA"; // This base belongs to Team A

    [Header("Settings")]
    [SerializeField] private bool debugLogs = true;

    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player")) return;

        // Get flag handler from the global Flag Node
        FlagPickupHandler flagHandler = FindObjectOfType<FlagPickupHandler>();

        if (flagHandler == null) {
            Debug.LogWarning("No FlagPickupHandler found.");
            return;
        }

        // Check if flag is held (assumes public property or method exists)
        bool flagIsHeld = flagHandler.IsFlagHeld();

        if (!flagIsHeld) {
            if (debugLogs) Debug.Log("Player entered base, but no flag is held.");
            return;
        }

        HandleFlagCapture(flagHandler);
    }

    private void HandleFlagCapture(FlagPickupHandler flagHandler) {
        FindObjectOfType<FlagUIFeedbackManager>()?.ShowMessage("Flag Captured at base!");

        if (debugLogs) Debug.Log($"Flag captured at base: {teamTag}");

        // Optional: reset flag to spawn point
        flagHandler.DropFlag(flagHandler.GetSpawnPosition());
    }

    public void SetTeamTag(string tag) {
        teamTag = tag;
    }

}
