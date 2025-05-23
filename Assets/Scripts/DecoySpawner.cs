using System.Collections;
using UnityEngine;

public class DecoySpawner : MonoBehaviour {
    [Header("Decoy Settings")]
    [SerializeField] private GameObject decoyWalkerPrefab;

    [Header("Activation")]
    [SerializeField] private KeyCode activateKey = KeyCode.E;
    [SerializeField] private float cooldownDuration = 6f;

    [Header("Spawn Position Offset")]
    [SerializeField] private Vector3 spawnOffset = new Vector3(0, 0, 1.5f); // Slightly in front

    [Header("Debug")]
    [SerializeField] private bool showGizmos = true;
    [SerializeField] private Color gizmoColor = Color.yellow;

    private bool isOnCooldown = false;

    private void Update() {
        if (Input.GetKeyDown(activateKey) && !isOnCooldown) {
            StartCoroutine(TriggerDecoy());
        }
    }

    private IEnumerator TriggerDecoy() {
        isOnCooldown = true;

        // Spawn the decoy in front of player
        Vector3 spawnPos = transform.position + transform.TransformDirection(spawnOffset);
        Quaternion spawnRot = transform.rotation;

        Instantiate(decoyWalkerPrefab, spawnPos, spawnRot);

        yield return new WaitForSeconds(cooldownDuration);
        isOnCooldown = false;
    }

    private void OnDrawGizmosSelected() {
        if (!showGizmos) return;

        Gizmos.color = gizmoColor;
        Vector3 spawnPos = transform.position + transform.TransformDirection(spawnOffset);
        Gizmos.DrawWireSphere(spawnPos, 0.3f);
    }
}
