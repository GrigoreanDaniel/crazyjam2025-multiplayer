using Fusion;
using UnityEngine;

public enum PlayerInput
{
    None,
    MoveForward,
    MoveBackward,
    MoveLeft,
    MoveRight,
    Jump,
    Shoot,
}

public struct NetworkInputData : INetworkInput
{
    public NetworkBool isJumpPressed;
    public Vector2 movementInput;
    public float rotationInput;

}

