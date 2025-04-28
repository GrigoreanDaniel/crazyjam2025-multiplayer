using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class NetworkPlayer : NetworkBehaviour, IPlayerLeft
{
    public static NetworkPlayer LocalPlayer { get; private set; }

    public override void Spawned()
    {
        if (!Object.HasInputAuthority)
        {
            Debug.Log("spawned remote Player.");
            return;

        }
        else
        {
            LocalPlayer = this; // Set the local player reference
            Debug.Log("spawned local Player.");
        }
    }

    public void PlayerLeft(PlayerRef player)
    {
        if (player == Object.InputAuthority)
        {
            Runner.Despawn(Object); // Despawn the local player object
        }
    }
}
