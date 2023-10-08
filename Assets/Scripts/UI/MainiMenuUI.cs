using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainiMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;

    private void Awake()
    {
        playButton.onClick.AddListener(() =>
        {
            SceneLoader.Instance.Load(SceneLoader.Scene.GameScene);
        });

        quitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
        Time.timeScale = 1f;
    }
}
