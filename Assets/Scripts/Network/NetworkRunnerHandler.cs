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
        NetworkRunner networkRunnerInScene = FindObjectOfType<NetworkRunner>(); // Check if a NetworkRunner already exists in the scene

        if (networkRunnerInScene != null)
            networkRunner = networkRunnerInScene; // Find the existing NetworkRunner in the scene
    }

    void Start()
    {
        if (networkRunner == null)
        {
            networkRunner = Instantiate(networkRunnerPrefab);
            networkRunner.name = "Network Runner"; // Set the name of the NetworkRunner

            if (SceneManager.GetActiveScene().name != LoadScenes.SceneName.Lobby.ToString())
            {
                var clientTask = InitializeNetworkRunner(
                networkRunner,
                GameMode.Shared,
                GameManager.Instance.GetPlayerConnectionToken(), // Get the player connection token
                "testSession",
                NetAddress.Any(),
                SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex),
                null); // Initialize the NetworkRunner
            }

            Debug.Log("NetworkRunner initialized successfully.");
        }
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
            CustomLobbyName = "OurLobbyID", // Set the custom lobby name
            SceneManager = sceneManager, // Set the scene manager
            ConnectionToken = connectionToken,
            PlayerCount = 6, // Set the maximum number of players
        });
    }

    public void OnJoinLobby()
    {
        var clientTask = JoinLobby(); // Call the method to join the lobby
    }

    private async Task JoinLobby()
    {
        Debug.Log("Joining lobby started...");

        string lobbyID = "OurLobbyID"; // Replace with your lobby ID
        var result = await networkRunner.JoinSessionLobby(SessionLobby.Custom, lobbyID); // Join the lobby

        if (!result.Ok)
        {
            Debug.Log($"Failed to join lobby {lobbyID}");
            // Handle lobby join failure here
        }
        else
        {
            Debug.Log($"Joined lobby {lobbyID} successfully.");
            // Handle successful lobby join here
        }
    }

    public void CreateGame(string sessionName, string sceneName)
    {
        string scenePath = $"Assets/Scenes/{sceneName}.unity";
        int buildIndex = SceneUtility.GetBuildIndexByScenePath(scenePath);

        if (buildIndex == -1)
        {
            Debug.LogError($"Scene '{sceneName}' not found in Build Settings. Check the path: {scenePath}");
            return;
        }

        Debug.Log($"Create session {sessionName} scene {sceneName} build index {SceneUtility.GetBuildIndexByScenePath($"Scenes/{sceneName}")}");
        var clientTask = InitializeNetworkRunner(networkRunner, GameMode.Shared, GameManager.Instance.GetPlayerConnectionToken(), sessionName, NetAddress.Any(), SceneRef.FromIndex(buildIndex), null); // Initialize the NetworkRunner for hosting a game
    }

    public void JoinGame(SessionInfo sessionInfo, string sceneName)
    {
        string scenePath = $"Assets/Scenes/{sceneName}.unity";
        int buildIndex = SceneUtility.GetBuildIndexByScenePath(scenePath);

        if (buildIndex == -1)
        {
            Debug.LogError($"Scene '{sceneName}' not found in Build Settings. Check the path: {scenePath}");
            return;
        }

        Debug.Log($"Join session {sessionInfo.Name} scene {sceneName} build index {SceneUtility.GetBuildIndexByScenePath($"scenes/{sceneName}")} (sceneref method: {SceneRef.FromIndex(SceneUtility.GetBuildIndexByScenePath($"Scenes/{sceneName}"))}");
        var clientTask = InitializeNetworkRunner(networkRunner, GameMode.Shared, GameManager.Instance.GetPlayerConnectionToken(), sessionInfo.Name, NetAddress.Any(), SceneRef.FromIndex(buildIndex), null); // Initialize the NetworkRunner for hosting a game
    }
}
