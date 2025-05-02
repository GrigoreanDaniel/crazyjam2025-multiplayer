using UnityEngine;
using Fusion;

public class NetworkDecoyWalker :NetworkBehaviour {
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float maxLifetime = 2f;
    [SerializeField] private LayerMask terrainMask;

    [Header("Footstep Trail")]
    [SerializeField] private GameObject footstepPrefab;
    [SerializeField] private float footstepInterval = 0.3f;

    private float lifetimeTimer = 0f;
    private float footstepTimer = 0f;

    private void Update() {
        // --- Move forward
        transform.position += transform.forward * moveSpeed * Time.deltaTime;

        // --- Follow terrain height (raycast down)
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out RaycastHit hit, 5f, terrainMask)) {
            Vector3 pos = transform.position;
            pos.y = hit.point.y;
            transform.position = pos;
        }

        // --- Emit footstep at interval
        footstepTimer += Time.deltaTime;
        if (footstepTimer >= footstepInterval) {
            SpawnFootstep();
            footstepTimer = 0f;
        }

        // --- Destroy after max lifetime
        lifetimeTimer += Time.deltaTime;
        if (lifetimeTimer >= maxLifetime) {
            Destroy(gameObject);
        }
    }

    private void SpawnFootstep() {
        if (footstepPrefab != null) {
            Instantiate(footstepPrefab, transform.position, Quaternion.identity);
        }
    }

    // Optional Gizmo for debugging
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position + Vector3.up, Vector3.down * 5f);
    }
}
