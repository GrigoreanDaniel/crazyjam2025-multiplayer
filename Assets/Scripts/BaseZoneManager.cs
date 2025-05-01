using UnityEngine;
using System.Collections.Generic;

public class BaseZoneManager : MonoBehaviour {
    [Header("Base Prefab")]
    [SerializeField] private GameObject basePrefab;

    [Header("Spawn Points")]
    [SerializeField] private List<Transform> teamABaseSpawns;
    [SerializeField] private List<Transform> teamBBaseSpawns;

    [Header("Team Tags (Optional for Later)")]
    [SerializeField] private string teamATag = "TeamA";
    [SerializeField] private string teamBTag = "TeamB";

    private GameObject teamABase;
    private GameObject teamBBase;

    private void Start() {
        SpawnBaseForTeam(teamABaseSpawns, teamATag, out teamABase);
        SpawnBaseForTeam(teamBBaseSpawns, teamBTag, out teamBBase);
    }

    private void SpawnBaseForTeam(List<Transform> spawnOptions, string teamTag, out GameObject baseRef) {
        if (spawnOptions.Count == 0) {
            Debug.LogWarning("No spawn points defined for " + teamTag);
            baseRef = null;
            return;
        }

        Transform selectedSpawn = spawnOptions[Random.Range(0, spawnOptions.Count)];
        GameObject spawnedBase = Instantiate(basePrefab, selectedSpawn.position, selectedSpawn.rotation);
        spawnedBase.name = $"Base_{teamTag}";
        spawnedBase.tag = teamTag;
        baseRef = spawnedBase;

        Debug.Log($"Spawned {teamTag} base at: {selectedSpawn.position}");
    }

    public GameObject GetBaseForTeam(string teamTag) {
        return teamTag == teamATag ? teamABase : teamBBase;
    }
}
