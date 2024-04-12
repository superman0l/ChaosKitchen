using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlateKitchenObject;

public class NetPlateKitchenObject : NetKitchenObject
{
    [SerializeField]private List<KitchenObjectSO> kitchenObjectSOList = new List<KitchenObjectSO>();
    
    [SerializeField] private List<KitchenObjectSO> enableKitchenObjectSOList;
    [SerializeField] private PlateCompleteVisual plateCompleteVisual;
    [SerializeField] private KitchenObjectDic kitchenObjectDic;

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
                RPC_AddObject(kitchenObjectSO.id);
                kitchenObjectSOList.Add(kitchenObjectSO);

                // plateCompleteVisual.AddPlateKitchenObject(kitchenObjectSO);
                return true;
            }
        }
        return false;
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_AddObject(int id)
    {
        foreach (var kitchenObject in kitchenObjectDic.contents)
        {
            if(kitchenObject.id == id)
            {
                plateCompleteVisual.AddPlateKitchenObject(kitchenObject.kitchenObjectSO);
            }
        }
    }
}
