using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CuttingCounter : BaseCounter, IHasProgress
{
    [SerializeField] private CuttingObjectSO[] cuttingObjectSOs;
    private int cuttingCount = 0;

    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public static EventHandler OnAnyCut; 

    //按下F交互切菜
    public override void IntersectAlternate(Player player)
    {
        if (hasKitchenObject())
        {
            CuttingObjectSO cuttingObjectSO = getCuttingObjectSO(getKitchenObject().getKitchenObjectSO());
            cuttingCount++;
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                progressNormalized = (float)cuttingCount / cuttingObjectSO.CuttingCount
            });
            OnAnyCut?.Invoke(this, EventArgs.Empty);
            if (cuttingCount == cuttingObjectSO.CuttingCount)
            {
                getKitchenObject().DestroySelf();
                KitchenObject.SpawnKitchenObject(cuttingObjectSO.output, this);
            }
        }
    }

    //跟clearcounter一样先放上物品
    public override void Intersect(Player player)
    {
        if (player.hasKitchenObject())//角色持有物品
        {
            //角色手上的东西可以切且柜台为空
            if ((getCuttingObjectSO(player.getKitchenObject().getKitchenObjectSO()) != null) && !hasKitchenObject())
            {
                cuttingCount = 0;
                player.getKitchenObject().setKitchenObjectParent(this);
            }
            else if(player.getKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
            {
                if (plateKitchenObject.AddList(getKitchenObject().getKitchenObjectSO()))
                    getKitchenObject().DestroySelf();
            }
        }
        else
        {
            if (!hasKitchenObject())
            {
            }
            else//柜子上有物体 给角色
            {
                cuttingCount = 0;
                getKitchenObject().setKitchenObjectParent(player);
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = 0
                });
            }
        }
    }

    private CuttingObjectSO getCuttingObjectSO(KitchenObjectSO kitchenObjectSOInput)
    {
        foreach (var cuttingObjectSO in cuttingObjectSOs)
        {
            if (cuttingObjectSO.input == kitchenObjectSOInput)
                return cuttingObjectSO;
        }
        return null;
    }

    new public static void ResetStaticData()
    {
        OnAnyCut = null;
    }
}
