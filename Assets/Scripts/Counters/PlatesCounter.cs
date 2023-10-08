using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlatesCounter : BaseCounter
{
    private float spawnPlateTimer;
    private float spawnPlateTimerMax = 4f;
    private int platesSpawnedAmount;
    private int platesSpawnedAmountMax = 4;

    public event EventHandler PlateSpawn;
    public event EventHandler PlateRemove;

    [SerializeField] private KitchenObjectSO plateKitchenObjectSO;

    private void Update()
    {
        if (platesSpawnedAmount <= platesSpawnedAmountMax) spawnPlateTimer += Time.deltaTime;
        if(spawnPlateTimer > spawnPlateTimerMax && platesSpawnedAmount <= platesSpawnedAmountMax)
        {
            spawnPlateTimer = 0;
            platesSpawnedAmount++;
            PlateSpawn?.Invoke(this, EventArgs.Empty);
        }
    }

    public override void Intersect(Player player)
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

                KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);

                PlateRemove?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
