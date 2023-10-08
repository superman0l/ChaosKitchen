using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconTemplateUI : MonoBehaviour
{
    [SerializeField] private Transform icon;

    public void SetIconUI(KitchenObjectSO kitchenObjectSO)
    {
        if (icon.TryGetComponent<Image>(out Image image))
            image.sprite = kitchenObjectSO.sprite;
    }
}
