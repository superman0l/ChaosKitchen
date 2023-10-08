using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKitchenObjectParent
{
    public Transform getKitchenObjectPos();

    public void setKitchenObject(KitchenObject kitchenObject);

    public KitchenObject getKitchenObject();

    public void clearKitchenObject();

    public bool hasKitchenObject();
}
