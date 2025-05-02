using UnityEngine;

[RequireComponent(typeof(TeamIdentifier))]
public class TeamFlag : MonoBehaviour {
    [SerializeField] private MeshRenderer flagRenderer;
    [SerializeField] private float yOffset = 0.0f;

    private Vector3 spawnPosition;
    private Quaternion spawnRotation;
    private TeamIdentifier teamIdentifier;

    private string teamTag;

    private void Awake() {
        teamIdentifier = GetComponent<TeamIdentifier>();
    }

    // Inside TeamFlag.cs
    public void InitializeFlag(string teamTag, Material teamMaterial) {
        this.teamTag = teamTag; // Internal value

        // Ensure the TeamIdentifier is also updated
        var identifier = GetComponent<TeamIdentifier>();
        if (identifier != null) {
            identifier.SetTeamTag(teamTag);
        }

        if (flagRenderer != null && teamMaterial != null) {
            flagRenderer.material = teamMaterial;
        }

        transform.position = new Vector3(transform.position.x, transform.position.y + yOffset, transform.position.z);
    }


    public string GetTeamTag() => teamIdentifier != null ? teamIdentifier.TeamTag : "";

    public Vector3 GetSpawnPosition() => spawnPosition;

    public Material GetMaterial() => flagRenderer.material;

}
