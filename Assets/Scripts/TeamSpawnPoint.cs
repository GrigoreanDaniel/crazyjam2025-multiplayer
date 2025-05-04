using UnityEngine;

public class TeamSpawnPoint : MonoBehaviour {
    public TeamData Team;

    public void SetTeam(TeamData assignedTeam) {
        Team = assignedTeam;
    }
}
