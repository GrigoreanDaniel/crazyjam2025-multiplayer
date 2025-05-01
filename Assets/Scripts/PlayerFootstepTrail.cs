using UnityEngine;

public class PlayerFootstepTrail : MonoBehaviour {
    [Header("Footstep Settings")]
    [SerializeField] private GameObject footstepPrefab;
    [SerializeField] private float stepInterval = 0.4f;
    [SerializeField] private LayerMask groundLayer;

    private float stepTimer;
    private CharacterController controller;

    private Vector3 lastRayOrigin;
    private Vector3 lastRayDirection;
    private bool lastRayHit;
    private Vector3 lastHitPoint;

    private void Start() {
        controller = GetComponent<CharacterController>();
        stepTimer = stepInterval;
    }

    private void Update() {
        if (controller == null || footstepPrefab == null) return;

        bool isMoving = controller.velocity.magnitude > 0.1f && controller.isGrounded;

        if (isMoving) {
            stepTimer -= Time.deltaTime;

            if (stepTimer <= 0f) {
                SpawnFootstep();
                stepTimer = stepInterval;
            }
        } else {
            stepTimer = stepInterval; // Reset if idle
        }
    }

    private void SpawnFootstep() {
        Vector3 baseOrigin = transform.position + Vector3.up * 0.5f;
        Vector3 forwardOrigin = transform.position + transform.forward * 0.3f + Vector3.up * 1f;
        float rayDistance = 5f;

        bool hitFound = false;
        RaycastHit hit;

        // First: try direct downward ray
        if (Physics.Raycast(baseOrigin, Vector3.down, out hit, rayDistance, groundLayer)) {
            hitFound = true;
        } else if (Physics.Raycast(forwardOrigin, Vector3.down, out hit, rayDistance, groundLayer)) {
            hitFound = true;
        }

        if (hitFound) {
            Vector3 spawnPos = hit.point + Vector3.up * 0.01f;
            Instantiate(footstepPrefab, spawnPos, Quaternion.identity);
        }
    }
    private void OnDrawGizmos() {
        if (!Application.isPlaying) return;

        Gizmos.color = lastRayHit ? Color.green : Color.red;
        Gizmos.DrawRay(lastRayOrigin, lastRayDirection * 2.5f);

        if (lastRayHit) {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(lastHitPoint, 0.1f);
        }
    }

}
