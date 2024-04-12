using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using FusionHelpers;

public class DeliveryManagerSingleUI : MonoBehaviour
{
    [SerializeField] private Transform recipeTitle;
    [SerializeField] private Transform iconContainer;
    [SerializeField] private Transform iconTemplate;

    public void SetRecipeTitle(string newTitle)
    {
        recipeTitle.GetComponent<TextMeshProUGUI>().text = newTitle;
    }

    public void SetKitchenObjectUI(List<KitchenObjectSO> list)
    {
        foreach (KitchenObjectSO kitchenObjectSO in list)
        {
            IconTemplateUI iconTemplateUI = LocalObjectPool.Acquire(iconTemplate.GetComponent<IconTemplateUI>(), Vector3.zero, Quaternion.identity, iconContainer);
            // Transform addIconTemplate = Instantiate(iconTemplate, iconContainer);
            iconTemplateUI.SetIconUI(kitchenObjectSO);
        }
    }

    public void DestroySelf()
    {
        LocalObjectPool.Release(this);
    }
}
