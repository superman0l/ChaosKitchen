using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    //[SerializeField] private Transform counterTopPoint;
    //[SerializeField] private KitchenObjectSO kitchenObjectSO;
    //private KitchenObject kitchenObject;

    public override void Intersect(Player player)
    {
        if (player.hasKitchenObject())//角色持有物品
        {
            if(!hasKitchenObject())
            {
                player.getKitchenObject().setKitchenObjectParent(this);
            }
            else 
            {
                //柜台的东西放到角色的盘子里
                if (player.getKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    if(plateKitchenObject.AddList(getKitchenObject().getKitchenObjectSO()))
                        getKitchenObject().DestroySelf();
                }
                //柜台的盘子装角色的物品
                else if(getKitchenObject().TryGetPlate(out plateKitchenObject))
                {
                    if(plateKitchenObject.AddList(player.getKitchenObject().getKitchenObjectSO()))
                        player.getKitchenObject().DestroySelf();
                }
            }
        }
        else
        {
            if(!hasKitchenObject())
            {
            }
            else//柜子上有物体 给角色
            {
                getKitchenObject().setKitchenObjectParent(player);
            }
        }
    }
}
