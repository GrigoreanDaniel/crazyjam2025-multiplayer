using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;


public class CharacterInputHandler : MonoBehaviour
{
    public Vector2 moveInputVector;
    public NetworkBool isJumpPressed;
    public string identifier;
    private void Awake()
    {
        identifier = gameObject.GetComponent<AnimalIdentifier>().identifier;
    }
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

        if (identifier != null)
        {
            if (identifier == "Owl")
            {
                networkInputData.sonarPressed = Input.GetKeyDown(KeyCode.E);
            }
            else if (identifier == "Fox")
            {
                networkInputData.decoyPressed = Input.GetKeyDown(KeyCode.E);
            }
            else if (identifier == "Mole")
            {
                networkInputData.digPressed = Input.GetKeyDown(KeyCode.E);
            }
        }
        else
        {
            Debug.Log("No identifier found in player!");
        }
                       
         //right click
        

        networkInputData.isAttackPressed = Input.GetButton("Fire1"); //left click

        return networkInputData;
    }
}
