using UnityEngine;

public class TrapZone : MonoBehaviour {
    [Header("Trap Settings")]
    [SerializeField] private float trapDuration;
    [SerializeField] private bool applyDizzyEffect = false;
    [SerializeField] private float dizzinessDuration = 2.5f;

    [Header("Optional Visuals")]
    [SerializeField] private GameObject trapVisualEffect;
    [SerializeField] private AudioClip trapSound;
    [SerializeField] private AudioSource audioSource;

    private void OnTriggerEnter(Collider other) {
        Debug.Log("Something entered the trap zone: " + other.name);

        if (!other.CompareTag("Player")) return;

        Debug.Log("Player entered trap zone!");

        // TRIGGER JAIL via existing system
        PlayerJailHandler jail = other.GetComponent<PlayerJailHandler>();
        if (jail != null) {
            jail.TriggerJail("Trap", trapDuration, dizzinessDuration);
        }

        if (!applyDizzyEffect) {
            Debug.Log("Dizzy effect is OFF for this trap.");
        }

        // Optional VFX & sound
        if (trapVisualEffect != null)
            Instantiate(trapVisualEffect, transform.position, Quaternion.identity);

        if (trapSound != null && audioSource != null)
            audioSource.PlayOneShot(trapSound);
    }


    private System.Collections.IEnumerator ReleasePlayerAfterDelay(PlayerMovement player, float delay) {
        yield return new WaitForSeconds(delay);
        player.enabled = true;
    }
}
