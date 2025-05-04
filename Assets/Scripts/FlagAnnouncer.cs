using UnityEngine;

public class FlagAnnouncer : MonoBehaviour {
    private MessageDisplayer _messageDisplayer;

    private void Awake() {
        _messageDisplayer = FindObjectOfType<MessageDisplayer>();
        if (_messageDisplayer == null)
            Debug.LogWarning("[FlagAnnouncer] No MessageDisplayer found in scene.");
    }

    private void OnEnable() {
        FlagEvents.OnFlagPickedUp += HandleFlagPickedUp;
        FlagEvents.OnFlagDropped += HandleFlagDropped;
        FlagEvents.OnFlagReturned += HandleFlagReturned;
    }

    private void OnDisable() {
        FlagEvents.OnFlagPickedUp -= HandleFlagPickedUp;
        FlagEvents.OnFlagDropped -= HandleFlagDropped;
        FlagEvents.OnFlagReturned -= HandleFlagReturned;
    }

    private void HandleFlagPickedUp(Flag flag, PlayerFlagCarrier carrier) {
        var flagTeam = flag.GetComponent<TeamIdentifier>()?.Team;
        var carrierTeam = carrier.Team;

        if (flagTeam == null || carrierTeam == null) return;

        if (_messageDisplayer == null) return;

        if (carrierTeam == flagTeam) {
            // Picked up own flag
            _messageDisplayer.ShowSingleMessage(MessageDisplayer.MessageType.FriendlyFlagReturned, 2.5f);
            _messageDisplayer.ShowSingleMessage(MessageDisplayer.MessageType.FriendlyFlagReturnedIcon, 2.5f);
        } else {
            // Picked up enemy flag
            _messageDisplayer.ShowSingleMessage(MessageDisplayer.MessageType.EnemyFlagStolen, 2.5f);
            _messageDisplayer.ShowSingleMessage(MessageDisplayer.MessageType.EnemyFlagStolenIcon, 2.5f);
        }
    }

    private void HandleFlagDropped(Flag flag, PlayerFlagCarrier carrier) {
        // You can optionally show a dropped flag message if desired
        // e.g., _messageDisplayer.ShowSingleMessage(MessageDisplayer.MessageType.FlagDropped, 2f);
    }

    private void HandleFlagReturned(Flag flag) {
        TeamData flagTeam = flag.GetComponent<TeamIdentifier>()?.Team;
        if (flagTeam == null) return;

        // Determine who is local player team
        var localPlayer = FindAnyObjectByType<PlayerFlagInput>();
        TeamData localTeam = localPlayer?.Team;

        if (localTeam == flagTeam) {
            _messageDisplayer.ShowSingleMessage(MessageDisplayer.MessageType.FriendlyFlagReturned, 2f);
            _messageDisplayer.ShowSingleMessage(MessageDisplayer.MessageType.FriendlyFlagReturnedIcon, 2f);
        } else {
            _messageDisplayer.ShowSingleMessage(MessageDisplayer.MessageType.EnemyFlagReturned, 2f);
            _messageDisplayer.ShowSingleMessage(MessageDisplayer.MessageType.EnemyFlagReturnedIcon, 2f);
        }
    }

}
