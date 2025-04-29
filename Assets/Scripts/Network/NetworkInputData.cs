using Fusion;
using UnityEngine;


public struct NetworkInputData : INetworkInput
{
    public NetworkBool isJumpPressed;
    public Vector2 movementInput;
    public float rotationInput;

}

