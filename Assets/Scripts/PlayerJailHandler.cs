using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJailHandler : MonoBehaviour{

    [SerializeField] private JailUIManager jailUIManager;
    [SerializeField] private float jailDuration = 10f;
    [SerializeField] private FlagPickupHandler flagPickupHandler;

    private bool isJailed = false;
    private float jailTimer = 0f;
    private string jailReason = "Jail"; // default

    private PlayerMovement playerMovement; // No SerializeField, handled automatically

    private void Awake(){

        // Auto-fetch PlayerMovement from the same GameObject
        playerMovement = GetComponent<PlayerMovement>();
        jailUIManager = FindObjectOfType<JailUIManager>();

        if (playerMovement == null){

            Debug.LogError("PlayerMovement component not found on Player! Jail system won't work!");
        }
    }

    private void Update(){

        if (!isJailed) return;

        jailTimer -= Time.deltaTime;

        if (jailTimer <= 0f){

            ReleaseFromJail();
            jailUIManager.HideJailUI();
        }
    }

    public void TriggerJail(string reason = "Jail", float durationOverride = -1f, float dizzyOverride = -1f) {
        if (isJailed) return;

        jailReason = reason;
        isJailed = true;
        jailTimer = (durationOverride > 0) ? durationOverride : jailDuration;
        playerMovement.enabled = false;

        if (flagPickupHandler != null)
            flagPickupHandler.DropFlag(transform.position);

        PlayerDizzyEffect dizzy = GetComponent<PlayerDizzyEffect>();
        if (dizzy != null && dizzyOverride > 0)
            dizzy.ApplyDizziness(dizzyOverride);

        if (jailUIManager != null)
            jailUIManager.ShowJailUI(jailTimer, jailReason);
    }


    private void ReleaseFromJail() {
        isJailed = false;
        playerMovement.enabled = true;

        
        if (jailUIManager != null)
            jailUIManager.HideJailUI();
    }


    public void AssignFlagReference(FlagPickupHandler handler) {
        flagPickupHandler = handler;
    }

}
