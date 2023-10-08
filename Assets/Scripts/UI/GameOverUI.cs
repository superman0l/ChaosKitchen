using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GameOverUI : BaseUI
{
    [SerializeField] private TextMeshProUGUI RecipeDeliveredNumText;

    private void Start()
    {
        KitchenGameManager.Instance.OnGameStateChanged += UpdateGameOverUIState;
        Hide();
    }

    private void UpdateGameOverUIState(object sender, EventArgs e)
    {
        if (KitchenGameManager.Instance.isGameOver())
        {
            Show();
            RecipeDeliveredNumText.text = DeliveryManager.Instance.getRecipeDeliveredNum();
        }
        else Hide();
    }

}
