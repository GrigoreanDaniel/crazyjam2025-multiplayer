using UnityEngine;

public class JailUIManager : MonoBehaviour {

    private MessageDisplayer _messageDisplayer;

    private void Start() {
        _messageDisplayer = FindFirstObjectByType<MessageDisplayer>();

        if (_messageDisplayer == null)
            Debug.LogWarning("[JailUIManager] MessageDisplayer not found in scene.");
    }

    public void ShowCaughtUI(float totalDuration, bool isTrap) {
        if (_messageDisplayer == null) return;

        if (isTrap) {
            float neutralDuration = 5f;
            float introDuration = 2f;

            // Stage 1: Quick red message
            _messageDisplayer.ShowSingleMessage(MessageDisplayer.MessageType.Trap, introDuration);
            _messageDisplayer.ShowSingleMessage(MessageDisplayer.MessageType.TrapRed, introDuration);

            // Stage 2: Persistent neutral + timer
            _messageDisplayer.QueueMessage(MessageDisplayer.MessageType.TrapNeutral, neutralDuration);
            _messageDisplayer.QueueMessage(MessageDisplayer.MessageType.TrapTimer, neutralDuration);
        } else {
            float neutralDuration = 100f;
            float introDuration = 2f;

            // Stage 1: Quick red message
            _messageDisplayer.ShowSingleMessage(MessageDisplayer.MessageType.Caught, introDuration);
            _messageDisplayer.ShowSingleMessage(MessageDisplayer.MessageType.JailRed, introDuration);

            // Stage 2: Persistent neutral + timer
            _messageDisplayer.QueueMessage(MessageDisplayer.MessageType.JailNeutral, neutralDuration);
            _messageDisplayer.QueueMessage(MessageDisplayer.MessageType.JailTimer, neutralDuration);
        }
    }

    public void ShowReleasedUI(float duration, bool isTrap) {
        if (_messageDisplayer == null) return;

        float releaseDuration = 2f;

        _messageDisplayer.ShowSingleMessage(MessageDisplayer.MessageType.Released, releaseDuration);
        if (isTrap)
            _messageDisplayer.ShowSingleMessage(MessageDisplayer.MessageType.TrapGreen, releaseDuration);
        else
            _messageDisplayer.ShowSingleMessage(MessageDisplayer.MessageType.JailGreen, releaseDuration);
    }
}
