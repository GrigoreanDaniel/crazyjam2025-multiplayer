using UnityEngine;

public class TeamFlag : MonoBehaviour {
    [SerializeField] private MeshRenderer flagRenderer;
    [SerializeField] private TeamData team;
    [SerializeField] private float yOffset = 0f;

    private void Start() {
        var pos = transform.position;
        transform.position = new Vector3(pos.x, pos.y + yOffset, pos.z);
    }

    public TeamData Team => team;
    public Color GetColor() => team.teamColor;
}