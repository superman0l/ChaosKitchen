using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounter : BaseCounter
{
    //用于soundmanager调用counter位置，并且送餐台只会存在一个，因此使用单例
    public static DeliveryCounter Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public override void Intersect(Player player)
    {
        if (player.hasKitchenObject())//角色持有物品
        {
            if (player.getKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
            {
                DeliveryManager.Instance.DeliverRecipe(plateKitchenObject);
                player.getKitchenObject().DestroySelf();
            }
        }
        else
        {
        }
    }
}
