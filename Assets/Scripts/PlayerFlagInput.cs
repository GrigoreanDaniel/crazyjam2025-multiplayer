using UnityEngine;

public class PlayerFlagInput : MonoBehaviour {
    
    public TeamData Team => GetComponent<TeamIdentifier>().Team;
}