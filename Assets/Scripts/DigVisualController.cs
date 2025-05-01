using UnityEngine;

public class DigVisualController : MonoBehaviour {
    [Header("Visual Components")]
    [SerializeField] private GameObject playerMesh;
    [SerializeField] private GameObject footsteps;
    [SerializeField] private ParticleSystem digFX;
    [SerializeField] private AudioSource digAudio;

    [Header("Sonar Detection")]
    [SerializeField] private MonoBehaviour sonarComponent; // Replace with your actual component

    public void ApplyDigState(bool isDigging) {
        if (playerMesh != null)
            playerMesh.SetActive(!isDigging);

        if (footsteps != null)
            footsteps.SetActive(!isDigging);

        if (digFX != null) {
            if (isDigging) digFX.Play();
            else digFX.Stop();
        }

        if (digAudio != null) {
            if (isDigging) digAudio.Play();
            else digAudio.Stop();
        }

        if (sonarComponent != null)
            sonarComponent.enabled = !isDigging;
    }
}
