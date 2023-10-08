using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    private IKitchenObjectParent kitchenObjectParent;

    public KitchenObjectSO getKitchenObjectSO()
    {
        return kitchenObjectSO;
    }

    public static KitchenObject SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent kitchenObjectParent)
    {
        Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);
        KitchenObject kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();
        kitchenObject.setKitchenObjectParent(kitchenObjectParent);
        return kitchenObject;
    }

    public void setKitchenObjectParent(IKitchenObjectParent kitchenObjectParent)
    {

        if (kitchenObjectParent.hasKitchenObject()) // 判断新父对象是否为空
        {
            Debug.LogError("kitchenObjectParent already has a KitchenObject!");
            return;
        }
        kitchenObjectParent.setKitchenObject(this);

        this.kitchenObjectParent?.clearKitchenObject();//解除当前父对象
        this.kitchenObjectParent = kitchenObjectParent;

        transform.parent = kitchenObjectParent.getKitchenObjectPos();
        transform.localPosition = Vector3.zero;
    }

    public IKitchenObjectParent getkitchenObjectParent()
    {
        return kitchenObjectParent;
    }

    public void DestroySelf()
    {
        getkitchenObjectParent().clearKitchenObject();
        Destroy(gameObject);
    }

    public bool TryGetPlate(out PlateKitchenObject plateKitchenObject)
    {
        if (this is PlateKitchenObject)
        {
            plateKitchenObject = this as PlateKitchenObject;
            return true;
        }
        else
        {
            plateKitchenObject = null;
            return false;
        }
    }
}
