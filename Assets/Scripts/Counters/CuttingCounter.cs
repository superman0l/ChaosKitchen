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

    //����F�����в�
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

    //��clearcounterһ���ȷ�����Ʒ
    public override void Intersect(Player player)
    {
        if (player.hasKitchenObject())//��ɫ������Ʒ
        {
            //��ɫ���ϵĶ����������ҹ�̨Ϊ��
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
            else//������������ ����ɫ
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
