using UnityEngine;

public class JailZone : MonoBehaviour
{
    [SerializeField] private Transform jailSpawnPoint;

    public Vector3 GetJailPosition()
    {
        if (jailSpawnPoint != null)
            return jailSpawnPoint.position;
        else
            return transform.position; // fallback if not assigned
    }

    private void OnDrawGizmos()
    {
        if (jailSpawnPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(jailSpawnPoint.position, 0.3f);
        }
    }
}
