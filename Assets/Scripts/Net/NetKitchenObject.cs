using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetKitchenObject : NetworkBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    private INetKitchenObjectParent netKitchenObjectParent;

    public KitchenObjectSO getKitchenObjectSO()
    {
        return kitchenObjectSO;
    }

    public static NetKitchenObject SpawnNetKitchenObject(KitchenObjectSO kitchenObjectSO, INetKitchenObjectParent kitchenObjectParent)
    {
        Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);
        NetKitchenObject netKitchenObject = kitchenObjectTransform.GetComponent<NetKitchenObject>();
        netKitchenObject.setKitchenObjectParent(kitchenObjectParent);
        return netKitchenObject;
    }

    public void setKitchenObjectParent(INetKitchenObjectParent kitchenObjectParent)
    {
        if (Object.IsValid)
        {
            if (kitchenObjectParent.hasKitchenObject()) // 判断新父对象是否为空
            {
                Debug.LogError("kitchenObjectParent already has a KitchenObject!");
                return;
            }
            kitchenObjectParent.setKitchenObject(this);

            this.netKitchenObjectParent?.clearKitchenObject();//解除当前父对象
            this.netKitchenObjectParent = kitchenObjectParent;

            transform.parent = kitchenObjectParent.getSelf();
            transform.localPosition = kitchenObjectParent.getKitchenObjectPos().localPosition;
        }
    }

    public INetKitchenObjectParent getkitchenObjectParent()
    {
        return netKitchenObjectParent;
    }

    public void DestroySelf()
    {
        if (Object.HasStateAuthority)
        {
            getkitchenObjectParent().clearKitchenObject();
            Runner.Despawn(Object);
        }
    }

    public bool TryGetPlate(out NetPlateKitchenObject plateKitchenObject)
    {
        if (this is NetPlateKitchenObject)
        {
            plateKitchenObject = this as NetPlateKitchenObject;
            return true;
        }
        else
        {
            plateKitchenObject = null;
            return false;
        }
    }
}
