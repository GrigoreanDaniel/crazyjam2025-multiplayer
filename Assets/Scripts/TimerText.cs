using TMPro;
using UnityEngine;

public class TimerText : MonoBehaviour {

    public TMP_Text timerText;

    public void SetTimerText(string text) {
        timerText.text = text;
    }

    public void Hide() {
        timerText.text = string.Empty;
    }
}
