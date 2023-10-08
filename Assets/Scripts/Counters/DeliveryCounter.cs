using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounter : BaseCounter
{
    //����soundmanager����counterλ�ã������Ͳ�ֻ̨�����һ�������ʹ�õ���
    public static DeliveryCounter Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public override void Intersect(Player player)
    {
        if (player.hasKitchenObject())//��ɫ������Ʒ
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
