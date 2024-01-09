using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string nameOfScene;
    public PlayableDirector director;
    
    void Start()
    {
        director.stopped += LoadSceneAsync;
    }

    void LoadSceneAsync(PlayableDirector aDirector)
    {
        if (director == aDirector) {
            StartCoroutine(LoadSceneAsync());
        }
    }

    static public IEnumerator LoadSceneAsync(string _nameOfScene)
    {
        if (SceneManager.GetSceneByName(_nameOfScene) == null)
        {
            yield return new Exception("Scene not found");
        }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(_nameOfScene);

        while (!asyncLoad.isDone)
        {
            Debug.Log(asyncLoad.progress);
            yield return null;
        }
    }

    IEnumerator LoadSceneAsync()
    {
        if (SceneManager.GetSceneByName(nameOfScene) == null)
        {
            yield return new Exception("Scene not found");
        }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nameOfScene);

        while (!asyncLoad.isDone)
        {
            Debug.Log(asyncLoad.progress);
            yield return null;
        }
    }
}
