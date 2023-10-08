using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GameStartCountDown : BaseUI
{
    [SerializeField] private TextMeshProUGUI countdownText;

    private void Start()
    {
        KitchenGameManager.Instance.OnGameStateChanged += UpdateCountState;
        Hide();
    }
    private void UpdateCountState(object sender, EventArgs e)
    {
        if (KitchenGameManager.Instance.isCountdownToStart())
        {
            Show();
            
        }
        else Hide();
    }

    private void Update()
    {
        countdownText.text = KitchenGameManager.Instance.getCountdownTime();
    }
}
