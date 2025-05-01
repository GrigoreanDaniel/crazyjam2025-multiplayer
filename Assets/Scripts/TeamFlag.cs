using UnityEngine;

public class TeamFlag : MonoBehaviour {
    [SerializeField] private string teamTag;
    [SerializeField] private MeshRenderer flagRenderer;
    [SerializeField] private float yOffset = 0.0f;

    private Vector3 spawnPosition;
    private Quaternion spawnRotation;

    public void InitializeFlag(string teamTag, Material teamMaterial) {
        this.teamTag = teamTag;
        spawnPosition = transform.position;
        spawnRotation = transform.rotation;

        if (flagRenderer != null && teamMaterial != null) {
            flagRenderer.material = teamMaterial;
        }

        transform.position = new Vector3(transform.position.x, transform.position.y + yOffset, transform.position.z);
    }

    public string GetTeamTag() => teamTag;
    public Vector3 GetSpawnPosition() => spawnPosition;
}
