using UnityEngine;

public class DecoyMover : MonoBehaviour {
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float maxLifetime = 2f;
    [SerializeField] private LayerMask collisionMask;

    [Header("Trail Spawning")]
    [SerializeField] private GameObject footstepPrefab;
    [SerializeField] private float trailInterval = 0.3f;

    private float lifeTimer = 0f;
    private float trailTimer = 0f;
    private Vector3 direction;

    private void Start() {
        direction = transform.forward;
    }

    private void Update() {
        lifeTimer += Time.deltaTime;
        trailTimer += Time.deltaTime;

        if (CheckCollision()) return;

        MoveForward();
        TrySpawnTrail();
        CheckSelfDestruct();
    }

    private bool CheckCollision() {
        if (Physics.Raycast(transform.position, direction, moveSpeed * Time.deltaTime, collisionMask)) {
            Debug.Log("[Decoy] Hit object. Terminating early.");
            Destroy(gameObject);
            return true;
        }
        return false;
    }

    private void MoveForward() {
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    private void TrySpawnTrail() {
        if (trailTimer >= trailInterval && footstepPrefab != null) {
            GameObject trail = Instantiate(footstepPrefab, transform.position, Quaternion.LookRotation(direction));
            Destroy(trail, 2f); // Cleanup trail objects
            trailTimer = 0f;
        }
    }

    private void CheckSelfDestruct() {
        if (lifeTimer >= maxLifetime) {
            Destroy(gameObject);
        }
    }
    public void Setup(float moveSpeed, float lifetime, LayerMask mask, GameObject trailPrefab, float interval) {
        this.moveSpeed = moveSpeed;
        this.maxLifetime = lifetime;
        this.collisionMask = mask;
        this.footstepPrefab = trailPrefab;
        this.trailInterval = interval;

        direction = transform.forward;
    }

}
