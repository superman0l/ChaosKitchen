using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class NetCuttingCounter : NetBaseCounter
{
    [SerializeField] private CuttingObjectSO[] cuttingObjectSOs;
    [SerializeField] private NetProgressBar progress;

    CuttingObjectSO cuttingObjectSO;
    private ChangeDetector _changes;
    [Networked] public int cuttingCount { get; private set; }
    [Networked] public int maxCutCount { get; private set; }

    public override void Spawned()
    {
         _changes = GetChangeDetector(ChangeDetector.Source.SimulationState);
    }

    public override void Render()
    {
        foreach (var change in _changes.DetectChanges(this, out var previousBuffer, out var currentBuffer))
        {
            switch (change)
            {
                case nameof(cuttingCount):
                    var reader = GetPropertyReader<int>(nameof(cuttingCount));
                    var (previous, current) = reader.Read(previousBuffer, currentBuffer);
                    float percent = (float)current / maxCutCount;
                    progress.SetBar(percent);
                    break;
            }
        }
    }

    public override void IntersectAlternate(NetPlayerController player)
    {
        if (hasKitchenObject())
        {
            cuttingObjectSO = getCuttingObjectSO(getKitchenObject().getKitchenObjectSO());
            maxCutCount = cuttingObjectSO.CuttingCount;
            cuttingCount++;
            //OnAnyCut?.Invoke(this, EventArgs.Empty);
            if (cuttingCount == cuttingObjectSO.CuttingCount)
            {
                getKitchenObject().DestroySelf();
                SpawnNetKitchenObject(cuttingObjectSO.output);
            }
        }
    }

    public void SpawnNetKitchenObject(KitchenObjectSO spawnObject)
    {
        NetKitchenObject netKitchenObject = spawnObject.prefab.GetComponent<NetKitchenObject>();
        Transform target = getKitchenObjectPos();
        var kitchenObjectTransform = Runner.Spawn(netKitchenObject,
                target.position,
                target.rotation,
                Object.InputAuthority);
        kitchenObjectTransform.setKitchenObjectParent(this);
    }

    //跟clearcounter一样先放上物品
    public override void Intersect(NetPlayerController player)
    {
        if (player.hasKitchenObject())//角色持有物品
        {
            //角色手上的东西可以切且柜台为空
            if ((getCuttingObjectSO(player.getKitchenObject().getKitchenObjectSO()) != null) && !hasKitchenObject())
            {
                cuttingCount = 0;
                player.getKitchenObject().setKitchenObjectParent(this);
            }
            else if (player.getKitchenObject().TryGetPlate(out NetPlateKitchenObject plateKitchenObject))
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

                progress.SetBar(0);
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
}
