using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FusionHelpers;

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
            //Destroy(child.gameObject);
            child.GetComponent<DeliveryManagerSingleUI>().DestroySelf();
        }
        foreach (RecipeSO recipeSO in DeliveryManager.Instance.getManagerList())
        {
            DeliveryManagerSingleUI deliveryManagerSingleUI = LocalObjectPool.Acquire(recipeTemplate.GetComponent<DeliveryManagerSingleUI>(), Vector3.zero, Quaternion.identity, container);
            //Transform recipeTransform = Instantiate(recipeTemplate, container);
            deliveryManagerSingleUI.SetRecipeTitle(recipeSO.recipeName);
            deliveryManagerSingleUI.SetKitchenObjectUI(recipeSO.kitchenObjectSOList);
        }
    }

    public void UpdateRecipeUI(List<RecipeSO> recipeSOList)
    {
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }
        foreach (RecipeSO recipeSO in recipeSOList)
        {
            DeliveryManagerSingleUI deliveryManagerSingleUI = LocalObjectPool.Acquire(recipeTemplate.GetComponent<DeliveryManagerSingleUI>(), Vector3.zero, Quaternion.identity,  container);
            //Transform recipeTransform = Instantiate(recipeTemplate, container);
            deliveryManagerSingleUI.SetRecipeTitle(recipeSO.recipeName);
            deliveryManagerSingleUI.SetKitchenObjectUI(recipeSO.kitchenObjectSOList);
        }
    }
}
