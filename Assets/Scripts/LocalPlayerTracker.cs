using UnityEngine;

public class LocalPlayerTracker : MonoBehaviour {
    public static LocalPlayerTracker Instance { get; private set; }

    public TeamData MyTeam { get; private set; }

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start() {
        var teamComp = GetComponent<TeamIdentifier>();
        if (teamComp != null) {
            MyTeam = teamComp.Team;
        }
    }
}