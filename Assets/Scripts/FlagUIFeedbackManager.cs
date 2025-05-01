using UnityEngine;
using TMPro;
using System.Collections;

public class FlagUIFeedbackManager : MonoBehaviour {
    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Display Settings")]
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private float displayDuration = 2f;

    private Coroutine currentRoutine;

    public void ShowMessage(string message) {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(FadeMessageRoutine(message));
    }

    private IEnumerator FadeMessageRoutine(string message) {
        messageText.text = message;

        // Fade In
        float t = 0f;
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
    }
}
