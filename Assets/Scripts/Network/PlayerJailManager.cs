using Fusion;
using UnityEngine;

public class PlayerJailManager : NetworkBehaviour
{
    [SerializeField] private JailZone redTeamJailZone;
    [SerializeField] private JailZone blueTeamJailZone;
    [SerializeField] private float jailDuration = 10f;

    [Networked] private TickTimer JailTimer { get; set; }
    private CharacterController characterController;
    private NetworkPlayer networkPlayer;

    private bool wasJailedLastTick = false;
    public bool IsJailed => JailTimer.ExpiredOrNotRunning(Runner) == false;

    JailUIManager jailUIManager;

    public override void Spawned()
    {
        characterController = GetComponent<CharacterController>();
        networkPlayer = GetComponent<NetworkPlayer>();

        jailUIManager = GameObject.Find("JailCanvas").GetComponent<JailUIManager>();

        redTeamJailZone = GameObject.Find("RedJailZone").GetComponent<JailZone>();
        blueTeamJailZone = GameObject.Find("BlueJailZone").GetComponent<JailZone>();
    }

    public void JailPlayer()
    {
        if (!Runner.IsServer) return; // Only server/state authority can jail

        if (IsJailed) return; // Already jailed

        Vector3 jailPosition = GetJailZoneForTeam(networkPlayer.TeamIndex).GetJailPosition();
        characterController.enabled = false; // Disable to teleport safely
        transform.position = jailPosition;
        characterController.enabled = true;

        Debug.Log($"[Jail] Player {Object.InputAuthority} jailed for {jailDuration} seconds");

        JailTimer = TickTimer.CreateFromSeconds(Runner, jailDuration);
        wasJailedLastTick = true;

        networkPlayer.SetMovementEnabled(false);

        Rpc_ShowCaughtUI();
    }

    public override void FixedUpdateNetwork()
    {
        if (IsJailed)
        {
            if (!wasJailedLastTick)
            {
                // Entered jail this tick
                networkPlayer.SetMovementEnabled(false);
                wasJailedLastTick = true;
            }
        }
        else if (wasJailedLastTick)
        {
            // Just released from jail
            networkPlayer.SetMovementEnabled(true);
            wasJailedLastTick = false;

            Debug.Log($"[Jail] Player {Object.InputAuthority} released from jail");
        }
    }

    private JailZone GetJailZoneForTeam(int teamIndex)
    {
        return teamIndex == 0 ? blueTeamJailZone : redTeamJailZone; // Reverse: red team jails in blue zone and vice versa
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.InputAuthority)]
    public void Rpc_ShowCaughtUI()
    {
        Debug.Log("[JailUITrigger] Showing Caught UI via RPC");
        jailUIManager.ShowCaughtUI(jailDuration, false);
    }
}
