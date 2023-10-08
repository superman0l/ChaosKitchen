using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    public enum Scene
    {
        MainMenuScene,
        GameScene,
        LoadingScene
    }

    private static Scene targetScene;

    public static void Load(Scene targetScene)
    {
        Loader.targetScene = targetScene;

        SceneManager.LoadScene(Scene.LoadingScene.ToString());
    }

    public static IEnumerator LoaderCallback()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(targetScene.ToString());
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        SceneManager.LoadScene(targetScene.ToString());
    }
}
