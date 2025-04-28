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
    public NetworkButtons Buttons;
    public Vector2 direction;
}

