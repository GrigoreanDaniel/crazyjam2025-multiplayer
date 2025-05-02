using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;


public class CharacterInputHandler : MonoBehaviour
{
    public Vector2 moveInputVector;
    public NetworkBool isJumpPressed;

    // Update is called once per frame
    void Update()
    {
        moveInputVector.x = Input.GetAxis("Horizontal");
        moveInputVector.y = Input.GetAxis("Vertical");
    }

    public NetworkInputData GetNetworkInputData()
    {
        NetworkInputData networkInputData = new NetworkInputData();

        networkInputData.movementInput = moveInputVector;
        networkInputData.isJumpPressed = Input.GetButton("Jump");

        networkInputData.sonarPressed = Input.GetButton("Fire2"); //right click
        networkInputData.decoyPressed = Input.GetButton("Fire3"); //left shift

        return networkInputData;
    }
}
