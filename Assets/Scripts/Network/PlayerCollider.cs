using Fusion;
using UnityEngine;

public class NetworkPlayerCollider : NetworkBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!HasStateAuthority) return;
        /*
        var otherPlayerCollider = other.GetComponent<NetworkPlayerCollider>();
        if (otherPlayerCollider == null) return;

        var myTeam = GetComponentInParent<TeamIdentifier>()?.Team;
        var otherTeam = otherPlayerCollider.GetComponentInParent<TeamIdentifier>()?.Team;

        if (myTeam == null || otherTeam == null || myTeam == otherTeam) return;

        Debug.Log($"[Trigger] {name} (team: {myTeam.name}) touched {otherPlayerCollider.name} (team: {otherTeam.name})");

        // Jail the other player (enemy)
        var otherNetworkPlayer = otherPlayerCollider.GetComponentInParent<NetworkPlayer>();
        if (otherNetworkPlayer != null)
        {
            Debug.Log($"Sending {otherNetworkPlayer.name} to jail");
            otherNetworkPlayer.JailPlayer();
        }
        */
    }
}
