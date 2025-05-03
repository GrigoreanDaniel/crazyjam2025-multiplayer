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

    private void Start() {
        HideAll();
    }

    private void Update() {
        if (isJailed) {
            jailTimeRemaining -= Time.deltaTime;
            if (jailTimeRemaining <= 0f) {
                isJailed = false;
                ShowReleasedUI(false); // Not trap
            } else {
                UpdateJailTimerUI();
            }
        }

        if (isTrapped) {
            trapTimeRemaining -= Time.deltaTime;
            if (trapTimeRemaining <= 0f) {
                isTrapped = false;
                ShowReleasedUI(true); // Is trap
            } else {
                UpdateTrapTimerUI();
            }
        }
    }


    // Called when jailed by enemy or manually
    public void ShowCaughtUI(float duration, bool isTrap) {
        if (isTrap) {
            isTrapped = true; 
            trapTimeRemaining = duration;
            trapTimerContainer?.SetActive(true);
            trapCaughtMessage?.SetActive(true);
            trapIconRed?.SetActive(true);
            trapIconNeutral?.SetActive(true);
            hasShownTrapReleasedUI = false; // Reset for new trap event
        } else {
            isJailed = true;
            jailTimeRemaining = duration;
            jailTimerContainer?.SetActive(true);
            caughtMessage?.SetActive(true);
            jailIconRed?.SetActive(true);
            jailIconNeutral?.SetActive(true);
        }

        StartCoroutine(HideCaughtUIAfterSeconds(2f));
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

        StartCoroutine(HideReleasedUIAfterSeconds(2f));
    }

    private IEnumerator HideReleasedUIAfterSeconds(float delay) {
        yield return new WaitForSeconds(delay);
        HideAll();
    }

    public void HideAll() {
        caughtMessage?.SetActive(false);
        jailIconRed?.SetActive(false);
        releasedMessage?.SetActive(false);
        jailIconGreen?.SetActive(false);
        jailIconNeutral?.SetActive(false);

        trapCaughtMessage?.SetActive(false);
        trapIconRed?.SetActive(false);
        trapReleasedMessage?.SetActive(false);
        trapIconGreen?.SetActive(false);
        trapIconNeutral?.SetActive(false);
        
        jailTimerContainer?.SetActive(false);
        trapTimerContainer?.SetActive(false);
    }

    private void UpdateJailTimerUI() {
        if (jailTimerText != null) {
            int minutes = Mathf.FloorToInt(jailTimeRemaining / 60f);
            int seconds = Mathf.FloorToInt(jailTimeRemaining % 60f);
            jailTimerText.text = $"{minutes:D1}:{seconds:D2}";
        }
    }

    private void UpdateTrapTimerUI() {
        if (trapTimerText != null) {
            int displayTime = Mathf.Max(0, Mathf.FloorToInt(trapTimeRemaining + 0.01f));
            trapTimerText.text = displayTime.ToString(); // Only seconds
        }
    }

}
