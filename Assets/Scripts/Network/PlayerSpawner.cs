using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;


public class PlayerSpawner : MonoBehaviour, INetworkRunnerCallbacks
{
    [Header("Player Prefab")]
    [Tooltip("Player prefab to spawn for each player.")]
    [SerializeField] private NetworkPlayer playerPrefab;

    private SessionListUIHandler sessionListUIHandler; // Reference to the SessionListUIHandler script

    private Dictionary<int, NetworkPlayer> mapTokenIdWithNetworkPlayer; // Dictionary to store players
    private CharacterInputHandler characterInputHandler;

    void Awake()
    {
        mapTokenIdWithNetworkPlayer = new Dictionary<int, NetworkPlayer>();

        sessionListUIHandler = FindObjectOfType<SessionListUIHandler>(true); // Find the SessionListUIHandler in the scene
    }

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
        if (player == runner.LocalPlayer)
        {
            // Spawn the player prefab for the new player
            SpawnPlayer(runner, player);
            Debug.Log("we are server. Player joined: " + player);
        }

    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (player == runner.LocalPlayer)
        {
            int playerToken = GetPlayerToken(runner, player); // Get the player token
            if (mapTokenIdWithNetworkPlayer.TryGetValue(playerToken, out NetworkPlayer networkPlayer))
            {
                Debug.Log("Removing Player from the dictionary. Player ID: " + playerToken);

                mapTokenIdWithNetworkPlayer.Add(playerToken, networkPlayer);
            }
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
        var spawnPosition = new Vector3(0, 50, 0); // Set spawn position as needed
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

    void INetworkRunnerCallbacks.OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        /*Debug.Log("OnHostMigration started.");
        await runner.Shutdown(shutdownReason: ShutdownReason.HostMigration);

        FindObjectOfType<NetworkRunnerHandler>().StartHostMigration(hostMigrationToken);*/
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

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        Debug.Log("Session list updated. Number of sessions: " + sessionList.Count);

        if (sessionListUIHandler == null)
        {
            Debug.LogError("SessionListUIHandler not found in the scene.");
            return;
        }

        if (sessionList.Count == 0)
        {
            Debug.Log("No active game found.");
            sessionListUIHandler.SetStatusText(" No active game was found"); // Call the method to update the session list UI
        }
        else
        {
            sessionListUIHandler.ClearList(); // Clear the existing session list UI

            foreach (SessionInfo sessionInfo in sessionList)
            {
                sessionListUIHandler.AddToList(sessionInfo); // Add each session to the list UI
                Debug.Log("Session found: " + sessionInfo.Name + " - Players Count: " + sessionInfo.PlayerCount); // Log the session information
            }
        }
    }

    void INetworkRunnerCallbacks.OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {

    }

    void INetworkRunnerCallbacks.OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {

    }
}
