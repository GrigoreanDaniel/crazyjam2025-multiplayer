using UnityEngine;
using Cinemachine;

public class PlayerDizzyEffect : MonoBehaviour {
    [SerializeField] private Transform cameraToRotate;
    [SerializeField] private float rotationSpeed = 200f;
    [SerializeField] private CinemachineFreeLook cinemachineCamera;

    private bool isDizzy = false;
    public bool IsDizzy => isDizzy;

    public void ApplyDizziness(float duration) {
        if (isDizzy || cameraToRotate == null) return;
        StartCoroutine(SpinCamera(duration));
    }

    private System.Collections.IEnumerator SpinCamera(float duration) {
        isDizzy = true;

        // Disable manual camera input
        if (cinemachineCamera != null) {
            cinemachineCamera.m_XAxis.m_InputAxisName = "";
            cinemachineCamera.m_YAxis.m_InputAxisName = "";
        }
        var lookAtBackup = cinemachineCamera.LookAt;
        cinemachineCamera.LookAt = null;

        // Spin the camera
        float timer = 0f;
        while (timer < duration) {
            cinemachineCamera.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
            timer += Time.deltaTime;
            Debug.Log("Dizzy Spin: " + cameraToRotate.rotation.eulerAngles);
            yield return null;
        }

        cinemachineCamera.LookAt = lookAtBackup;

        // Restore LookAt + camera input
        if (cinemachineCamera != null) {
            cinemachineCamera.LookAt = lookAtBackup;
            cinemachineCamera.m_XAxis.m_InputAxisName = "Mouse X";
            cinemachineCamera.m_YAxis.m_InputAxisName = "Mouse Y";
        }

        isDizzy = false;
    }
}
