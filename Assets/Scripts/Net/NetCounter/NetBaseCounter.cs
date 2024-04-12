using Fusion;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NetBaseCounter : NetworkBehaviour, INetKitchenObjectParent
{
    [SerializeField] private Transform counterTopPoint;

    private SelectedCounterVisual selectedCounterVisual;
    public NetKitchenObject netKitchenObject;
    void Start()
    {
        selectedCounterVisual = transform.Find("Selected").GetComponent<SelectedCounterVisual>();
        selectedCounterVisual.hide();
    }

    public virtual void Intersect(NetPlayerController player)
    {
        Debug.Log("BaseCounter.Intersect()");
    }

    public virtual void IntersectAlternate(NetPlayerController player)
    {
        Debug.LogError("BaseCounter.IntersectAlternate()");
    }

    public void OnSelected()
    {
        selectedCounterVisual.show();
    }

    public void OnUnselected()
    {
        selectedCounterVisual.hide();
    }

    public Transform getKitchenObjectPos()
    {
        return counterTopPoint;
    }

    public void setKitchenObject(NetKitchenObject kitchenObject)
    {
        this.netKitchenObject = kitchenObject;
    }

    public NetKitchenObject getKitchenObject()
    {
        return netKitchenObject;
    }

    public void clearKitchenObject()
    {
        netKitchenObject = null;
    }

    public bool hasKitchenObject()
    {
        return netKitchenObject != null;
    }

    Transform INetKitchenObjectParent.getSelf()
    {
        return transform;
    }
}
