using Fusion;
using UnityEngine;


public class PlayerSpawner : NetworkBehaviour, IPlayerJoined, IPlayerLeft
{

    [SerializeField] private NetworkPrefabRef playerPrefab;
    [Networked, Capacity(6)] private NetworkDictionary<PlayerRef, Player> Players => default; // Dictionary to store players

    // Called when a player joins the game
    public void PlayerJoined(PlayerRef player)
    {
        if (HasStateAuthority)
        {
            // Spawn the player prefab for the new player
            SpawnPlayer(Runner, player);
        }
    }

    // Called when a player leaves the game
    public void PlayerLeft(PlayerRef player)
    {
        if (!HasStateAuthority)
        {
            return;
        }

        // Clean up the player object when they leave
        CleanupPlayer(player);


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
        Players.Add(player, playerObject.GetComponent<Player>()); // Add the player to the dictionary
    }

    // Method to clean up a player when they leave
    private void CleanupPlayer(PlayerRef player)
    {
        if (Players.TryGet(player, out Player playerBehaviour))
        {
            Players.Remove(player);
            Runner.Despawn(playerBehaviour.Object); // Despawn the player's object
        }
    }



}
