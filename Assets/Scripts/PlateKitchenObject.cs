using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlateKitchenObject : KitchenObject
{
    private List<KitchenObjectSO> kitchenObjectSOList;
    [SerializeField] private List<KitchenObjectSO> enableKitchenObjectSOList;

    public event EventHandler<OnAddedEventArgs> OnAdded;
    public class OnAddedEventArgs : EventArgs
    {
        public KitchenObjectSO kitchenObjectSO;
    }

    private void Awake()
    {
        kitchenObjectSOList = new List<KitchenObjectSO>();
    }

    public List<KitchenObjectSO> getKitchenObjectSOList()
    {
        return kitchenObjectSOList;
    }

    public bool AddList(KitchenObjectSO kitchenObjectSO)
    {
        foreach (var enableKitchenObjectSO in enableKitchenObjectSOList)
        {
            if (kitchenObjectSO == enableKitchenObjectSO)
            {
                foreach (var existedKitchenObjectSO in kitchenObjectSOList)
                {
                    if (kitchenObjectSO == existedKitchenObjectSO)
                        return false;
                }
                kitchenObjectSOList.Add(kitchenObjectSO);
                OnAdded?.Invoke(this, new OnAddedEventArgs
                {
                    kitchenObjectSO = kitchenObjectSO
                });
                return true;
            }
        }
        return false;
    }
}
