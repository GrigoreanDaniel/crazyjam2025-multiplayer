using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;


public class PlayerSpawner : MonoBehaviour, INetworkRunnerCallbacks
{

    [SerializeField] private NetworkPrefabRef playerPrefab;
    //[Networked, Capacity(6)] private NetworkDictionary<PlayerRef, Player> Players => default; // Dictionary to store players

    void INetworkRunnerCallbacks.OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer)
        {
            // Spawn the player prefab for the new player
            SpawnPlayer(runner, player);
            Debug.Log("we are server. Player joined: " + player);
        }
        else
        {
            Debug.Log("we are client. Player joined: " + player);
        }
    }

    void INetworkRunnerCallbacks.OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (!runner.IsServer)
        {
            return;
        }

        // Clean up the player object when they leave
        //CleanupPlayer(runner, player);
    }



    /// <summary>
    /// Method to spawn a player prefab
    /// </summary>
    /// <param name="runner"></param>
    /// <param name="player"></param>
    private void SpawnPlayer(NetworkRunner runner, PlayerRef player)
    {
        var spawnPosition = new Vector3(0, 0, 0); // Set spawn position as needed
        var spawnRotation = Quaternion.identity; // Set spawn rotation as needed

        var playerObject = runner.Spawn(playerPrefab, spawnPosition, spawnRotation, player); //Spawn the player
        //Players.Add(player, playerObject.GetComponent<Player>()); // Add the player to the dictionary
    }

    // Method to clean up a player when they leave
    /*private void CleanupPlayer(NetworkRunner runner, PlayerRef player)
    {
        if (Players.TryGet(player, out Player playerBehaviour))
        {
            Players.Remove(player);
            runner.Despawn(playerBehaviour.Object); // Despawn the player's object
        }
    }*/
    void INetworkRunnerCallbacks.OnInput(NetworkRunner runner, NetworkInput input)
    {

    }



    void INetworkRunnerCallbacks.OnConnectedToServer(NetworkRunner runner)
    {
        Debug.Log("Connected to server.");
    }


    void INetworkRunnerCallbacks.OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        Debug.Log("Connection failed: " + reason);
    }

    void INetworkRunnerCallbacks.OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        Debug.Log("Connect request received: " + request.RemoteAddress.ToString());
    }

    void INetworkRunnerCallbacks.OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        Debug.Log("Custom authentication response received.");
    }

    void INetworkRunnerCallbacks.OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
        Debug.Log("Disconnected from server: " + reason);
    }

    void INetworkRunnerCallbacks.OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {

    }


    void INetworkRunnerCallbacks.OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {

    }

    void INetworkRunnerCallbacks.OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {

    }

    void INetworkRunnerCallbacks.OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {

    }

    void INetworkRunnerCallbacks.OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {

    }

    void INetworkRunnerCallbacks.OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {

    }

    void INetworkRunnerCallbacks.OnSceneLoadDone(NetworkRunner runner)
    {

    }

    void INetworkRunnerCallbacks.OnSceneLoadStart(NetworkRunner runner)
    {

    }

    void INetworkRunnerCallbacks.OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {

    }

    void INetworkRunnerCallbacks.OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {

    }

    void INetworkRunnerCallbacks.OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {

    }


}
