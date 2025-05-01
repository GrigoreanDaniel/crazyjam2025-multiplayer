using UnityEngine;

public class FlagBeaconController : MonoBehaviour {
    [Header("Beacon Settings")]
    [SerializeField] private GameObject beaconVisual;
    [SerializeField] private float followYOffset = 2.5f;

    [SerializeField] private bool showBeaconWhenDropped = true;

    [Header("Tracking")]
    [SerializeField] private bool followCarrier = false;
    private Transform flagCarrier;

    private void Start() {
        if (beaconVisual != null) {
            beaconVisual.SetActive(false); // Beacon hidden by default
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
            beaconVisual.SetActive(showBeaconWhenDropped);
    }

    public void SetBeaconColor(Color color) {
        if (beaconVisual.TryGetComponent<Renderer>(out Renderer renderer)) {
            Material runtimeMat = renderer.material; // Make sure it's a unique instance

            // Set for URP Lit Shader
            if (runtimeMat.HasProperty("_BaseColor"))
                runtimeMat.SetColor("_BaseColor", color);

            if (runtimeMat.HasProperty("_EmissionColor"))
                runtimeMat.SetColor("_EmissionColor", color * 2f); // Optional glow
        }
    }

    public void DisableBeacon() {
        if (beaconVisual != null)
            beaconVisual.SetActive(false);
    }

}
