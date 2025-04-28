using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class CharacterMovementHandler : NetworkBehaviour
{
    private CustomNetworkCharacterController characterController;
    // Start is called before the first frame update
    void Awake()
    {
        characterController = GetComponent<CustomNetworkCharacterController>();
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData networkInputData))
        {
            Vector3 moveDirection = transform.forward * networkInputData.movementInput.y + transform.right * networkInputData.movementInput.x;
            moveDirection.Normalize();

            characterController.Move(moveDirection);
            //* 5f * Runner.DeltaTime

        }
    }


}
