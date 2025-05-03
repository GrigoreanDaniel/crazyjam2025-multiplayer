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
            networkRunner.name = "NetworkRunner"; // Set the name of the NetworkRunner
        }

        var clientTask = InitializeNetworkRunner(
            networkRunner,
            GameMode.AutoHostOrClient,
            "testSession",
            NetAddress.Any(),
            SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex),
            null); // Initialize the NetworkRunner

        Debug.Log("NetworkRunner initialized successfully.");
    }

    INetworkSceneManager GetSceneManager(NetworkRunner runner)
    {
        INetworkSceneManager sceneManager = runner.GetComponents(typeof(MonoBehaviour)).OfType<INetworkSceneManager>().FirstOrDefault();

        if (sceneManager == null)
        {
            sceneManager = runner.gameObject.AddComponent<NetworkSceneManagerDefault>();
        }

        return sceneManager;

    }

    protected virtual Task InitializeNetworkRunner(NetworkRunner runner, GameMode gameMode, string sessionName, NetAddress addres, SceneRef scene, Action<NetworkRunner> initialized)
    {

        INetworkSceneManager sceneManager = GetSceneManager(runner); // Get the scene manager

        runner.ProvideInput = true; // Enable input for the runner

        return runner.StartGame(new StartGameArgs
        {
            GameMode = gameMode, // Set the game mode
            SessionName = sessionName, // Set the session name
            Address = addres, // Set the network address
            Scene = scene, // Set the scene reference
            CustomLobbyName = "testLobbyName", // Set the custom lobby name
            SceneManager = sceneManager, // Set the scene manager
            //PlayerCount = 6, // Set the maximum number of players
        });
    }
}
