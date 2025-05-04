using UnityEngine;

public class PlayerJailHandler : MonoBehaviour {

    [SerializeField] private JailUIManager jailUIManager;
    [SerializeField] private float jailDuration = 10f;

    private bool isJailed = false;
    private float jailTimer = 0f;
    private string jailReason = "Jail"; // Default: non-trap

    private PlayerMovement playerMovement;

    private void Awake() {
        playerMovement = GetComponent<PlayerMovement>();
        jailUIManager = FindFirstObjectByType<JailUIManager>();

        if (playerMovement == null)
            Debug.LogError("[PlayerJailHandler] No PlayerMovement component found!");

        if (jailUIManager == null)
            Debug.LogWarning("[PlayerJailHandler] No JailUIManager found in scene.");
    }

    private void Update() {
        if (!isJailed) return;

        jailTimer -= Time.deltaTime;

        if (jailTimer <= 0f) {
            ReleaseFromJail();
        }
    }

    public void TriggerJail(string reason = "Jail", float durationOverride = -1f, float dizzyOverride = -1f) {
        if (isJailed) return;

        jailReason = reason;
        bool isTrap = (jailReason == "Trap");

        // Trap uses different duration
        float defaultDuration = isTrap ? 3f : jailDuration;
        jailTimer = (durationOverride > 0f) ? durationOverride : defaultDuration;

        isJailed = true;
        playerMovement.enabled = false;

        // Optional dizziness
        var dizzy = GetComponent<PlayerDizzyEffect>();
        if (dizzy != null && dizzyOverride > 0f)
            dizzy.ApplyDizziness(dizzyOverride);

        // UI logic
        jailUIManager?.ShowCaughtUI(jailTimer, isTrap);
    }


    private void ReleaseFromJail() {
        isJailed = false;
        playerMovement.enabled = true;

        bool isTrap = (jailReason == "Trap");
        jailUIManager?.ShowReleasedUI(jailTimer, isTrap);
    }
}
