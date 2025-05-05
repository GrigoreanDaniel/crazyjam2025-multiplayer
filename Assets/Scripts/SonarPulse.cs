using System.Collections;
using UnityEngine;

public class SonarPulse : MonoBehaviour, IAbility {
    [Header("Sonar Settings")]
    [Range(1f, 50f)]
    [SerializeField] private float sonarRadius = 10f;
    [SerializeField] private float revealDuration = 3f;
    [SerializeField] private float cooldownDuration = 10f;
    [SerializeField] private KeyCode activateKey = KeyCode.Q;

    [Header("Gizmo Settings")]
    [SerializeField] private Color pulseGizmoColor = new Color(0f, 1f, 1f, 0.25f);

    private bool isOnCooldown = false;
    private float cooldownTimer = 0f;

    private void Update() {
        if (Input.GetKeyDown(activateKey) && !isOnCooldown) {
            StartCoroutine(ActivateSonarPulse());
        }
    }
    public float GetCooldownNormalized()
    {
        return isOnCooldown ? cooldownTimer / cooldownDuration : 0f;
    }

    private IEnumerator ActivateSonarPulse() {
        isOnCooldown = true;
        cooldownTimer = cooldownDuration;

        // Detect players
        Collider[] hits = Physics.OverlapSphere(transform.position, sonarRadius);
        foreach (Collider hit in hits) {
            if (hit.CompareTag("Player") && hit.gameObject != this.gameObject) {
                Debug.Log($"[Sonar] Revealed: {hit.name}");
                StartCoroutine(TemporarilyReveal(hit.gameObject));
            }
        }

        // Show radius briefly
        yield return new WaitForSeconds(0.5f);
        //showGizmo = false;

        // Start cooldown
        yield return new WaitForSeconds(cooldownDuration - 0.5f);

        while (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
            yield return null;
        }

        isOnCooldown = false;
    }

    private IEnumerator TemporarilyReveal(GameObject target) {
        Renderer renderer = target.GetComponentInChildren<Renderer>();
        if (renderer != null && renderer.material.HasProperty("_EmissionColor")) {
            Color originalEmission = renderer.material.GetColor("_EmissionColor");
            Color glowColor = Color.cyan * 2f; // Use HDR intensity for a real glow in post-processing

            // Enable emission keyword if not active
            renderer.material.EnableKeyword("_EMISSION");
            renderer.material.SetColor("_EmissionColor", glowColor);

            yield return new WaitForSeconds(revealDuration);

            renderer.material.SetColor("_EmissionColor", originalEmission);
        } else {
            Debug.LogWarning($"[Sonar] {target.name} has no emission-ready material.");
        }

        Debug.Log($"[Sonar] {target.name} is no longer revealed.");
    }


    #if UNITY_EDITOR
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, sonarRadius);

    #if UNITY_EDITOR
        UnityEditor.Handles.Label(
            transform.position + Vector3.up * 2f,
            $"Sonar Radius: {sonarRadius:F1} units"
        );
    #endif
    }
    #endif

}
