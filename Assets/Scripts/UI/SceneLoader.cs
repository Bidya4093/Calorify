using System.Collections;
using System.Collections.Generic;
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

    IEnumerator LoadSceneAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nameOfScene);
        while (!asyncLoad.isDone)
        {
            Debug.Log(asyncLoad.progress);
            yield return null;
        }
    }
}
