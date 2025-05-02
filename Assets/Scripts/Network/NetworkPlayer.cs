using Fusion;
using Fusion.Sockets;
using System;
using UnityEngine;
using TMPro;
using Cinemachine;

public class NetworkPlayer : NetworkBehaviour, IPlayerLeft
{
    [SerializeField] TextMeshProUGUI playerNameText;
    public static NetworkPlayer LocalPlayer { get; private set; }
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private CinemachineVirtualCamera cineMachinevirtualCamera;
    CinemachineBrain cinemachineBrain;
    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    [Networked, OnChangedRender(nameof(OnPlayerNameChanged))]
    [SerializeField] private NetworkString<_16> PlayerNickName { get; set; }

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
        if (!Object.HasInputAuthority)
        {
            Debug.Log("spawned remote Player.");
            return;
        }
        else
        {
            LocalPlayer = this;
            cinemachineBrain = FindObjectOfType<CinemachineBrain>();

            if (cineMachinevirtualCamera != null)
            {
                cineMachinevirtualCamera.Priority = 100;
                cineMachinevirtualCamera.Follow = transform;
                cineMachinevirtualCamera.LookAt = transform;
            }

            RPC_SetPlayerName(PlayerPrefs.GetString("PlayerName"));
            Debug.Log("spawned local Player.");
        }
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
        Vector3 inputDirection = Vector3.zero;

        if (GetInput(out NetworkInputData networkInputData))
        {
            // Handle horizontal movement
            inputDirection = networkInputData.movementInput.normalized;

            if (inputDirection.magnitude != 0f)
            {
                // Handle jumping
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
    public void RPC_SetPlayerName(string playerName, RpcInfo info = default)
    {
        PlayerNickName = playerName;
        Debug.Log($"[RPC] set nickname {PlayerNickName}");
    }
}
