using UnityEngine;

public class TeamManager : MonoBehaviour
{
    public static TeamManager Instance { get; private set; }

    public TeamData[] AvailableTeams;
    private int _nextTeamIndex = 0;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public int GetNextTeamIndex()
    {
        int teamIndex = _nextTeamIndex;
        _nextTeamIndex = (_nextTeamIndex + 1) % AvailableTeams.Length;
        return teamIndex;
    }
    public TeamData GetTeamData(int index)
    {
        if (index >= 0 && index < AvailableTeams.Length)
            return AvailableTeams[index];
        return null;
    }
}
