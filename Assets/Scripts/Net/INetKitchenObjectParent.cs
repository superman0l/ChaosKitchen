using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INetKitchenObjectParent
{
    public Transform getKitchenObjectPos();

    public Transform getSelf();

    public void setKitchenObject(NetKitchenObject kitchenObject);

    public NetKitchenObject getKitchenObject();

    public void clearKitchenObject();

    public bool hasKitchenObject();
}
