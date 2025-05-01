using UnityEngine;

public class PlayerDizzyEffect : MonoBehaviour {
    [SerializeField] private Transform cameraToRotate;
    [SerializeField] private float rotationSpeed = 200f;

    private bool isDizzy = false;

    public void ApplyDizziness(float duration) {
        if (isDizzy || cameraToRotate == null) return;
        StartCoroutine(SpinCamera(duration));
    }

    private System.Collections.IEnumerator SpinCamera(float duration) {
        isDizzy = true;
        float timer = 0f;

        while (timer < duration) {
            cameraToRotate.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }

        isDizzy = false;
    }
}
