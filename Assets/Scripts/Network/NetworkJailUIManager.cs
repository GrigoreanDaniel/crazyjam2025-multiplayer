using TMPro;
using UnityEngine;

public class NetworkJailUIManager : MonoBehaviour
{
    public static NetworkJailUIManager Instance { get; private set; }

    [SerializeField] private GameObject jailPanel;
    [SerializeField] private TMP_Text jailTimerText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        HideJailUI();
    }

    public void ShowJailUI(float duration)
    {
        if (jailPanel != null) jailPanel.SetActive(true);
        if (jailTimerText != null) jailTimerText.gameObject.SetActive(true);
        UpdateJailTimerText(duration);
    }

    public void UpdateJailTimerText(float time)
    {
        if (jailTimerText != null)
        {
            jailTimerText.text = $"Time Remaining: {Mathf.CeilToInt(time)}s";
        }
    }

    public void HideJailUI()
    {
        if (jailPanel != null) jailPanel.SetActive(false);
        if (jailTimerText != null) jailTimerText.gameObject.SetActive(false);
    }
}
