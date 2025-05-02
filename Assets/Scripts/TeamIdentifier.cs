using UnityEngine;

public class TeamIdentifier : MonoBehaviour {
    [SerializeField] private string teamTag = "TeamA";
    public string TeamTag => teamTag;

    public void OverrideTeam(string newTag) {
        teamTag = newTag;
    }

}
