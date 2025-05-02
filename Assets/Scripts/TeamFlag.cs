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

    public void InitializeFlag(string teamTag, Material teamMaterial) {
        this.teamTag = teamTag;

        var identifier = GetComponent<TeamIdentifier>();
        if (identifier != null) {
            identifier.SetTeamTag(teamTag);
            Debug.Log($"[TeamFlag] Set TeamIdentifier to {teamTag} on {gameObject.name}");
        }

        if (flagRenderer != null && teamMaterial != null) {
            flagRenderer.material = teamMaterial;
        }
    }

    private void Start() {
        // Adjust Y offset after parenting
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + yOffset, transform.localPosition.z);
    }

    public string GetTeamTag() => teamIdentifier != null ? teamIdentifier.TeamTag : "";
    public Vector3 GetSpawnPosition() => spawnPosition;
    public Material GetMaterial() => flagRenderer.material;
}
