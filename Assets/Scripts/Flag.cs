using UnityEngine;
using System;
using System.Collections;

public class Flag : MonoBehaviour {
    public TeamData owningTeam;

    public event Action<Flag, PlayerFlagCarrier> OnPickedUp;
    public event Action<Flag, PlayerFlagCarrier> OnDropped;
    public event Action<Flag> OnReturnedToBase;

    [SerializeField] public Transform basePosition;

    private bool isAtBase = true;
    private PlayerFlagCarrier currentCarrier;

    private Coroutine autoReturnCoroutine;
    [SerializeField] private float autoReturnDelay = 10f;

    private void Update() {
        if (currentCarrier == null)
            transform.Rotate(Vector3.up * 60f * Time.deltaTime);
    }

    public bool IsAtBase() => isAtBase;

    public bool TryPickup(PlayerFlagCarrier carrier) {
        if (!IsPickupAllowed(carrier)) return false;

        currentCarrier = carrier;
        transform.SetParent(carrier.transform);
        transform.localPosition = carrier.GetFlagOffset();
        isAtBase = false;

        FlagEvents.OnFlagPickedUp?.Invoke(this, carrier);

        if (autoReturnCoroutine != null) {
            StopCoroutine(autoReturnCoroutine);
            autoReturnCoroutine = null;
        }
        return true;
    }


    public void Drop() {
        if (currentCarrier == null) return;

        FlagEvents.OnFlagDropped?.Invoke(this, currentCarrier);

        transform.SetParent(null);
        currentCarrier = null;
        isAtBase = false;

        // Start auto-return countdown
        if (autoReturnCoroutine != null)
            StopCoroutine(autoReturnCoroutine);

        autoReturnCoroutine = StartCoroutine(AutoReturnAfterDelay());
    }

    public void ReturnToBase() {
        var handler = FlagEvents.OnFlagReturned;
        if (handler != null) {
            // Safely create a list copy
            var subscribers = handler.GetInvocationList();
            for (int i = 0; i < subscribers.Length; i++) {
                try {
                    ((Action<Flag>)subscribers[i])(this);
                } catch (Exception e) {
                    Debug.LogWarning($"[Flag.cs] FlagReturned handler failed: {e.TargetSite?.DeclaringType?.Name} threw '{e.Message}'");
                }
            }
        }

        transform.SetParent(null);
        currentCarrier = null;
        isAtBase = true;
        transform.position = basePosition.position;

        if (autoReturnCoroutine != null) {
            StopCoroutine(autoReturnCoroutine);
            autoReturnCoroutine = null;
        }
    }

    private bool IsPickupAllowed(PlayerFlagCarrier carrier) {
        TeamIdentifier flagTeam = GetComponent<TeamIdentifier>();
        if (flagTeam == null || flagTeam.Team == null) return true; // Neutral flag

        return flagTeam.Team != carrier.Team; // Allow if enemy team
    }

    private IEnumerator AutoReturnAfterDelay() {
        yield return new WaitForSeconds(autoReturnDelay);
        ReturnToBase();
        autoReturnCoroutine = null;
    }

    public void ForceResetToBase() {
        if (currentCarrier != null) {
            currentCarrier.DropFlag();  // Cleanly detach
            currentCarrier = null;
        }

        isAtBase = true;

        transform.SetParent(null);
        transform.position = basePosition.position;
        transform.rotation = basePosition.rotation; // Optional, but good to reset

        if (autoReturnCoroutine != null)
            StopCoroutine(autoReturnCoroutine);
        autoReturnCoroutine = null;

        gameObject.SetActive(true); // Ensure visibility
    }

}
