using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;



#if UNITY_EDITOR
using UnityEditor;
#endif
public class MessageDisplayer : MonoBehaviour {
    public enum MessageType {
        Caught,
        Released,
        Trap,
        TrapRelease,
        PickUpEnemyFlag,
        PickedUpFriendlyFlag,
        EnemyFlagStolen,
        FriendlyFlagReturned,
        EnemyFlagReturned,
        FriendlyFlagStolen,
        PickUpEnemyFlagBlueTeam,
        PickUpEnemyFlagRedTeam,
        EnemyFlagStolenIcon,
        FriendlyFlagStolenIcon,
        EnemyFlagReturnedIcon,
        FriendlyFlagReturnedIcon,
        JailNeutral,
        JailRed,
        JailGreen,
        TrapNeutral,
        TrapRed,
        TrapGreen,
        JailTimer,
        TrapTimer,
        TeamNameLeft,
        TeamNameRight,
        TeamIconLeft,
        TeamIconRight,
        ScoreBarLeft,
        ScoreBarRight,
        ScoreTimerLeft,
        ScoreTimerRight,
        RoundTimer,
        CrownLeft,
        CrownRight
    }

    [Header("Assign your individual message GameObjects here:")]
    [SerializeField] private GameObject CaughtMessage;
    [SerializeField] private GameObject ReleasedMessage;
    [SerializeField] private GameObject TrapMessage;
    [SerializeField] private GameObject TrapReleaseMessage;
    [SerializeField] private GameObject PickUpEnemyFlagMessage;
    [SerializeField] private GameObject PickedUpFriendlyFlag;
    [SerializeField] private GameObject EnemyFlagStolen;
    [SerializeField] private GameObject FriendlyFlagReturned;
    [SerializeField] private GameObject EnemyFlagReturned;
    [SerializeField] private GameObject FriendlyFlagStolen;

    [SerializeField] private GameObject PickUpEnemyFlagBlueTeamIcon;
    [SerializeField] private GameObject PickUpEnemyFlagRedTeamIcon;
    [SerializeField] private GameObject EnemyFlagStolenIcon;
    [SerializeField] private GameObject FriendlyFlagStolenIcon;
    [SerializeField] private GameObject EnemyFlagReturnedIcon;
    [SerializeField] private GameObject FriendlyFlagReturnedIcon;

    [SerializeField] private GameObject JailNeutralIcon;
    [SerializeField] private GameObject JailRedIcon;
    [SerializeField] private GameObject JailGreenIcon;
    [SerializeField] private GameObject TrapNeutralIcon;
    [SerializeField] private GameObject TrapRedIcon;
    [SerializeField] private GameObject TrapGreenIcon;

    [SerializeField] private GameObject JailTimerText;
    [SerializeField] private GameObject TrapTimerText;

    [SerializeField] private TMP_Text TeamNameLeft;
    [SerializeField] private TMP_Text TeamNameRight;
    [SerializeField] private Image TeamIconLeft;
    [SerializeField] private Image TeamIconRight;
    [SerializeField] private Slider ScoreBarLeft;
    [SerializeField] private Slider ScoreBarRight;
    [SerializeField] private TMP_Text ScoreTimerLeft;
    [SerializeField] private TMP_Text ScoreTimerRight;
    [SerializeField] private TMP_Text RoundTimer;
    [SerializeField] private GameObject CrownLeft;
    [SerializeField] private GameObject CrownRight;


    // Active message tracking
    private GameObject currentMessageObject;
    private TimerText currentTimerText;

    // Countdown timer in seconds. When <= 0, all messages hide.
    private float countdownTimer = 0f;
    private bool timerActive = false;

    private void Update() {
        if (!timerActive)
            return;

        // Decrement
        countdownTimer -= Time.deltaTime;

        // Update timer text if present
        if (currentTimerText != null) {
            currentTimerText.SetTimerText(FormatTime(countdownTimer));
        }

        // Hide when done
        if (countdownTimer <= 0f) {
            HideAllMessages();
            if (currentTimerText != null) {
                currentTimerText.Hide();
            }
            timerActive = false;
            currentMessageObject = null;
            currentTimerText = null;
        }
    }

    public void SetTeamUI(TeamData leftTeam, TeamData rightTeam) {

        TeamNameLeft.text = leftTeam.displayName;
        TeamNameRight.text = rightTeam.displayName;

        TeamIconLeft.sprite = leftTeam.icon;
        TeamIconRight.sprite = rightTeam.icon;

        Debug.Log($"Setting team names: {leftTeam.displayName} | {rightTeam.displayName}");
        Debug.Log($"Left Text Assigned: {TeamNameLeft != null}, Right Text Assigned: {TeamNameRight != null}");

    }

    public void UpdateScoreUI(float leftScore, float rightScore, float leftTime, float rightTime, float roundTime, bool leftLeads) {
        if (ScoreBarLeft != null) ScoreBarLeft.value = leftScore / 100f;
        if (ScoreBarRight != null) ScoreBarRight.value = rightScore / 100f;

        if (ScoreTimerLeft != null) ScoreTimerLeft.text = FormatTime(leftTime);
        if (ScoreTimerRight != null) ScoreTimerRight.text = FormatTime(rightTime);
        if (RoundTimer != null) RoundTimer.text = FormatTime(roundTime);

        bool scoresAreEqual = Mathf.Approximately(leftScore, rightScore);

        if (CrownLeft != null) CrownLeft.SetActive(leftLeads);
        if (CrownRight != null) CrownRight.SetActive(!leftLeads && !scoresAreEqual);
    }

    /// <summary>
    /// Disables all message objects if they are assigned.
    /// </summary>
    private void HideAllMessages() {
        void Hide(GameObject go) { if (go) go.SetActive(false); }

        Hide(CaughtMessage);
        Hide(ReleasedMessage);
        Hide(TrapMessage);
        Hide(TrapReleaseMessage);
        Hide(PickUpEnemyFlagMessage);
        Hide(PickedUpFriendlyFlag);
        Hide(EnemyFlagStolen);
        Hide(FriendlyFlagReturned);
        Hide(EnemyFlagReturned);
        Hide(FriendlyFlagStolen);
        Hide(PickUpEnemyFlagBlueTeamIcon);
        Hide(PickUpEnemyFlagRedTeamIcon);
        Hide(EnemyFlagStolenIcon);
        Hide(FriendlyFlagStolenIcon);
        Hide(EnemyFlagReturnedIcon);
        Hide(FriendlyFlagReturnedIcon);
        Hide(JailNeutralIcon);
        Hide(JailRedIcon);
        Hide(JailGreenIcon);
        Hide(TrapNeutralIcon);
        Hide(TrapRedIcon);
        Hide(TrapGreenIcon);
        Hide(JailTimerText);
        Hide(TrapTimerText);
    }

    /// <summary>
    /// Shows the specified message type for the given duration.
    /// If the GameObject has a TimerText component, it will update during the countdown.
    /// </summary>
    public void ShowSingleMessage(MessageType type, float duration = 2f) {
        GameObject go = GetMessageObject(type);
        if (!go) return;

        go.SetActive(true);

        var timer = go.GetComponent<TimerText>();
        if (timer != null) {
            timer.SetTimerText(FormatTime(duration));
            StartCoroutine(HideAfter(go, timer, duration));
        } else {
            StartCoroutine(HideAfter(go, null, duration));
        }
    }

    private IEnumerator HideAfter(GameObject go, TimerText timer, float delay) {
        float time = delay;
        while (time > 0) {
            if (timer != null) timer.SetTimerText(FormatTime(time));
            time -= Time.deltaTime;
            yield return null;
        }

        go.SetActive(false);
        if (timer != null) timer.Hide();
    }

    // Queues a message after a slight delay without clearing previous ones
    public void QueueMessage(MessageType type, float duration = 2f) {
        GameObject go = GetMessageObject(type);
        if (!go) return;

        go.SetActive(true);

        TimerText timer = go.GetComponent<TimerText>();
        if (timer != null) {
            timer.SetTimerText(FormatTime(duration)); // THIS LINE ENSURES IT STARTS VISIBLY AT FULL VALUE
            StartCoroutine(UpdateAndHide(go, timer, duration));
        } else {
            StartCoroutine(AutoHide(go, duration));
        }
    }

    private IEnumerator UpdateAndHide(GameObject go, TimerText timer, float time) {
        float t = time;
        while (t > 0f) {
            if (go == null || !go.activeSelf) yield break;  // early exit if disabled externally
            timer.SetTimerText(FormatTime(t));
            t -= Time.deltaTime;
            yield return null;
        }

        go.SetActive(false);
        timer.Hide();
    }

    private IEnumerator AutoHide(GameObject obj, float delay) {
        yield return new WaitForSeconds(delay);
        if (obj) obj.SetActive(false);
    }

    /// <summary>
    /// Retrieves the GameObject for a given message type.
    /// </summary>
    private GameObject GetMessageObject(MessageType type) {
        switch (type) {
            case MessageType.Caught: return CaughtMessage;
            case MessageType.Released: return ReleasedMessage;
            case MessageType.Trap: return TrapMessage;
            case MessageType.TrapRelease: return TrapReleaseMessage;
            case MessageType.PickUpEnemyFlag: return PickUpEnemyFlagMessage;
            case MessageType.PickedUpFriendlyFlag: return PickedUpFriendlyFlag;
            case MessageType.EnemyFlagStolen: return EnemyFlagStolen;
            case MessageType.FriendlyFlagReturned: return FriendlyFlagReturned;
            case MessageType.EnemyFlagReturned: return EnemyFlagReturned;
            case MessageType.FriendlyFlagStolen: return FriendlyFlagStolen; // Messages end here
            case MessageType.PickUpEnemyFlagBlueTeam: return PickUpEnemyFlagBlueTeamIcon; // Icons start here
            case MessageType.PickUpEnemyFlagRedTeam: return PickUpEnemyFlagRedTeamIcon;
            case MessageType.EnemyFlagStolenIcon: return EnemyFlagStolenIcon;
            case MessageType.FriendlyFlagStolenIcon: return FriendlyFlagStolenIcon;
            case MessageType.EnemyFlagReturnedIcon: return EnemyFlagReturnedIcon;
            case MessageType.FriendlyFlagReturnedIcon: return FriendlyFlagReturnedIcon;
            case MessageType.JailNeutral: return JailNeutralIcon;
            case MessageType.JailRed: return JailRedIcon;
            case MessageType.JailGreen: return JailGreenIcon;
            case MessageType.TrapNeutral: return TrapNeutralIcon;
            case MessageType.TrapRed: return TrapRedIcon;
            case MessageType.TrapGreen: return TrapGreenIcon; // Icons end here
            case MessageType.JailTimer: return JailTimerText; // Timer starts here
            case MessageType.TrapTimer: return TrapTimerText; // Timer ends here
            case MessageType.TeamNameLeft: return TeamNameLeft?.gameObject;// NEW UI Elements
            case MessageType.TeamNameRight: return TeamNameRight?.gameObject;
            case MessageType.TeamIconLeft: return TeamIconLeft?.gameObject;
            case MessageType.TeamIconRight: return TeamIconRight?.gameObject;
            case MessageType.ScoreBarLeft: return ScoreBarLeft?.gameObject;
            case MessageType.ScoreBarRight: return ScoreBarRight?.gameObject;
            case MessageType.ScoreTimerLeft: return ScoreTimerLeft?.gameObject;
            case MessageType.ScoreTimerRight: return ScoreTimerRight?.gameObject;
            case MessageType.RoundTimer: return RoundTimer?.gameObject;
            case MessageType.CrownLeft: return CrownLeft;
            case MessageType.CrownRight: return CrownRight;
            default: return null;
        }
    }

    /// <summary>
    /// Formats a float time in seconds to MM:SS (if minutes > 0) or SS format without leading zeros when minutes are zero.
    /// </summary>
    private string FormatTime(float time) {
        time = Mathf.Max(time, 0f);
        int minutes = (int)(time / 60);
        int seconds = (int)(time % 60);
        if (minutes > 0)
            return string.Format("{0}:{1:00}", minutes, seconds);
        return seconds.ToString();
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(MessageDisplayer))]
public class MessageDisplayerEditor : Editor {
    private MessageDisplayer.MessageType testType;

    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        GUILayout.Space(8);
        GUILayout.Label("Test Message", EditorStyles.boldLabel);
        testType = (MessageDisplayer.MessageType)EditorGUILayout.EnumPopup("Select Type", testType);

        if (GUILayout.Button("Show Test Message")) {
            var md = (MessageDisplayer)target;
            md.ShowSingleMessage(testType);
        }
    }
}
#endif