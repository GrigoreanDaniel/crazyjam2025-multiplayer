using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JailUIManager : MonoBehaviour {

    [Header("Caught UI")]
    [SerializeField] private GameObject caughtMessage;
    [SerializeField] private GameObject jailIconRed;

    [Header("Released UI")]
    [SerializeField] private GameObject releasedMessage;
    [SerializeField] private GameObject jailIconGreen;

    [Header("Trap Specific UI")]
    [SerializeField] private GameObject trapCaughtMessage;
    [SerializeField] private GameObject trapIconRed;
    [SerializeField] private GameObject trapReleasedMessage;
    [SerializeField] private GameObject trapIconGreen;

    [SerializeField] private GameObject jailIconNeutral;
    [SerializeField] private GameObject trapIconNeutral;

    [Header("Timer Display")]
    [SerializeField] private TMP_Text jailTimerText;
    [SerializeField] private TMP_Text trapTimerText;
    [SerializeField] private GameObject jailTimerContainer;
    [SerializeField] private GameObject trapTimerContainer;

    private float jailTimeRemaining = 0f;
    private bool isJailed = false;

    private float trapTimeRemaining = 0f;
    private bool isTrapped = false;

    private bool hasShownTrapReleasedUI = false;

    private MessageDisplayer _messageDisplayer;

    private void Start() {
        _messageDisplayer = FindFirstObjectByType<MessageDisplayer>();
    }

    // Called when jailed by enemy or manually
    public void ShowCaughtUI(float duration, bool isTrap) {
        if (isTrap) {
            _messageDisplayer.ShowMessage(MessageDisplayer.MessageType.Trap, duration);
        } else {
            _messageDisplayer.ShowMessage(MessageDisplayer.MessageType.Caught, duration);
        }
    }



    private IEnumerator HideCaughtUIAfterSeconds(float delay) {
        yield return new WaitForSeconds(delay);
        caughtMessage?.SetActive(false);
        jailIconRed?.SetActive(false);
        trapCaughtMessage?.SetActive(false);
        trapIconRed?.SetActive(false);
    }



    // Called when released or time ends
    public void ShowReleasedUI(bool isTrap) {
        if (isTrap) {
            trapIconRed?.SetActive(false);
            trapCaughtMessage?.SetActive(false);
            trapIconGreen?.SetActive(true);
            trapReleasedMessage?.SetActive(true);
            trapIconNeutral?.SetActive(false);
        } else {
            jailIconRed?.SetActive(false);
            caughtMessage?.SetActive(false);
            jailIconGreen?.SetActive(true);
            releasedMessage?.SetActive(true);
            jailIconNeutral?.SetActive(false);
        }
    }
}
