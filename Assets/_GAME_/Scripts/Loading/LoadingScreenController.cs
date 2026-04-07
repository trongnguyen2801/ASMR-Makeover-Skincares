using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreenController : MonoBehaviour
{
    public Image logo;
    [Header("GC")] public bool triggerGarbageCollector = true;

    private Vector3 curScale;

    private void Start()
    {
        curScale = logo.transform.localScale;
    }

    public void StartAnimating(string sceneName, Action PreLoadAction, Action PostLoadAction)
    {
        gameObject.SetActive(true);
        // AdvertisementManager.Instance.HideBannerAd();
        StartCoroutine(LoadingCoroutine(sceneName, PreLoadAction, PostLoadAction));
        logo?.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.4f)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Yoyo);
    }

    private IEnumerator LoadingCoroutine(string sceneName, Action PreloadAction, Action PostLoadAction)
    {
        // AudioManager.Play_SFX(AudioClipId.Loading);
        // AdvertisementManager.Instance.HideBannerAd();
        PreloadAction?.Invoke();
        yield return new WaitForSeconds(0.75f);
        var asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        while (asyncOperation != null && !asyncOperation.isDone)
        {
            yield return null;
        }

        logo?.transform.DOKill();
        DOTween.KillAll();
        if (triggerGarbageCollector)
        {
            GC.Collect();
#if UNITY_EDITOR
            Debug.LogFormat("Memory used after full collection:   {0:N0}", GC.GetTotalMemory(true));
#endif
        }

        yield return new WaitForSeconds(0.5f);
        // AdvertisementManager.Instance.ShowBannerAd();
        PostLoadAction?.Invoke();
        gameObject.SetActive(false);
        logo.transform.localScale = curScale;
    }
}