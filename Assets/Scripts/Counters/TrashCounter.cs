using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TrashCounter : BaseCounter
{
    public static EventHandler OnTrash;
    public override void Intersect(Player player)
    {
        player.getKitchenObject().DestroySelf();
        OnTrash?.Invoke(this, EventArgs.Empty);
    }
    new public static void ResetStaticData()
    {
        OnTrash = null;
    }
}
