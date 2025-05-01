using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class JailUIManager : MonoBehaviour{

    [Header("UI Elements")]
    [SerializeField] private GameObject jailMessagePanel; // Parent object or just the "You are Jailed!" text
    [SerializeField] private TMP_Text jailTimerText;
    [SerializeField] private TMP_Text jailReasonText;

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
        if (jailReasonText != null) {
            jailReasonText.gameObject.SetActive(true);
            jailReasonText.text = (reason == "Trap") ? "Bear Trap!" : "Jail Time!";
        }

        UpdateJailTimerText();
    }

    public void HideJailUI() {
        isJailed = false;

        if (jailMessagePanel != null) jailMessagePanel.SetActive(false);
        if (jailTimerText != null) jailTimerText.gameObject.SetActive(false);
        if (jailReasonText != null) jailReasonText.gameObject.SetActive(false);
    }


    private void UpdateJailTimerText(){

        if (jailTimerText != null){

            jailTimerText.text = $"Time Remaining: {Mathf.CeilToInt(jailTimeRemaining)}s";
        }
    }
}
