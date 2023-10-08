using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : BaseUI
{
    public static GamePauseUI Instance { get; private set; }

    [SerializeField] private Button resumeButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button mainMenuButton;

    private void Awake()
    {
        resumeButton.onClick.AddListener(() =>
        {
            KitchenGameManager.Instance.TogglePauseGame();
        });
        mainMenuButton.onClick.AddListener(() =>
        {
            SceneLoader.Instance.Load(SceneLoader.Scene.MainMenuScene);
        });
        optionsButton.onClick.AddListener(() =>
        {
            OptionsUI.Instance.Show();
            //Hide();
        });
    }

    private void Start()
    {
        KitchenGameManager.Instance.GamePause += PauseUI;
        Hide();
    }

    private void PauseUI(object sender, KitchenGameManager.GamePauseEventArgs e)
    {
        if (e.isPaused) Show();
        else
        {
            Hide();
            OptionsUI.Instance.Hide();
        }
    }
}
