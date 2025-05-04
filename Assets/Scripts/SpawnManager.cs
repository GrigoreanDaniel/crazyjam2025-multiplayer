using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {
    private TeamSpawnPoint[] allSpawnPoints => FindObjectsOfType<TeamSpawnPoint>();

    public Transform GetRandomSpawnPointForTeam(TeamData team) {
        List<Transform> validPoints = new();

        foreach (var spawnPoint in allSpawnPoints) {
            if (spawnPoint.Team == team)
                validPoints.Add(spawnPoint.transform);
        }

        if (validPoints.Count == 0) {
            Debug.LogWarning($"No spawn points found for team: {team.teamName}");
            return null;
        }

        if (allSpawnPoints == null || allSpawnPoints.Length == 0) {
            Debug.LogWarning("No TeamSpawnPoints found in scene.");
            return null;
        }

        return validPoints[Random.Range(0, validPoints.Count)];
    }
}
