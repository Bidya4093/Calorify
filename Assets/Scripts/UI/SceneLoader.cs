using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;


public class Scenes
{
    public static readonly string Auth = "Authorization";
    public static readonly string Main = "MainScreen";
    public static readonly string Loading = "LoadingScreen";
}

public class SceneLoader : MonoBehaviour
{
    public SceneAsset scene;
    public PlayableDirector director;

    void Start()
    {
        director.stopped += LoadSceneAsync;
    }

    void LoadSceneAsync(PlayableDirector aDirector)
    {
        if (director == aDirector) {
            LoadSceneAsync(scene);
        }
    }

    void LoadSceneAsync(SceneAsset sceneAsset)
    {
        StartCoroutine(LoadSceneAsync(sceneAsset.name));
    }

    static public IEnumerator LoadSceneAsync(string nameOfScene)
    {
        if (SceneManager.GetSceneByName(nameOfScene) == null)
        {
            yield return new Exception("Scene not found");
        }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nameOfScene);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
