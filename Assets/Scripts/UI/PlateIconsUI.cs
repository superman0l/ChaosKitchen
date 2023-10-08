using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlateIconsUI : MonoBehaviour
{
    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private GameObject iconTemplate;

    private void Awake()
    {
        plateKitchenObject.OnAdded += AddPlateIconUI;
    }

    private void AddPlateIconUI(object sender, PlateKitchenObject.OnAddedEventArgs e)
    {
        GameObject addedIconTemplate = iconTemplate;
        addedIconTemplate.GetComponent<IconTemplateUI>().SetIconUI(e.kitchenObjectSO);
        Instantiate(addedIconTemplate, transform);
    }
}
