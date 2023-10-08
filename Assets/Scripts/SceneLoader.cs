using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }

    private AsyncOperation asyncLoad;
    private string loadTarget;

    public enum Scene
    {
        MainMenuScene,
        GameScene,
        LoadingScene
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    public void Load(Scene targetScene)
    {
        loadTarget = targetScene.ToString();
        StartCoroutine(AsLoad(loadTarget));
    }

    public IEnumerator AsLoad(string target)
    {
        yield return SceneManager.LoadSceneAsync(Scene.LoadingScene.ToString());

        asyncLoad = SceneManager.LoadSceneAsync(target);
        asyncLoad.allowSceneActivation = false;
        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }
    }

}
