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

    [Header("Team Materials")]
    [SerializeField] private Material teamAMaterial;
    [SerializeField] private Material teamBMaterial;

    [SerializeField] private GameObject flagPrefab;
    [SerializeField] private Material teamAFlagMaterial;
    [SerializeField] private Material teamBFlagMaterial;

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

        // Set Team Tag on BaseZone script inside CaptureZone
        BaseZone baseZone = spawnedBase.GetComponentInChildren<BaseZone>();
        if (baseZone != null) {
            baseZone.SetTeamTag(teamTag);
        }

        // Assign color
        // Find all target parts inside spawned base
        Material teamMat = (teamTag == "TeamA") ? teamAMaterial : teamBMaterial;

        // Helper function to color all children of a part
        void ApplyMaterialToChildren(Transform part) {
            if (part == null) return;

            foreach (MeshRenderer r in part.GetComponentsInChildren<MeshRenderer>())
                r.material = teamMat;
        }

        // Fetch and color target parts
        Transform baseRoot = spawnedBase.transform;

        ApplyMaterialToChildren(baseRoot.Find("BaseComponents/Foundation"));
        ApplyMaterialToChildren(baseRoot.Find("BaseComponents/StairsAll"));
        ApplyMaterialToChildren(baseRoot.Find("BaseComponents/Columns"));
        ApplyMaterialToChildren(baseRoot.Find("BaseComponents/Roof"));

        // Spawn Flag
        Transform flagMount = spawnedBase.transform.Find("CaptureZone"); // Or another defined mount
        if (flagMount != null) {
            GameObject spawnedFlag = Instantiate(flagPrefab, flagMount.position, flagMount.rotation);
            spawnedFlag.transform.SetParent(flagMount); // Optional for syncing visuals

            Material teamFlagMat = (teamTag == "TeamA") ? teamAFlagMaterial : teamBFlagMaterial;

            TeamFlag flag = spawnedFlag.GetComponent<TeamFlag>();
            if (flag != null) {
                flag.InitializeFlag(teamTag, teamFlagMat);
            }
        }

        baseRef = spawnedBase;
        Debug.Log($"Spawned {teamTag} base at: {selectedSpawn.position}");
    }

    public GameObject GetBaseForTeam(string teamTag) {
        return teamTag == teamATag ? teamABase : teamBBase;
    }
}
