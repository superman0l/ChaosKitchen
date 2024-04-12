using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetClearCounter : NetBaseCounter
{
    //[SerializeField] private KitchenObjectSO kitchenObjectSO;
    public override void Intersect(NetPlayerController player)
    {
        if (player.hasKitchenObject())//��ɫ������Ʒ
        {
            if (!hasKitchenObject())
            {
                player.getKitchenObject().setKitchenObjectParent(this);
            }
            else
            {
                //��̨�Ķ����ŵ���ɫ��������
                if (player.getKitchenObject().TryGetPlate(out NetPlateKitchenObject plateKitchenObject))
                {
                    if (plateKitchenObject.AddList(getKitchenObject().getKitchenObjectSO()))
                        getKitchenObject().DestroySelf();
                }
                //��̨������װ��ɫ����Ʒ
                else if (getKitchenObject().TryGetPlate(out plateKitchenObject))
                {
                    if (plateKitchenObject.AddList(player.getKitchenObject().getKitchenObjectSO()))
                        player.getKitchenObject().DestroySelf();
                }
            }
        }
        else
        {
            if (!hasKitchenObject()){}
            else//������������ ����ɫ
            {
                getKitchenObject().setKitchenObjectParent(player);
            }
        }
    }
}
