using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JailUIManager : MonoBehaviour{

    [Header("UI Elements")]
    [SerializeField] private GameObject jailMessagePanel; // Parent object or just the "You are Jailed!" text
    [SerializeField] private TMP_Text jailTimerText;
    [SerializeField] private TMP_Text jailReasonText;
    [SerializeField] private Image jailIcon;

    private bool isJailed = false;
    private float jailTimeRemaining = 0f;

    private void Start(){
        HideJailUI();
    }

    private void Update(){

        if (!isJailed) return;

        jailTimeRemaining -= Time.deltaTime;
        if (jailTimeRemaining < 0f) jailTimeRemaining = 0f;

        UpdateJailTimerText();
    }

    public void ShowJailUI(float duration, string reason) {
        isJailed = true;
        jailTimeRemaining = duration;

        if (jailMessagePanel != null) jailMessagePanel.SetActive(true);
        if (jailTimerText != null) jailTimerText.gameObject.SetActive(true);
        if (jailIcon != null) jailIcon.gameObject.SetActive(true);
        if (jailReasonText != null) {
            jailReasonText.gameObject.SetActive(true);
            jailReasonText.text = (reason == "Trap") ? "Bear Trap!" : "You've been caught!";
        }

        UpdateJailTimerText();
    }

    public void HideJailUI() {
        isJailed = false;

        if (jailMessagePanel != null) jailMessagePanel.SetActive(false);
        if (jailTimerText != null) jailTimerText.gameObject.SetActive(false);
        if (jailReasonText != null) jailReasonText.gameObject.SetActive(false);
        if (jailIcon != null) jailIcon.gameObject.SetActive(false);
    }


    private void UpdateJailTimerText(){

        if (jailTimerText != null){

            jailTimerText.text = $"{Mathf.CeilToInt(jailTimeRemaining)}";
        }
    }
}
