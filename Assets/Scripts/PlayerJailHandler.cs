using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJailHandler : MonoBehaviour{

    [SerializeField] private JailUIManager jailUIManager;
    [SerializeField] private float jailDuration = 10f;
    [SerializeField] private FlagPickupHandler flagPickupHandler;

    private bool isJailed = false;
    private float jailTimer = 0f;

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

    public void TriggerJail(){

        if (isJailed) return;

        isJailed = true;
        jailTimer = jailDuration;
        playerMovement.enabled = false;

        // Drop flag if holding one
        if (flagPickupHandler != null) {
            Debug.Log("Jail: flagPickupHandler is on object: " + flagPickupHandler.gameObject.name);
            flagPickupHandler.DropFlag(transform.position);
        } else {
            Debug.LogWarning("Jail: No FlagPickupHandler assigned!");
        }



        if (jailUIManager != null){

            jailUIManager.ShowJailUI(jailDuration);
        }
    }

    private void ReleaseFromJail(){

        isJailed = false;
        playerMovement.enabled = true;
        if (jailUIManager != null){

            jailUIManager.HideJailUI();
        }
    }

    public void AssignFlagReference(FlagPickupHandler handler) {
        flagPickupHandler = handler;
    }

}
