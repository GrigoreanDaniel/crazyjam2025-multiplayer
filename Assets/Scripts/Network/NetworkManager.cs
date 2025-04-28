using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;

/*public class NetworkManager : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] private NetworkRunner _runnerPrefab;
    [SerializeField] private PlayerSpawner _playerSpawner;

    private NetworkRunner _runner;


    // Start a game session
    public async void StartGame(GameMode mode)
    {
        // Create the NetworkRunner if it doesn't exist
        if (_runner == null)
        {
            _runner = Instantiate(_runnerPrefab);
            _runner.name = "NetworkRunner";

            // Add the callbacks
            _runner.AddCallbacks(this);
        }

        // Start or join a session
        var startGameArgs = new StartGameArgs()
        {
            GameMode = mode,
            SessionName = "3v3Game", //  players will it
            SceneManager = _runner.GetComponent<NetworkSceneManagerDefault>(),
            PlayerCount = 6
        };

        await _runner.StartGame(startGameArgs);
    }

    // INetworkRunnerCallbacks implementation
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        // Spawn a player when someone joins
           if (_playerSpawner != null)
             _playerSpawner.SpawnPlayer(runner, player);
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        // Clean up when a

    }



    // Implement the remaining INetworkRunnerCallbacks methods...
    public void OnInput(NetworkRunner runner, NetworkInput input) { }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnDisconnectedFromServer(NetworkRunner runner) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }

    }

}*/
