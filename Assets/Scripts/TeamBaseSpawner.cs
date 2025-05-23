using UnityEngine;
using System.Collections.Generic;

public class TeamBaseSpawner : MonoBehaviour {
    [System.Serializable]
    public class TeamSpawnGroup {
        public TeamData team;
        public GameObject basePrefab;
        public GameObject flagPrefab;
        public List<Transform> spawnPoints;
    }


    [SerializeField] private List<TeamSpawnGroup> spawnGroups;

    private void Start() {
        foreach (var group in spawnGroups) {
            if (group.spawnPoints.Count == 0 || group.basePrefab == null) continue;

            Transform chosenPoint = group.spawnPoints[Random.Range(0, group.spawnPoints.Count)];
            GameObject baseInstance = Instantiate(group.basePrefab, chosenPoint.position, chosenPoint.rotation);

            // Find a Transform inside base to use as flag anchor
            Transform flagAnchor = baseInstance.transform.Find("FlagAnchor"); // or use a public field if not named
            
            if (group.flagPrefab != null && flagAnchor != null) {
                GameObject flagInstance = Instantiate(group.flagPrefab, flagAnchor.position, flagAnchor.rotation);

                Flag flag = flagInstance.GetComponent<Flag>();
                TeamIdentifier flagTeamId = flagInstance.GetComponent<TeamIdentifier>();

                if (flag != null) {
                    flag.basePosition = flagAnchor;
                    flag.owningTeam = group.team;
                }

                if (flagTeamId != null)
                    flagTeamId.Team = group.team;

            }
            // Optional: color the base to match team
            Renderer[] renderers = baseInstance.GetComponentsInChildren<Renderer>();
            foreach (var rend in renderers) {
                if (rend.material.HasProperty("_Color"))
                    rend.material.color = group.team.teamColor;
            }

            // Optional: assign TeamIdentifier if not already assigned in prefab
            var teamId = baseInstance.GetComponent<TeamIdentifier>();
            if (teamId != null)
                teamId.Team = group.team;
        }
    }
}
