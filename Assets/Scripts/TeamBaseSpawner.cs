using UnityEngine;
using System.Collections.Generic;

public class TeamBaseSpawner : MonoBehaviour {
    [System.Serializable]
    public class TeamSpawnGroup {
        public TeamData Team;
        public GameObject basePrefab;
        public GameObject flagPrefab;
        public List<Transform> spawnPoints;
    }

    [SerializeField] private FlagHoldTracker flagHoldTracker;
    [SerializeField] private List<TeamSpawnGroup> spawnGroups;

    private void Start() {
        if (flagHoldTracker == null) {
            Debug.LogError("[TeamBaseSpawner] FlagHoldTracker reference is missing!");
            return;
        }

        var activeTeams = new HashSet<TeamData> {
        flagHoldTracker.LeftTeam,
        flagHoldTracker.RightTeam
    };

        foreach (var group in spawnGroups) {
            if (!activeTeams.Contains(group.Team)) continue;

            if (group.spawnPoints.Count == 0 || group.basePrefab == null) continue;

            Transform chosenPoint = group.spawnPoints[Random.Range(0, group.spawnPoints.Count)];
            GameObject baseInstance = Instantiate(group.basePrefab, chosenPoint.position, chosenPoint.rotation);

            var spawnPoints = baseInstance.GetComponentsInChildren<TeamSpawnPoint>();
            foreach (var point in spawnPoints) {
                point.SetTeam(group.Team);
            }

            Transform flagAnchor = baseInstance.transform.Find("FlagAnchor");
            if (group.flagPrefab != null && flagAnchor != null) {
                GameObject flagInstance = Instantiate(group.flagPrefab, flagAnchor.position, flagAnchor.rotation);

                Flag flag = flagInstance.GetComponent<Flag>();
                TeamIdentifier flagTeamId = flagInstance.GetComponent<TeamIdentifier>();

                if (flag != null) {
                    flag.basePosition = flagAnchor;
                    flag.owningTeam = group.Team;
                }

                if (flagTeamId != null)
                    flagTeamId.Team = group.Team;
            }

            Renderer[] renderers = baseInstance.GetComponentsInChildren<Renderer>();
            foreach (var rend in renderers) {
                if (rend.material.HasProperty("_Color"))
                    rend.material.color = group.Team.teamColor;
            }

            var teamId = baseInstance.GetComponent<TeamIdentifier>();
            if (teamId != null)
                teamId.Team = group.Team;
        }
    }

}