using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour {

    [SerializeField] private GameObject playerPrefab; // Optional, in case you want to instantiate
    [SerializeField] private TeamData team;           // Assigned in inspector
    [SerializeField] private AbilityCooldownUI abilityCooldownUI;
    [SerializeField] private GameObject abilityUIPrefab;

    public static List<GameObject> AllPlayers = new();

    private SpawnManager spawnManager;

    private void Start() {
        spawnManager = FindObjectOfType<SpawnManager>();
        Debug.Log("[PlayerSpawner] Start called.");
        if (spawnManager == null) {
            Debug.LogError("No SpawnManager found in scene.");
            return;
        }

        StartCoroutine(SpawnAfterDelay());
    }

    private IEnumerator SpawnAfterDelay() {
        yield return new WaitForSeconds(0.1f);

        if (team == null) {
            Debug.LogError("Team not assigned to PlayerSpawner.");
            yield break;
        }

        var spawnPoint = spawnManager.GetRandomSpawnPointForTeam(team);
        if (spawnPoint != null) {
            Debug.Log($"Spawning player for {team.teamName} at: {spawnPoint.position}");
            if (playerPrefab != null) {
                Vector3 pos = spawnPoint.position + Vector3.up * 1.5f;
                var instance = Instantiate(playerPrefab, pos, spawnPoint.rotation);
                AllPlayers.Add(instance);  // Track the live instance
                Debug.Log($"[PlayerSpawner] Instantiated player: {instance.name} at {pos}");
                // NEW: Hook to UI cooldown
                if (abilityUIPrefab != null)
                {
                    GameObject uiInstance = Instantiate(abilityUIPrefab);

                    // Optional: assign this to a specific canvas if needed
                    Canvas mainCanvas = FindObjectOfType<Canvas>(); // Or reference your HUD canvas directly
                    if (mainCanvas != null)
                        uiInstance.transform.SetParent(mainCanvas.transform, false);

                    var cooldownUI = uiInstance.GetComponent<AbilityCooldownUI>();
                    if (cooldownUI != null)
                        cooldownUI.Setup(instance);
                }

            }
            else {
                Debug.LogError("Player prefab is null!");
            }
        } else {
            Debug.LogError($"No valid spawn point found for team {team.teamName}");
        }
    }
}