using UnityEngine;
using System;

public class FlagPickupHandler : MonoBehaviour{

    [Header("Flag Settings")]
    [SerializeField] private GameObject crownObject; // The flame/crown visual
    [SerializeField] private float pickupCooldown = 5f;

    [Header("Mount Point")]
    [SerializeField] private Transform crownMountPoint; // Where to attach crown

    [Header("Optional Events")]
    public Action OnFlagPickedUp;
    public Action OnFlagAvailable;

    private bool isFlagHeld = false;
    private bool isPickupAvailable = true;
    private float cooldownTimer = 0f;

    private void Update(){

        if (!isPickupAvailable){

            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f){

                isPickupAvailable = true;
                OnFlagAvailable?.Invoke();
            }
        }
    }

    private void OnTriggerEnter(Collider other){

        if (!isPickupAvailable || isFlagHeld) return;

        if (other.CompareTag("Player")){

            AttachCrownToPlayer(other.transform);
        }
    }

    private void AttachCrownToPlayer(Transform player){

        // Look for mount point inside player
        Transform mount = player.Find("Crown Mount Point");
        if (mount == null){

            Debug.LogWarning("Player is missing a Crown Mount Point.");
            return;
        }

        crownObject.transform.SetParent(mount);
        crownObject.transform.localPosition = Vector3.zero;
        crownObject.transform.localRotation = Quaternion.identity;

        isFlagHeld = true;
        isPickupAvailable = false;
        cooldownTimer = pickupCooldown;

        OnFlagPickedUp?.Invoke();
        //gameObject.SetActive(false); // Disable pickup trigger
    }
}
