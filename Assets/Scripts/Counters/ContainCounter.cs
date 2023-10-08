using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ContainCounter : BaseCounter
{

    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public event EventHandler OnPlayerGrabbedObject;

    public override void Intersect(Player player)
    {
        if (!player.hasKitchenObject())
        {
            Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab, player.getKitchenObjectPos());
            kitchenObjectTransform.GetComponent<KitchenObject>().setKitchenObjectParent(player);

            OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
        }
        else
        {
        }
    }
}
