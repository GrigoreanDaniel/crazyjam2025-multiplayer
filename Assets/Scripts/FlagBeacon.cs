using UnityEngine;

public class FlagBeacon : MonoBehaviour {
    [SerializeField] private Renderer beaconRenderer;

    public void SetColor(Color color) {
        beaconRenderer.material.color = color;
        if (beaconRenderer.material.HasProperty("_EmissionColor"))
            beaconRenderer.material.SetColor("_EmissionColor", color);
    }
}
