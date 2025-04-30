using UnityEngine;

public class Footstep : MonoBehaviour {
    [SerializeField] private float lifetime = 2f;

    private void Start() {
        Destroy(gameObject, lifetime);
    }

    // Optional: Could add animation/fade logic here later
}
