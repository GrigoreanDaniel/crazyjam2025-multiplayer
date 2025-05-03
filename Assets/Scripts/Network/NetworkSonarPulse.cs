using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class NetworkSonarPulse : NetworkBehaviour
{
    [Header("Sonar Settings")]
    [Range(1f, 50f)]
    [SerializeField] private float sonarRadius = 10f;
    [SerializeField] private float revealDuration = 3f;
    [SerializeField] private float cooldownDuration = 10f;
    [SerializeField] private KeyCode activateKey = KeyCode.Q;

    [Header("Gizmo Settings")]
    [SerializeField] private Color pulseGizmoColor = new Color(0f, 1f, 1f, 0.25f);
    public bool isOnCooldown = false;
    //private bool showGizmo = false;

    public override void FixedUpdateNetwork()
    {
        if (!Object.HasInputAuthority) return;

        if (GetInput(out NetworkInputData networkInputData))
        {
            //activates with Fire2, right click
            if (networkInputData.sonarPressed && !isOnCooldown)
            {
                isOnCooldown = true;
                RPC_TriggerSonar(transform.position);
                StartCoroutine(SonarCooldown());
            }
        }            

    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.InputAuthority)]
    private void RPC_TriggerSonar(Vector3 origin)
    {
        Debug.Log("Sonnar triggered");
        Collider[] hits = Physics.OverlapSphere(origin, sonarRadius);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player") && hit.gameObject != this.gameObject)
            {
                Debug.Log($"[Sonar] Detected {hit.name}");
                StartCoroutine(RevealPlayer(hit.gameObject));
            }
        }
    }

    private IEnumerator RevealPlayer(GameObject target)
    {
        Renderer renderer = target.GetComponentInChildren<Renderer>();
        if (renderer != null && renderer.material.HasProperty("_EmissionColor"))
        {
            Color originalEmission = renderer.material.GetColor("_EmissionColor");
            Color glowColor = Color.cyan * 2f;

            renderer.material.EnableKeyword("_EMISSION");
            renderer.material.SetColor("_EmissionColor", glowColor);

            yield return new WaitForSeconds(revealDuration);

            renderer.material.SetColor("_EmissionColor", originalEmission);
        }
    }

    private IEnumerator SonarCooldown()
    {
        yield return new WaitForSeconds(cooldownDuration);
        isOnCooldown = false;
    }



#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
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
