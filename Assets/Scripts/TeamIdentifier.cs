using UnityEngine;

public class TeamIdentifier : MonoBehaviour {
    [SerializeField] private string teamTag;
    public string TeamTag => teamTag;

    public void SetTeamTag(string tag) {
        teamTag = tag;
    }


    public void OverrideTeam(string newTag) {
        teamTag = newTag;
    }

}
