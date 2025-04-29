using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Fusion;
using UnityEngine;

public class NetworkPlayer : NetworkBehaviour, IPlayerLeft
{
    public static NetworkPlayer LocalPlayer { get; private set; }

    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float rotationSpeed = 20f;
    //[SerializeField] private Transform cameraTransform;
    private CinemachineVirtualCamera cineMachinevirtualCamera;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

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
            cineMachinevirtualCamera = FindObjectOfType<CinemachineVirtualCamera>();

            if (cineMachinevirtualCamera != null)
            {
                cineMachinevirtualCamera.Follow = transform;
                cineMachinevirtualCamera.LookAt = transform;
            }
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
        if (Object.HasStateAuthority)
        {
            isGrounded = controller.isGrounded;

            if (isGrounded && velocity.y < 0)
            {

                velocity.y = -2f;
            }
        }

        if (GetInput(out NetworkInputData networkInputData))
        {
            // Handle horizontal movement

            Vector3 inputDirection = networkInputData.movementInput.normalized;

            if (inputDirection.magnitude >= 0.1f)
            {
                /*// Get camera forward/right projected to horizontal plane
                Vector3 camForward = Camera.main.transform.forward;
                Vector3 camRight = Camera.main.transform.right;

                camForward.y = 0f;
                camRight.y = 0f;
                camForward.Normalize();
                camRight.Normalize();*/

                Vector3 moveDirection = transform.forward * networkInputData.movementInput.y + transform.right * networkInputData.movementInput.x; ;
                moveDirection.Normalize();
                controller.Move(moveDirection * moveSpeed * Runner.DeltaTime);

                // Smoothly rotate toward movement
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Runner.DeltaTime * rotationSpeed);
                // Handle jumping
                if (networkInputData.isJumpPressed && isGrounded)
                {
                    velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                }

                // Apply gravity
                velocity.y += gravity * Runner.DeltaTime;
                controller.Move(new Vector3(0, velocity.y, 0) * Runner.DeltaTime);

            }
        }
    }
}
