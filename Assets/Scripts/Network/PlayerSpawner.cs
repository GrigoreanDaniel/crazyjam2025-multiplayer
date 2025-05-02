using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;


public class PlayerSpawner : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] private NetworkPlayer playerPrefab;
    private readonly Dictionary<int, NetworkPlayer> mapTokenIdWithNetworkPlayer = new Dictionary<int, NetworkPlayer>(); // Dictionary to store players
    private CharacterInputHandler characterInputHandler;

    private int GetPlayerToken(NetworkRunner runner, PlayerRef player)
    {
        if (runner.LocalPlayer == player)
        {
            return ConnectionTokenUtils.HashToken(GameManager.Instance.GetPlayerConnectionToken());
        }
        else
        {
            var token = runner.GetPlayerConnectionToken(player);

            if (token != null)
                return ConnectionTokenUtils.HashToken(token);

            Debug.LogError("Player token is null for player: " + player);
            return 0; // Return an invalid token if null
        }
    }

    public void SetConnectionTokenMapping(int token, NetworkPlayer player)
    {
        mapTokenIdWithNetworkPlayer.Add(token, player); // Add the player to the dictionary
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
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

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (!runner.IsServer)
        {
            return;
        }
        {
            //PlayersConnected.Remove(player);
        }
    }
    /// <summary>
    /// Method to spawn a player prefab
    /// </summary>
    /// <param name="runner"></param>
    /// <param name="player"></param>
    private void SpawnPlayer(NetworkRunner runner, PlayerRef player)
    {
        int playerToken = GetPlayerToken(runner, player); // Get the player token
        var spawnPosition = new Vector3(0, 0, 0); // Set spawn position as needed
        var spawnRotation = Quaternion.identity; // Set spawn rotation as needed

        Debug.Log("Spawning player with token: " + playerToken);

        if (mapTokenIdWithNetworkPlayer.TryGetValue(playerToken, out NetworkPlayer networkPlayer))
        {
            Debug.Log("Player already exists in the dictionary. Player ID: " + playerToken);

            networkPlayer.Object.AssignInputAuthority(player); // Set input authority for the player object
        }
        else
        {
            NetworkPlayer newNetworkPlayer = runner.Spawn(playerPrefab, spawnPosition, spawnRotation, player); //Spawn the player
            newNetworkPlayer.PlayerToken = playerToken; // Set the player token
            mapTokenIdWithNetworkPlayer.Add(playerToken, newNetworkPlayer); // Add the player to the dictionary

            Debug.Log("Spawning new player. Player ID: " + playerToken);
        }
    }
    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        if (characterInputHandler == null && NetworkPlayer.LocalPlayer != null)
        {
            // Get the CharacterInputHandler component from the player object
            characterInputHandler = NetworkPlayer.LocalPlayer.GetComponent<CharacterInputHandler>();
        }

        if (characterInputHandler != null)
        {
            // Get the input data from the CharacterInputHandler and set it to the NetworkInput
            input.Set(characterInputHandler.GetNetworkInputData());
        }
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

    public async void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        Debug.Log("OnHostMigration started.");
        await runner.Shutdown(shutdownReason: ShutdownReason.HostMigration);

        FindObjectOfType<NetworkRunnerHandler>().StartHostMigration(hostMigrationToken);
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
