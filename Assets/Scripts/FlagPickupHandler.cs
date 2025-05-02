using UnityEngine;
using System;

public class FlagPickupHandler : MonoBehaviour {
    [Header("Flag Settings")]
    [SerializeField] private GameObject crownObject; // The flame/crown visual
    [SerializeField] private float pickupCooldown = 5f;

    [Header("Mount Point")]
    [SerializeField] private Transform crownMountPoint; // Where to attach crown

    [Header("Beacon Control")]
    [SerializeField] private FlagBeaconController beaconController;

    [SerializeField] private FlagReturnTimer returnTimer;

    [Header("Optional Events")]
    public Action OnFlagPickedUp;
    public Action OnFlagAvailable;

    private bool isFlagHeld = false;
    private bool isPickupAvailable = true;
    private float cooldownTimer = 0f;

    private Vector3 spawnPosition;

    private void Start() {
        spawnPosition = transform.position;
    }

    private void Update() {
        if (!isPickupAvailable) {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f) {
                isPickupAvailable = true;
                OnFlagAvailable?.Invoke();
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (!isPickupAvailable || isFlagHeld) return;

        if (other.CompareTag("Player")) {
            AttachCrownToPlayer(other.transform);
        }
    }

    private void AttachCrownToPlayer(Transform player) {
        // Look for mount point inside player
        Transform mount = player.Find("Crown Mount Point");
        if (mount == null) {
            Debug.LogWarning("Player is missing a Crown Mount Point.");
            return;
        }

        // Move crown to player
        crownObject.transform.SetParent(mount);
        crownObject.transform.localPosition = Vector3.zero;
        crownObject.transform.localRotation = Quaternion.identity;

        // Beacon follows the player
        if (beaconController != null) {
            beaconController.AttachToCarrier(player);
        }

        Debug.Log("ATTACHING crown to player: " + player.name);

        isFlagHeld = true;
        isPickupAvailable = false;
        cooldownTimer = pickupCooldown;

        var playerTeam = player.GetComponent<TeamIdentifier>()?.TeamTag;
        var flagTeam = GetComponent<TeamIdentifier>()?.TeamTag;

        Material mat = GetComponent<TeamFlag>().GetMaterial();
        Color teamColor = mat.color; // already correct

        if (playerTeam == flagTeam) {
            FlagUIFeedbackManager.Instance.ShowMessage("You picked up your flag!", teamColor);
            FlagUIFeedbackManager.Instance.EnableFlagIcon(teamColor);
        } else {
            FlagUIFeedbackManager.Instance.ShowMessage("You picked up enemy's flag!", teamColor);
            FlagUIFeedbackManager.Instance.EnableFlagIcon(teamColor);
        }

        OnFlagPickedUp?.Invoke();

        PlayerFlagInput input = player.GetComponent<PlayerFlagInput>();
        if (input != null) {
            input.AssignFlagReference(this); // `this` = this instance of FlagPickupHandler
        }

        PlayerJailHandler jail = player.GetComponent<PlayerJailHandler>();
        if (jail != null) {
            jail.AssignFlagReference(this);
        }


        // Cancel return timer when picked up
        FindObjectOfType<FlagReturnTimer>()?.CancelReturnCountdown();
    }

    public void DropFlag(Vector3 dropPosition, GameObject player) {
        if (!isFlagHeld) {
            Debug.LogWarning("Tried to drop flag, but isFlagHeld == false");
            return;
        }

        Debug.Log("Dropping flag at: " + dropPosition);

        // Unparent the crown
        crownObject.transform.SetParent(null);
        crownObject.transform.position = dropPosition;

        if (beaconController != null) {
            beaconController.DetachFromCarrier(dropPosition);
        }

        isFlagHeld = false;
        isPickupAvailable = false;
        cooldownTimer = pickupCooldown;

        FlagUIFeedbackManager.Instance.DisableFlagIcon();

        OnFlagAvailable?.Invoke();

        // Start return timer
        FindObjectOfType<FlagReturnTimer>()?.StartReturnCountdown(player);
    }

    public bool IsFlagHeld() {
        return isFlagHeld;
    }

    public Vector3 GetSpawnPosition() {
        return spawnPosition; // You may need to define this
    }

}
