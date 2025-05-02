using UnityEngine;

public class LocalPlayerTracker : MonoBehaviour {
    public static LocalPlayerTracker Instance { get; private set; }

    private string playerTeamTag;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    void Start() {
        var team = GetComponent<TeamIdentifier>()?.TeamTag;
        LocalPlayerTracker.Instance.SetTeam(team);

        Debug.Log($"[LocalTracker] Local player team set to: {team}");
        Debug.Log("[Tracker] My final team is: " + team);  // fixed
    }


    public void SetTeam(string teamTag) {
        playerTeamTag = teamTag;
    }

    public string GetTeam() {
        return playerTeamTag;
    }
}
