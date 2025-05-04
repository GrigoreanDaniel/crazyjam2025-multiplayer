using TMPro;
using UnityEngine;

public class TimerText : MonoBehaviour {

    public TMP_Text timerText;

    public void SetTimerText(string text) {
        if (timerText != null)
            timerText.text = text;
        else
            Debug.LogWarning("[TimerText] timerText is null!");
    }


    public void Hide() {
        timerText.text = string.Empty;
    }
}
