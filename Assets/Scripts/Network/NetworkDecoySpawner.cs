using System.Collections;
using UnityEngine;
using Fusion;

public class NetworkDecoySpawner : NetworkBehaviour {
    [Header("Decoy Settings")]
    [SerializeField] private NetworkPrefabRef decoyWalkerPrefab;

    [Header("Spawn Position Offset")]
    [SerializeField] private Vector3 spawnOffset = new Vector3(0, 0, 1.5f); // Slightly in front

    [Header("Debug")]
    [SerializeField] private bool showGizmos = true;
    [SerializeField] private Color gizmoColor = Color.yellow;

    [Header("Cooldown")]
    [SerializeField] private float cooldownDuration = 6f;
    private float cooldownEndTime = 0f;

    public override void FixedUpdateNetwork()
    {
        if (!HasStateAuthority) return; 

        if (GetInput(out NetworkInputData input))
        {
            if (input.decoyPressed && Runner.SimulationTime >= cooldownEndTime)
            {
                SpawnDecoy();
                cooldownEndTime = Runner.SimulationTime + cooldownDuration;
            }
        }
    }

    private void SpawnDecoy()
    {
        Vector3 spawnPos = transform.position + transform.TransformDirection(spawnOffset);
        Quaternion spawnRot = transform.rotation;

        Runner.Spawn(decoyWalkerPrefab, spawnPos, spawnRot);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected() {
        if (!showGizmos) return;

        Gizmos.color = gizmoColor;
        Vector3 spawnPos = transform.position + transform.TransformDirection(spawnOffset);
        Gizmos.DrawWireSphere(spawnPos, 0.3f);
    }
#endif
}
