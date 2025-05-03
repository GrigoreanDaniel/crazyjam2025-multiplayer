using UnityEngine;
using System;

public class FlagAnnouncer : MonoBehaviour {
    public static Action<string, TeamData> OnLocalMessage;  // Message, team (for color/icon)
    public static Action<string, TeamData> OnAllyMessage;   // Message, team
    public static Action<string, TeamData> OnEnemyMessage;  // Message, team

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
        TeamData flagTeam = flag.GetComponent<TeamIdentifier>().Team;
        TeamData carrierTeam = carrier.Team;

        if (flagTeam == null || carrierTeam == null) return;

        if (carrierTeam == flagTeam) {
            // Picked up own flag
            OnLocalMessage?.Invoke("You picked up your flag!", carrierTeam);
            OnAllyMessage?.Invoke("Your flag has returned!", carrierTeam);
            OnEnemyMessage?.Invoke("Enemy's flag returned!", flagTeam);
        } else {
            // Picked up enemy flag
            OnLocalMessage?.Invoke("You picked up enemy's flag!", flagTeam);
            OnAllyMessage?.Invoke("Enemy's flag stolen!", flagTeam);
            OnEnemyMessage?.Invoke("The enemy has your flag!", flagTeam);
            Debug.Log("Picked up enemy flag! Should display UI message.");
        }

    }

    private void HandleFlagDropped(Flag flag, PlayerFlagCarrier carrier) {
        // Optional: could notify here if desired
    }

    private void HandleFlagReturned(Flag flag) {
        TeamData flagTeam = flag.GetComponent<TeamIdentifier>().Team;

        OnAllyMessage?.Invoke("Your flag has returned!", flagTeam);
        OnEnemyMessage?.Invoke("Enemy's flag returned!", flagTeam);
    }
}
