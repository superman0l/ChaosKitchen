using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;


public enum PlayerInputButtons
{
    Intersect,
    Cut
}
public struct PlayerInput : INetworkInput
{
    public NetworkButtons buttons;
    public Vector2 moveDir;
}
