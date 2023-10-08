using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
    [SerializeField] private Transform counterTopPoint;

    private KitchenObject kitchenObject;
    public static EventHandler OnDropObject;//setobject放下物品

    public virtual void Intersect(Player player)
    {
        Debug.Log("BaseCounter.Intersect()");
    }

    public virtual void IntersectAlternate(Player player)
    {
        Debug.LogError("BaseCounter.IntersectAlternate()");
    }

    public Transform getKitchenObjectPos()
    {
        return counterTopPoint;
    }

    public void setKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
        OnDropObject?.Invoke(this, EventArgs.Empty);
    }

    public KitchenObject getKitchenObject()
    {
        return kitchenObject;
    }

    public void clearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool hasKitchenObject()
    {
        return kitchenObject != null;
    }

    public static void ResetStaticData()
    {
        OnDropObject = null;
    }
}
