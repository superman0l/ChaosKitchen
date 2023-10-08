using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DeliveryManagerUI : MonoBehaviour
{
    [SerializeField] private Transform recipeTemplate;
    [SerializeField] private Transform container;

    private void Start()
    {
        DeliveryManager.Instance.OnRecipeSpawned += UpdateRecipeUI;
        DeliveryManager.Instance.OnRecipeCompleted += UpdateRecipeUI;
    }

    //教学操作：清除recipe全部ui然后根据列表重新生成...ummm...
    private void UpdateRecipeUI(object sender, EventArgs e)
    {
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }
        foreach (RecipeSO recipeSO in DeliveryManager.Instance.getManagerList())
        {
            Transform recipeTransform = Instantiate(recipeTemplate, container);
            recipeTransform.GetComponent<DeliveryManagerSingleUI>().SetRecipeTitle(recipeSO.recipeName);
            recipeTransform.GetComponent<DeliveryManagerSingleUI>().SetKitchenObjectUI(recipeSO.kitchenObjectSOList);
        }
    }
}
