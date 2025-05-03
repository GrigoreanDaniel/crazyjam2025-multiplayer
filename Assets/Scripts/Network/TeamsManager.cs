using UnityEngine;

public class TeamManager : MonoBehaviour
{
    public static TeamManager Instance { get; private set; }

    public TeamData redTeam;
    public TeamData blueTeam;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public TeamData GetTeamForPlayer(int playerIndex)
    {
        // Example: alternate team assignment
        return playerIndex % 2 == 0 ? redTeam : blueTeam;
    }

    public TeamData GetTeamForIndex(int index)
    {
        return index == 0 ? redTeam : blueTeam;
    }
}
