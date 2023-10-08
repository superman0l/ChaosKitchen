using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
            Transform addIconTemplate = Instantiate(iconTemplate, iconContainer);
            addIconTemplate.GetComponent<IconTemplateUI>().SetIconUI(kitchenObjectSO);
        }
    }
}
