using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJailHandler : MonoBehaviour {

    [SerializeField] private JailUIManager jailUIManager;
    [SerializeField] private float jailDuration = 10f;
    //[SerializeField] private FlagPickupHandler flagPickupHandler;

    private bool isJailed = false;
    private float jailTimer = 0f;
    private string jailReason = "Jail"; // default

    private PlayerMovement playerMovement; // No SerializeField, handled automatically

    private void Awake() {

        // Auto-fetch PlayerMovement from the same GameObject
        playerMovement = GetComponent<PlayerMovement>();
        jailUIManager = FindObjectOfType<JailUIManager>();

        if (playerMovement == null) {

            Debug.LogError("PlayerMovement component not found on Player! Jail system won't work!");
        }
    }

    private void Update() {

        if (!isJailed) return;

        jailTimer -= Time.deltaTime;

        if (jailTimer <= 0f) {

            ReleaseFromJail();
            //jailUIManager.HideAll();
        }
    }

    public void TriggerJail(string reason = "Jail", float durationOverride = -1f, float dizzyOverride = -1f) {
        if (isJailed) return;

        jailReason = reason;
        isJailed = true;
        jailTimer = (durationOverride > 0) ? durationOverride : jailDuration;
        playerMovement.enabled = false;

        PlayerDizzyEffect dizzy = GetComponent<PlayerDizzyEffect>();
        if (dizzy != null && dizzyOverride > 0)
            dizzy.ApplyDizziness(dizzyOverride);

        bool isTrap = (jailReason == "Trap");

        if (jailUIManager != null)
            jailUIManager.ShowCaughtUI(jailTimer, isTrap);
    }

    private void ReleaseFromJail() {
        isJailed = false;
        playerMovement.enabled = true;

        bool isTrap = (jailReason == "Trap");
        jailUIManager.ShowReleasedUI(isTrap);

        /*if (jailUIManager != null)
            jailUIManager.HideAll();*/
    }


    /*public void AssignFlagReference(FlagPickupHandler handler) {
        flagPickupHandler = handler;
    }*/

}
