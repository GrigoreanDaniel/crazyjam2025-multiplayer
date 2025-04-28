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


    void Start()
    {
        networkRunner = Instantiate(networkRunnerPrefab); // Instantiate the NetworkRunner prefab

        var clientTask = InitializeNetworkRunner(
            networkRunner,
            GameMode.AutoHostOrClient,
            NetAddress.Any(),
            SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex),
            null); // Initialize the NetworkRunner

        Debug.Log("NetworkRunner initialized successfully.");
    }

    protected virtual Task InitializeNetworkRunner(NetworkRunner runner, GameMode gameMode, NetAddress addres, SceneRef scene, Action<NetworkRunner> initialized)
    {

        var sceneManager = runner.GetComponents(typeof(MonoBehaviour)).OfType<INetworkSceneManager>().FirstOrDefault();

        if (sceneManager == null)
        {
            sceneManager = runner.gameObject.AddComponent<NetworkSceneManagerDefault>();
        }

        runner.ProvideInput = true; // Enable input for the runner

        var startGameArgs = new StartGameArgs
        {
            GameMode = gameMode, // Set the game mode
            SessionName = "Test", // Set the session name
            Address = addres, // Set the network address
            Scene = scene, // Set the scene reference
            SceneManager = sceneManager, // Set the scene manager
            //PlayerCount = 6, // Set the maximum number of players
        };

        return runner.StartGame(startGameArgs).ContinueWith(task =>
        {
            if (task.Exception == null)
            {
                initialized?.Invoke(runner);
            }
            else
            {
                Debug.LogError($"Failed to start game: {task.Exception}");
            }
        });
    }


}
