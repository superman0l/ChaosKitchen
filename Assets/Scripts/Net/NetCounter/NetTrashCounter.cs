using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetTrashCounter : NetBaseCounter
{
    public override void Intersect(NetPlayerController player)
    {
        player.getKitchenObject().DestroySelf();
    }
}
