using FusionHelpers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetDeliveryCounter : NetBaseCounter
{
    public override void Intersect(NetPlayerController player)
    {
        if (player.hasKitchenObject())//��ɫ������Ʒ
        {
            if (player.getKitchenObject().TryGetPlate(out NetPlateKitchenObject plateKitchenObject))
            {
                if (Runner.TryGetSingleton(out NetDeliveryManager deliveryManager))
                    deliveryManager.DeliverRecipe(plateKitchenObject);
                // DeliveryManager.Instance.DeliverRecipe(plateKitchenObject);
                player.getKitchenObject().DestroySelf();
            }
        }
        else
        {
        }
    }
}
