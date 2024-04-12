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
        if (player.hasKitchenObject())//��ɫ������Ʒ
        {
            if(!hasKitchenObject())
            {
                player.getKitchenObject().setKitchenObjectParent(this);
            }
            else 
            {
                //��̨�Ķ����ŵ���ɫ��������
                if (player.getKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    if(plateKitchenObject.AddList(getKitchenObject().getKitchenObjectSO()))
                        getKitchenObject().DestroySelf();
                }
                //��̨������װ��ɫ����Ʒ
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
            else//������������ ����ɫ
            {
                getKitchenObject().setKitchenObjectParent(player);
            }
        }
    }
}
