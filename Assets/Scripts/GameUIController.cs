using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class GameUIController : MonoBehaviour {
    [Header("Team Info")]
    [SerializeField] private TextMeshProUGUI blueTeamName;
    [SerializeField] private TextMeshProUGUI redTeamName;

    [Header("Progress Bars")]
    [SerializeField] private Slider blueProgressBar;
    [SerializeField] private Slider redProgressBar;

    [Header("Round Info")]
    [SerializeField] private TextMeshProUGUI blueRoundCount;
    [SerializeField] private TextMeshProUGUI redRoundCount;

    [Header("Round Timer")]
    [SerializeField] private TextMeshProUGUI roundTimerText;

    [Header("Crowns")]
    [SerializeField] private GameObject blueCrown;
    [SerializeField] private GameObject redCrown;

    [Header("Center Message")]
    [SerializeField] private TextMeshProUGUI centerMessage;

    [SerializeField] private TeamData blueTeam;
    [SerializeField] private TeamData redTeam;

    public void UpdateTeamInfo(TeamData blue, TeamData red) {
        blueTeam = blue;
        redTeam = red;

        blueTeamName.text = blue.teamName;
        redTeamName.text = red.teamName;
    }

    public void UpdateProgress(float blueScore, float redScore, float maxScore) {
        blueProgressBar.value = blueScore / maxScore;
        redProgressBar.value = redScore / maxScore;
    }

    public void UpdateRounds(int blueRounds, int redRounds) {
        blueRoundCount.text = blueRounds.ToString();
        redRoundCount.text = redRounds.ToString();
    }

    public void UpdateRoundTimer(float timeLeft) {
        roundTimerText.text = $"{timeLeft:0}";
    }

    public void UpdateRoundClock(float timeRemaining) {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        roundTimerText.text = $"{minutes}:{seconds:D2}";
    }

    public void ShowCenterMessage(string message, float duration = 2f) {
        centerMessage.text = message;
        centerMessage.gameObject.SetActive(true);
        StartCoroutine(HideCenterMessageAfter(duration));
    }

    private IEnumerator HideCenterMessageAfter(float delay) {
        yield return new WaitForSeconds(delay);
        centerMessage.gameObject.SetActive(false);
    }

    public void ShowCrown(TeamData leadingTeam) {
        blueCrown.SetActive(leadingTeam == blueTeam);
        redCrown.SetActive(leadingTeam == redTeam);
    }
}
