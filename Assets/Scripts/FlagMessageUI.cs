using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FlagMessageUI : MonoBehaviour {
    [Header("UI References")]
    [SerializeField] private GameObject flagMessagePanel;
    [SerializeField] private TMP_Text flagMessageText;
    [SerializeField] private Image flagIcon;

    [Header("Timing")]
    [SerializeField] private float messageDisplayDuration = 2.5f;

    private Coroutine hideRoutine;

    private void OnEnable() {
        FlagAnnouncer.OnLocalMessage += ShowMessage;
    }

    private void OnDisable() {
        FlagAnnouncer.OnLocalMessage -= ShowMessage;
    }

    public void ShowMessage(string message, TeamData team) {
        if (hideRoutine != null) StopCoroutine(hideRoutine);

        flagMessageText.text = message;
        flagMessageText.color = team.teamColor;
        flagIcon.color = team.teamColor;

        flagMessagePanel.SetActive(true);
        flagIcon.gameObject.SetActive(true);

        hideRoutine = StartCoroutine(HideAfterDelay());
    }

    private IEnumerator HideAfterDelay() {
        yield return new WaitForSeconds(messageDisplayDuration);
        HideMessage();
    }

    public void HideMessage() {
        flagMessagePanel.SetActive(false);
        flagIcon.gameObject.SetActive(false);
    }
}
