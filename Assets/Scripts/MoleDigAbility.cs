using UnityEngine;

public class MoleDigAbility : MonoBehaviour {
    [Header("Dig Parameters")]
    [SerializeField, Tooltip("How long the player stays underground.")]
    private float digDuration = 5f;

    [SerializeField, Tooltip("Cooldown after using Dig.")]
    private float cooldownDuration = 10f;

    [SerializeField, Tooltip("Key used to trigger Dig.")]
    private KeyCode digKey = KeyCode.Q;

    [Header("References")]
    [SerializeField] private DigVisualController digVisuals;

    private bool isDigging = false;
    private bool isOnCooldown = false;

    private float digTimer = 0f;
    private float cooldownTimer = 0f;

    private void Update() {
        HandleInput();
        UpdateTimers();
    }

    private void HandleInput() {
        if (Input.GetKeyDown(digKey) && !isDigging && !isOnCooldown) {
            BeginDig();
        }
    }

    private void UpdateTimers() {
        if (isDigging) {
            digTimer -= Time.deltaTime;
            if (digTimer <= 0f) {
                EndDig();
            }
        }

        if (isOnCooldown) {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f) {
                isOnCooldown = false;
            }
        }
    }

    private void BeginDig() {
        isDigging = true;
        digTimer = digDuration;
        digVisuals.ApplyDigState(true);
        // TODO: Photon sync: BroadcastBeginDig()
    }

    private void EndDig() {
        isDigging = false;
        isOnCooldown = true;
        cooldownTimer = cooldownDuration;
        digVisuals.ApplyDigState(false);
        // TODO: Photon sync: BroadcastEndDig()
    }

    public bool IsDigging => isDigging;
    public bool IsOnCooldown => isOnCooldown;
}
