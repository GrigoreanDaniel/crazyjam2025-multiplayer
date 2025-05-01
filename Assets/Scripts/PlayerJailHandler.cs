using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJailHandler : MonoBehaviour{

    [SerializeField] private JailUIManager jailUIManager;
    [SerializeField] private float jailDuration = 10f;

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
}
