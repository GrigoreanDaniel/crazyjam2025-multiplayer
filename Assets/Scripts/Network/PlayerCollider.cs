using Fusion;
using UnityEngine;

public class NetworkPlayerCollider : NetworkBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var otherPlayer = other.GetComponent<NetworkPlayer>();
        var myTeam = GetComponent<TeamIdentifier>()?.Team;
        var otherTeam = otherPlayer?.GetComponent<TeamIdentifier>()?.Team;

        if (otherPlayer != null && myTeam != null && otherTeam != null && myTeam != otherTeam)
        {
            Debug.Log("There was contact!");
            if (HasStateAuthority)
                otherPlayer.JailPlayer();
        }
    }
}
