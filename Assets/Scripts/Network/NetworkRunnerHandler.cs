using System;
using System.Threading.Tasks;
using System.Linq;
using UnityEngine.SceneManagement;
using Fusion;
using Fusion.Sockets;
using UnityEngine;

public class NetworkRunnerHandler : MonoBehaviour
{
    public NetworkRunner networkRunnerPrefab; // Prefab for the NetworkRunner
    private NetworkRunner networkRunner;

    void Awake()
    {
        networkRunner = FindObjectOfType<NetworkRunner>(); // Find the existing NetworkRunner in the scene
    }

    void Start()
    {
        if (networkRunner == null)
        {
            networkRunner = Instantiate(networkRunnerPrefab);
            networkRunner.name = "Network Runner"; // Set the name of the NetworkRunner
        }

        var clientTask = InitializeNetworkRunner(
        networkRunner,
            GameMode.AutoHostOrClient,
            GameManager.Instance.GetPlayerConnectionToken(), // Get the player connection token
            "testSession",
            NetAddress.Any(),
            SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex),
            null); // Initialize the NetworkRunner

        Debug.Log("NetworkRunner initialized successfully.");
    }

    public void StartHostMigration(HostMigrationToken hostMigrationToken)
    {
        networkRunner = FindObjectOfType<NetworkRunner>();

        networkRunner = Instantiate(networkRunnerPrefab);
        networkRunner.name = "Network Runner - Migrated"; // Set the name of the NetworkRunner
        Debug.Log("NetworkRunner" + networkRunner.name + " instantiated for host migration.");

        var clientTask = InitializeNetworkRunnerHostMigration(networkRunner, hostMigrationToken, GameManager.Instance.GetPlayerConnectionToken()); // Initialize the NetworkRunner for host migration

        Debug.Log("NetworkRunner migration started");
    }

    INetworkSceneManager GetSceneManager(NetworkRunner runner)
    {
        var sceneManager = runner.GetComponents(typeof(MonoBehaviour)).OfType<INetworkSceneManager>().FirstOrDefault();

        if (sceneManager == null)
        {
            sceneManager = runner.gameObject.AddComponent<NetworkSceneManagerDefault>();
        }

        return sceneManager;
    }

    protected virtual Task InitializeNetworkRunner(NetworkRunner runner, GameMode gameMode, byte[] connectionToken, string sessionName, NetAddress addres, SceneRef scene, Action<NetworkRunner> initialized)
    {

        var sceneManager = GetSceneManager(runner); // Get the scene manager

        runner.ProvideInput = true; // Enable input for the runner

        return runner.StartGame(new StartGameArgs
        {
            GameMode = gameMode, // Set the game mode
            SessionName = sessionName, // Set the session name
            Address = addres, // Set the network address
            Scene = scene, // Set the scene reference
            CustomLobbyName = "testLobbyName", // Set the custom lobby name
            SceneManager = sceneManager, // Set the scene manager
            ConnectionToken = connectionToken,
            //PlayerCount = 6, // Set the maximum number of players
        });
    }

    protected virtual Task InitializeNetworkRunnerHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken, byte[] connectionToken)
    {
        Debug.Log("InitializeNetworkRunnerHostMigration called.");

        var sceneManager = GetSceneManager(runner); // Get the scene manager

        runner.ProvideInput = true; // Enable input for the runner

        return runner.StartGame(new StartGameArgs
        {
            //GameMode = gameMode, // Set the game mode
            //SessionName = sessionName, // Set the session name
            //Address = addres, // Set the network address
            //Scene = scene, // Set the scene reference
            //CustomLobbyName = "testLobbyName", // Set the custom lobby name
            SceneManager = sceneManager, // Set the scene manager
            //PlayerCount = 6, // Set the maximum number of players
            HostMigrationToken = hostMigrationToken,
            ConnectionToken = connectionToken,
            HostMigrationResume = HostMigrationResume,
        });

    }

    private void HostMigrationResume(NetworkRunner runner)
    {
        Debug.Log("Host migration resumed started.");

        foreach (var resumeNetworkObject in runner.GetResumeSnapshotNetworkObjects())
        {
            if (resumeNetworkObject.TryGetBehaviour<NetworkTRSP>(out var trsp))
            {
                Debug.Log($"Resuming object {resumeNetworkObject} with authority: {trsp.Object.InputAuthority}, position: {trsp.transform.position}.");

                runner.Spawn(resumeNetworkObject, trsp.transform.position, trsp.transform.rotation, trsp.Object.InputAuthority, onBeforeSpawned: (runner, newNetworkObject) =>
                {
                    // Set the position and rotation of the resumed object

                    if (resumeNetworkObject.TryGetBehaviour<NetworkPlayer>(out var oldNetworkPlayer))
                    {
                        FindObjectOfType<PlayerSpawner>().SetConnectionTokenMapping(oldNetworkPlayer.PlayerToken, newNetworkObject.GetComponent<NetworkPlayer>()); // Map the old player token to the new player object
                    }

                });
            }
        }

        Debug.Log("Host migration completed.");

    }

}
