using Fusion;
using UnityEngine;
using TMPro;
using Cinemachine;

public class NetworkPlayer : NetworkBehaviour, IPlayerLeft
{
    [SerializeField] private TextMeshProUGUI playerNameText;
    public static NetworkPlayer LocalPlayer { get; private set; }
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float rotationSpeed = 20f;
    [SerializeField] private CinemachineVirtualCamera cineMachinevirtualCamera;
    CinemachineBrain cinemachineBrain;
    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    //Jail properties
    [SerializeField] private JailZone redTeamJailZone;
    [SerializeField] private JailZone blueTeamJailZone;
    [SerializeField] private JailUIManager jailUI;
    [SerializeField] private float jailDuration = 10f;

    private bool isJailed = false;
    private float jailTimer;

    [Networked, OnChangedRender(nameof(OnTeamIndexChangedRender))]
    public int TeamIndex { get; set; }

    [SerializeField] private TeamData[] availableTeams; // Assign in inspector
    public TeamData CurrentTeam { get; private set; }

    [Networked, OnChangedRender(nameof(OnPlayerNameChanged))]
    [SerializeField] public NetworkString<_16> PlayerNickName { get; set; }

    [Networked] public int PlayerToken { get; set; }

    // Start is called before the first frame update
    void Awake()
    {
        // Lock and hide the cursor when game starts
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        controller = GetComponent<CharacterController>();
    }

    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            LocalPlayer = this;
            cinemachineBrain = FindObjectOfType<CinemachineBrain>();

            if (cineMachinevirtualCamera != null)
            {
                cineMachinevirtualCamera.Priority = 100;
                cineMachinevirtualCamera.Follow = transform;
                cineMachinevirtualCamera.LookAt = transform;
            }

            PlayerNickName = PlayerPrefs.GetString("PlayerName");
            RPC_SetPlayerName(PlayerNickName.ToString());

            Debug.Log("spawned local Player.");

            jailUI = GameObject.Find("JailCanvas").GetComponent<JailUIManager>();
        }

        if (Object.HasStateAuthority)
        {
            // Assign team only on the State Authority
            int teamCount = TeamManager.Instance.AvailableTeams.Length;
            int nextTeam = TeamManager.Instance.GetNextTeamIndex(); // We’ll write this method below
            TeamIndex = nextTeam;
        }

        UpdateTeam(TeamIndex); // Update visuals/UI/etc. immediately

        OnPlayerNameChanged();

        transform.name = $"P_{Object.Id}";

    }

    public void PlayerLeft(PlayerRef player)
    {
        if (player == Object.InputAuthority)
        {
            Runner.Despawn(Object);
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (!Object.HasStateAuthority) return;

        if (isJailed)
        {
            jailTimer -= Runner.DeltaTime;
            if (jailTimer <= 0f)
            {
                Rpc_ShowReleasedUI();
            }
            return;
        }

        Vector3 inputDirection = Vector3.zero;

        if (GetInput(out NetworkInputData networkInputData))
        {
            inputDirection = networkInputData.movementInput.normalized;

            if (inputDirection.magnitude != 0f)
            {
                if (isGrounded && networkInputData.isJumpPressed)
                {
                    velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                }
            }
        }

        if (Object.HasStateAuthority)
        {
            isGrounded = controller.isGrounded;

            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            if (inputDirection.magnitude != 0f)
            {
                //Get camera forward/right projected to horizontal plane
                Vector3 camForward = cineMachinevirtualCamera.transform.forward;
                Vector3 camRight = cineMachinevirtualCamera.transform.right;

                camForward.y = 0f;
                camRight.y = 0f;
                camForward.Normalize();
                camRight.Normalize();

                Vector3 moveDirection = camForward * networkInputData.movementInput.y + camRight * networkInputData.movementInput.x;
                controller.Move(moveDirection * moveSpeed * Runner.DeltaTime);

                // Smoothly rotate toward movement
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Runner.DeltaTime);
            }
            // Apply gravity
            velocity.y += gravity * Runner.DeltaTime;
            controller.Move(new Vector3(0, velocity.y, 0) * Runner.DeltaTime);
        }

        if (networkInputData.isAttackPressed)
        {
            TryAttack();
        }
    }

    public override void Render()
    {
        if (Object.HasInputAuthority)
        {
            cinemachineBrain.ManualUpdate();
            cineMachinevirtualCamera.UpdateCameraState(Vector3.up, Runner.LocalAlpha);
        }
    }

    private void OnPlayerNameChanged()
    {
        Debug.Log($"Player name changed to {PlayerNickName} for player {gameObject.name}");
        playerNameText.text = PlayerNickName.ToString();
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_SetPlayerName(string playerName)
    {
        PlayerNickName = playerName;

        Debug.Log($"[RPC] set nickname {PlayerNickName}");
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void Rpc_RequestJail()
    {
        if (isJailed) return;

        isJailed = true;
        controller.enabled = false;
        Debug.Log($"[Jail] Host received jail request for {PlayerNickName}");

        // Move to opposing team's jail
        TeamIdentifier teamId = GetComponent<TeamIdentifier>();
        JailZone targetZone = teamId.Team.teamName == "Red" ? blueTeamJailZone : redTeamJailZone;
        transform.position = targetZone.jailPoint.position;
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void Rpc_RequestJailTarget(NetworkId targetPlayerId)
    {
        Debug.Log($"[RPC] Request to jail player with ID {targetPlayerId} received");

        var targetObject = Runner.FindObject(targetPlayerId);
        if (targetObject == null)
        {
            Debug.LogWarning($"[RPC] No object found with ID {targetPlayerId}");
            return;
        }

        var targetPlayer = targetObject.GetComponent<NetworkPlayer>();
        if (targetPlayer == null)
        {
            Debug.LogWarning($"[RPC] Object with ID {targetPlayerId} is not a NetworkPlayer");
            return;
        }

        // Double-check that attacker and target are on opposing teams
        var myTeam = GetComponent<TeamIdentifier>()?.Team;
        var theirTeam = targetPlayer.GetComponent<TeamIdentifier>()?.Team;

        if (myTeam != null && theirTeam != null && myTeam != theirTeam)
        {
            Debug.Log($"[RPC] Jailing target player {targetPlayer.name}");
            targetPlayer.Rpc_RequestJail();
            targetPlayer.Rpc_ShowCaughtUI();
        }
    }


    [Rpc(RpcSources.StateAuthority, RpcTargets.InputAuthority)]
    public void Rpc_ShowCaughtUI()
    {
        jailTimer = jailDuration;

        Debug.Log("[JailUITrigger] Showing Caught UI via RPC");
        jailUI.ShowCaughtUI(jailDuration, false);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.InputAuthority)]
    private void Rpc_ShowReleasedUI()
    {
        isJailed = false;
        controller.enabled = true;

        Debug.Log("[JailUITrigger] Showing Released UI via RPC");
        jailUI.ShowReleasedUI(false);

    }
    private void OnTeamIndexChangedRender()
    {
        UpdateTeam(TeamIndex);
    }

    private void OnTeamChanged()
    {
        TeamData team = TeamManager.Instance.GetTeamData(TeamIndex);

        if (team != null)
        {
            GetComponent<TeamIdentifier>().Team = team;
            GetComponent<Renderer>().material.color = team.teamColor;
        }
    }
    private void UpdateTeam(int index)
    {
        if (availableTeams == null || index < 0 || index >= availableTeams.Length)
        {
            Debug.LogWarning("Invalid team index or team list not set.");
            return;
        }

        CurrentTeam = availableTeams[index];

        // Assign to identifier so others can access team
        var identifier = GetComponent<TeamIdentifier>();
        if (identifier != null)
        {
            identifier.Team = CurrentTeam;
        }

        // Optional: Update visuals
        GetComponentInChildren<Renderer>().material.color = CurrentTeam.teamColor;

        Debug.Log($"[Team] Assigned team: {CurrentTeam.teamName} to player {Object.InputAuthority}");
    }

    private void TryAttack()
    {
        Debug.Log("Attacking");
        // Define attack radius and layer mask
        float attackRange = 2f;
        LayerMask hitMask = LayerMask.GetMask("Body");

        Collider[] hits = Physics.OverlapSphere(transform.position, attackRange, hitMask);

        foreach (var hit in hits)
        {
            if (hit.gameObject == this.gameObject) continue;

            var otherPlayer = hit.GetComponentInParent<NetworkPlayerCollider>();
            if (otherPlayer == null) continue;

            var myTeam = GetComponent<TeamIdentifier>()?.Team;
            var otherTeam = otherPlayer.GetComponentInParent<TeamIdentifier>()?.Team;

            if (myTeam != null && otherTeam != null && myTeam != otherTeam)
            {
                if (HasStateAuthority)
                {
                    Debug.Log($"[ATTACK] Sending jail request for {otherPlayer.name}");

                    var otherNetworkPlayer = otherPlayer.GetComponentInParent<NetworkPlayer>();

                    // Use our own player object (this) to send the request to the host
                    Rpc_RequestJailTarget(otherNetworkPlayer.Object.Id);

                }
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 2f);
    }
}





