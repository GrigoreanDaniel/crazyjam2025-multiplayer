using UnityEngine;

public class FlagReturnZone : MonoBehaviour {
    private TeamData zoneTeam;

    private void Awake() {
        var teamId = GetComponent<TeamIdentifier>();
        if (teamId != null)
            zoneTeam = teamId.Team;
    }

    private void OnTriggerEnter(Collider other) {
        Flag flag = other.GetComponent<Flag>();
        if (flag == null) return;

        var flagTeam = flag.GetComponent<TeamIdentifier>()?.Team;
        if (flagTeam != zoneTeam) return; // Only return own flag

        if (!flag.IsAtBase()) // Optional helper
            flag.ReturnToBase();
    }
}
