using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour{

    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float rotationSpeed = 20f;

    [SerializeField] private Transform cameraTransform;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    private void Start(){

        controller = GetComponent<CharacterController>();

        // Lock and hide the cursor when game starts
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update(){

        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0){

            velocity.y = -2f;
        }

        // Handle horizontal movement
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 inputDirection = new Vector3(x, 0f, z).normalized;



        if (inputDirection.magnitude >= 0.1f){

            // Get camera forward/right projected to horizontal plane
            Vector3 camForward = Camera.main.transform.forward;
            Vector3 camRight = Camera.main.transform.right;

            camForward.y = 0f;
            camRight.y = 0f;
            camForward.Normalize();
            camRight.Normalize();

            Vector3 moveDirection = camForward * z + camRight * x;
            controller.Move(moveDirection * moveSpeed * Time.deltaTime);

            // Smoothly rotate toward movement
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }

        // Handle jumping
        if (Input.GetButtonDown("Jump") && isGrounded){

            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(new Vector3(0, velocity.y, 0) * Time.deltaTime);
    }
}
