using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetContainCounter : NetBaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public event EventHandler OnPlayerGrabbedObject;

    public override void Intersect(NetPlayerController player)
    {
        if (Object.IsValid)
        {
            if (!player.hasKitchenObject())
            {
                NetKitchenObject netKitchenObject = kitchenObjectSO.prefab.GetComponent<NetKitchenObject>();
                Transform target = player.getKitchenObjectPos();
                var kitchenObjectTransform = Runner.Spawn(netKitchenObject,
                        target.position,
                        target.rotation,
                        Object.InputAuthority);
                //Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab, player.getKitchenObjectPos());
                Debug.LogWarning(player);
                kitchenObjectTransform.setKitchenObjectParent(player);

                // OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
            }
            else
            {
            }
        }
    }
}
