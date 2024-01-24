using Firebase.Auth;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class Scenes : StringEnum
{
    public static List<Scenes> Values = new List<Scenes>();
    private Scenes(string value) : base(value) { Values.Add(this); }
    public static Scenes Auth { get { return new Scenes("Authorization"); } }
    public static Scenes Main { get { return new Scenes("MainScreen"); } }
    public static Scenes Loading { get { return new Scenes("LoadingScreen"); } }
}

public class SceneLoader : MonoBehaviour
{
    //public string scene;
    public PlayableDirector director;

    void Start()
    {
        director.stopped += LoadSceneAsync;
    }

    void LoadSceneAsync(PlayableDirector aDirector)
    {
        if (director == aDirector) {
            if (FirebaseAuth.DefaultInstance.CurrentUser != null)
            {
                LoadSceneAsyncWrapper(Scenes.Main);
                return;
            }
            LoadSceneAsyncWrapper(Scenes.Auth);
        }
    }

    void LoadSceneAsyncWrapper(Scenes scene)
    {
        StartCoroutine(LoadSceneAsync(scene));
    }

    static public IEnumerator LoadSceneAsync(Scenes sceneType)
    {
        string nameOfScene = sceneType.ToString();
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
