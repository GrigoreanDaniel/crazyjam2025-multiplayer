using UnityEngine;
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
        FriendlyFlagStolen
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
    }

    /// <summary>
    /// Shows the specified message type for the given duration.
    /// If the GameObject has a TimerText component, it will update during the countdown.
    /// </summary>
    public void ShowMessage(MessageType type, float duration = 2f) {
        HideAllMessages();

        // Set up the chosen message
        currentMessageObject = GetMessageObject(type);
        if (currentMessageObject)
            currentMessageObject.SetActive(true);

        // Try to get a TimerText component if one exists
        currentTimerText = currentMessageObject ? currentMessageObject.GetComponent<TimerText>() : null;
        if (currentTimerText != null) {
            // initialize display
            currentTimerText.SetTimerText(FormatTime(duration));
        }

        countdownTimer = duration;
        timerActive = duration > 0f;
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
            case MessageType.FriendlyFlagStolen: return FriendlyFlagStolen;
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
            md.ShowMessage(testType);
        }
    }
}
#endif