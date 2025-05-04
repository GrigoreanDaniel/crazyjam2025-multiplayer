using UnityEngine;

public class EnvironmentSpawner : MonoBehaviour {

    [Header("Spawn Area")]
    [SerializeField] private Vector3 areaSize = new Vector3(50, 0, 50);
    [SerializeField] private LayerMask groundLayer;

    [Header("Prefabs")]
    [SerializeField] private GameObject[] prefabsToSpawn;

    [Header("Spawn Settings")]
    [SerializeField] private int spawnCount = 100;
    [SerializeField] private float minScale = 0.8f;
    [SerializeField] private float maxScale = 1.2f;
    [SerializeField] private float raycastHeight = 50f;
    [SerializeField] private bool useCollisionCheck = true;
    [SerializeField] private float collisionRadius = 1f;

    [Header("Runtime")]
    [SerializeField] private bool spawnOnStart = true;

    private void Start() {
        if (spawnOnStart)
            SpawnAll();
    }

    public void SpawnAll() {
        int spawned = 0;
        int attempts = 0;
        int maxAttempts = spawnCount * 5;

        while (spawned < spawnCount && attempts < maxAttempts) {
            if (TrySpawnObject())
                spawned++;
            attempts++;
        }

        Debug.Log($"Spawned {spawned}/{spawnCount} objects.");
    }

    private bool TrySpawnObject() {
        if (prefabsToSpawn == null || prefabsToSpawn.Length == 0)
            return false;

        GameObject prefab = prefabsToSpawn[Random.Range(0, prefabsToSpawn.Length)];

        Vector3 basePos = transform.position + new Vector3(
            Random.Range(-areaSize.x / 2f, areaSize.x / 2f),
            raycastHeight,
            Random.Range(-areaSize.z / 2f, areaSize.z / 2f)
        );
        //Debug.Log("Trying spawn from: " + basePos);
        if (Physics.Raycast(basePos, Vector3.down, out RaycastHit hit, raycastHeight * 2f)) {
            Vector3 finalPos = hit.point;

            // Collision check (optional)
            if (useCollisionCheck && Physics.CheckSphere(finalPos, collisionRadius, ~groundLayer))
                return false;
            //Debug.DrawRay(basePos, Vector3.down * raycastHeight * 2f, Color.red, 2f);
            //Debug.Log("Raycast hit at: " + hit.point + " | Hit object: " + hit.collider.name);

            GameObject instance = Instantiate(prefab, finalPos, Quaternion.Euler(0, Random.Range(0f, 360f), 0), transform);

            float scale = Random.Range(minScale, maxScale);
            instance.transform.localScale = Vector3.one * scale;

            return true;
        }
        //Debug.LogWarning("Raycast missed. No ground hit from: " + basePos);
        return false;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position + new Vector3(0, 0.1f, 0), areaSize);
    }
}
