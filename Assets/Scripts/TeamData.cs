using UnityEngine;

[CreateAssetMenu(menuName = "CTF/Team Data")]
public class TeamData : ScriptableObject {
    public string teamName;
    public Color teamColor;
    public Sprite flagIcon;
    public string displayName; // e.g., "Screech"
    public Sprite icon;        // e.g., bat icon
}
