using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class LoadSceneUtility
{
    public static string StartSceneName = "StartScene";
    public static string HomeSceneName = "HomeScene";
    public static string GameSceneName = "GameScene";

    public static void LoadScene(string sceneName, Action PreLoadAction = null, Action PostloadAction = null)
    {
        LoadSceneManager.Instance.LoadScene(sceneName, PreLoadAction, PostloadAction);
    }

    public static void ReloadCurrentScene()
    {
        LoadScene(SceneManager.GetActiveScene().name);
    }

    public static string CurrentSceneName => LoadSceneManager.Instance.currentSceneName;
}

public class LoadSceneManager : MonoBehaviour
{
    [NonSerialized] public string currentSceneName;
    public static LoadSceneManager Instance { get; private set; }
    public LoadingScreenController loadingScreenController;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        loadingScreenController.gameObject.SetActive(false);
    }

    public void LoadScene(string sceneName, Action PreloadAction = null, Action PostLoadAction = null)
    {
        Debug.Log("Loading scene " + sceneName);
        //if(sceneName.Equals(LoadSceneUtility.HomeDesignSceneName))
        //    FindObjectOfType<EventSystem>().
        DOTween.PauseAll();
        currentSceneName = sceneName;
        loadingScreenController.StartAnimating(sceneName, PreloadAction, PostLoadAction);
    }
}