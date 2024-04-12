using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetPlatesCounter : NetBaseCounter
{
    private float spawnPlateTimer;
    private float spawnPlateTimerMax = 4f;
    private int platesSpawnedAmount;
    private int platesSpawnedAmountMax = 4;

    [SerializeField] private PlatesCounterVisual platesCounterVisual;
    [SerializeField] private KitchenObjectSO plateKitchenObjectSO;

    private void Update()
    {
        if (platesSpawnedAmount <= platesSpawnedAmountMax) spawnPlateTimer += Time.deltaTime;
        if (spawnPlateTimer > spawnPlateTimerMax && platesSpawnedAmount <= platesSpawnedAmountMax)
        {
            spawnPlateTimer = 0;
            platesSpawnedAmount++;
            platesCounterVisual.SpawnPlate();
        }
    }

    public override void Intersect(NetPlayerController player)
    {
        if (player.hasKitchenObject())//角色持有物品
        {
            if (!hasKitchenObject())
            {

            }
            else
            {
            }
        }
        else
        {
            if (platesSpawnedAmount > 0)
            {
                platesSpawnedAmount--;

                SpawnNetKitchenObject(plateKitchenObjectSO, player);

                platesCounterVisual.DeletePlate();
            }
        }
    }

    public void SpawnNetKitchenObject(KitchenObjectSO spawnObject, NetPlayerController player)
    {
        NetKitchenObject netKitchenObject = spawnObject.prefab.GetComponent<NetKitchenObject>();
        Transform target = getKitchenObjectPos();
        var kitchenObjectTransform = Runner.Spawn(netKitchenObject,
                target.position,
                target.rotation,
                Object.InputAuthority);
        kitchenObjectTransform.setKitchenObjectParent(player);
    }
}
