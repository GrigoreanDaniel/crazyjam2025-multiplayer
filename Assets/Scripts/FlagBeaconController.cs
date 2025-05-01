using UnityEngine;

public class FlagBeaconController : MonoBehaviour {
    [Header("Beacon Settings")]
    [SerializeField] private GameObject beaconVisual;
    [SerializeField] private Color teamColor = Color.white;
    [SerializeField] private float followYOffset = 2.5f;

    [Header("Tracking")]
    [SerializeField] private bool followCarrier = false;
    private Transform flagCarrier;

    private void Start() {
        if (beaconVisual != null) {
            SetBeaconColor(teamColor);
            // beaconVisual.SetActive(true);
        }
    }

    private void Update() {
        if (followCarrier && flagCarrier != null) {
            Vector3 targetPos = flagCarrier.position + Vector3.up * followYOffset;
            transform.position = targetPos;
        }
    }

    public void AttachToCarrier(Transform carrier) {
        flagCarrier = carrier;
        followCarrier = true;

        if (beaconVisual != null)
            beaconVisual.SetActive(true);
    }

    public void DetachFromCarrier(Vector3 flagGroundPosition) {
        followCarrier = false;
        flagCarrier = null;
        transform.position = flagGroundPosition;

        if (beaconVisual != null)
            beaconVisual.SetActive(true); // Still visible on ground
    }


    private void SetBeaconColor(Color color) {
        if (beaconVisual.TryGetComponent<Renderer>(out Renderer renderer)) {
            renderer.material.color = color;
        }
    }
}
