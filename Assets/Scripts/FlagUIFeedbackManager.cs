using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class FlagUIFeedbackManager : MonoBehaviour {
    public static FlagUIFeedbackManager Instance { get; private set; }

    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Image flagIconImage;

    [Header("Display Settings")]
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private float displayDuration = 2f;

    private Coroutine currentRoutine;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (flagIconImage != null)
            flagIconImage.enabled = false;

    }

    public void ShowMessage(string message, Color flagColor) {
        messageText.text = message;

        if (flagIconImage != null) {
            flagIconImage.color = flagColor;
            flagIconImage.enabled = true;
        }

            if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(FadeMessageRoutine());
    }


    public void ShowMessage(string message) {
        ShowMessage(message, Color.white); // fallback to white
    }

    private IEnumerator FadeMessageRoutine() {
        float t = 0f;

        // Fade In
        while (t < fadeDuration) {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
            yield return null;
        }

        yield return new WaitForSeconds(displayDuration);

        // Fade Out
        t = 0f;
        while (t < fadeDuration) {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 0f;
        /*if (flagIconImage != null)
            flagIconImage.enabled = false;*/

    }

    public void EnableFlagIcon(Color color) {
        if (flagIconImage != null) {
            flagIconImage.enabled = true;
            flagIconImage.color = color;
        }
    }

    public void DisableFlagIcon() {
        Debug.Log("Disabling Flag Icon");
        if (flagIconImage != null) {
            flagIconImage.enabled = false;
        }
    }


}
