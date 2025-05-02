using UnityEngine;
using System.Collections;

public class FlagReturnTimer : MonoBehaviour {
    [Header("Return Settings")]
    [SerializeField] private float returnDelay = 10f;

    [Header("References")]
    [SerializeField] private GameObject crownObject;
    [SerializeField] private FlagBeaconController beaconController;

    private string lastCarrierTeamTag;

    private Vector3 spawnPosition;
    private Quaternion spawnRotation;
    private Coroutine returnCoroutine;

    private void Start() {
        // Cache initial position and rotation
        spawnPosition = transform.position;
        spawnRotation = transform.rotation;
    }

    public void SetSpawn(Vector3 pos, Quaternion rot) {
        spawnPosition = pos;
        spawnRotation = rot;
        Debug.Log("Setting spawn to: " + pos);
    }

    public void StartReturnCountdown(GameObject player) {
        if (returnCoroutine != null)
            StopCoroutine(returnCoroutine);

        lastCarrierTeamTag = player.GetComponent<TeamIdentifier>()?.TeamTag;
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
        Debug.Log($"[ReturnTimer] Returning flag to spawn: ({spawnPosition})");

        // Unparent flag if it's still on a player
        if (crownObject != null) {
            Debug.Log($"[ReturnTimer] Returning flag to spawn: {spawnPosition}");
            crownObject.transform.SetParent(null);
            crownObject.transform.position = spawnPosition;
            Debug.Log($"[ReturnTimer] Actual crown position after move: {crownObject.transform.position}");
            crownObject.transform.rotation = spawnRotation;
        }

        if (beaconController != null) {
            beaconController.DetachFromCarrier(spawnPosition);
            beaconController.DisableBeacon();
        }

        // Pop up message
        Material teamMat = GetComponent<TeamFlag>()?.GetMaterial();
        Color color = teamMat != null ? teamMat.color : Color.white;

        string flagTeam = GetComponent<TeamIdentifier>()?.TeamTag;
        string localTeam = LocalPlayerTracker.Instance?.GetTeam();

        Debug.Log($"[ReturnTimer] Returning flag. Flag Team: {flagTeam} | Local Player Team: {localTeam}");

        if (flagTeam == localTeam) {
            FlagUIFeedbackManager.Instance.ShowMessage("Your flag returned!", color);
            FlagUIFeedbackManager.Instance.DisableFlagIcon();
        } else {
            FlagUIFeedbackManager.Instance.ShowMessage("Enemy's flag returned!", color);
        }

        Debug.Log($"[ReturnTimer] I'm on GameObject: {gameObject.name} | Flag Team: {flagTeam}");

        var id = GetComponent<TeamIdentifier>();

        if (string.IsNullOrEmpty(localTeam)) {
            Debug.LogWarning("LocalPlayerTracker team not set!");
        }

        // Disable the Flag Icon
        FlagUIFeedbackManager.Instance.DisableFlagIcon();

        // Reset local state if needed (optional)
        Debug.Log("Flag auto-returned to spawn.");
    }
}
