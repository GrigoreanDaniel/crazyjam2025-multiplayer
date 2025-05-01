using UnityEngine;
using System.Collections;

public class FlagReturnTimer : MonoBehaviour {
    [Header("Return Settings")]
    [SerializeField] private float returnDelay = 10f;

    [Header("References")]
    [SerializeField] private GameObject crownObject;
    [SerializeField] private FlagBeaconController beaconController;

    private Vector3 spawnPosition;
    private Quaternion spawnRotation;
    private Coroutine returnCoroutine;

    private void Start() {
        // Cache initial position and rotation
        spawnPosition = transform.position;
        spawnRotation = transform.rotation;
    }

    public void StartReturnCountdown() {
        if (returnCoroutine != null) {
            StopCoroutine(returnCoroutine);
        }
        returnCoroutine = StartCoroutine(ReturnAfterDelay());
    }

    public void CancelReturnCountdown() {
        if (returnCoroutine != null) {
            StopCoroutine(returnCoroutine);
            returnCoroutine = null;
        }
    }

    private IEnumerator ReturnAfterDelay() {
        yield return new WaitForSeconds(returnDelay);

        // Unparent flag if it's still on a player
        crownObject.transform.SetParent(null);
        crownObject.transform.position = spawnPosition;
        crownObject.transform.rotation = spawnRotation;

        if (beaconController != null) {
            beaconController.DetachFromCarrier(spawnPosition);
        }

        FindObjectOfType<FlagUIFeedbackManager>()?.ShowMessage("Flag Returned to Base");
        // Reset local state if needed (optional)
        Debug.Log("Flag auto-returned to spawn.");
    }
}
