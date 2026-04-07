using System;
using System.Collections;
using System.Net;
using Popup;
using PopupSystem;
using UnityEngine;

public class TransitionManager : MonoBehaviour
{
    public GameObject objLoading;
    public Coroutine crt;

    private void Start()
    {
        DisableTransition();
    }
    public void PlayTransition(Action action = null)
    {
        Popup.PopupSystem.Instance.CloseAllPopupsImmediately();
        PopupUtility.ForceClosePopupLiteMessage();
        objLoading.SetActive(true);
        // AudioManager.Instance.PlaySFX(AudioClipId.Transition);
        StartCoroutine(CheckTransitionEnd(action));
       //AdvertisementManager.Instance?.HideAudioIconAd();
    }

    private IEnumerator CheckTransitionEnd(Action action)
    {
        yield return new WaitForSeconds(1.0f);
        DisableTransition();
        // action?.Invoke();
        // AudioManager.Instance.StopEffect();
        action?.Invoke();
    }

    private void DisableTransition()
    {
        objLoading.SetActive(false);
    }

    public void PlayTransitionLoadLv(Action action = null)
    {
        Popup.PopupSystem.Instance.CloseAllPopupsImmediately();
        PopupUtility.ForceClosePopupLiteMessage();

        var homeScreen = GameManager.Instance.uiManager.GetScreen<HomeScreen>();
        if (!homeScreen.isShowing)
        {
            homeScreen.Active();
        }
        if (crt != null)
        {
            StopCoroutine(crt);
            crt = null;
        }
        DisableTransition();
        return;
    }
    IEnumerator LoadAssetLv(Action action)
    {
        yield return new WaitForSeconds(1.3f);
        DisableTransition();
        // AudioManager.Instance.StopEffect();
        action?.Invoke();
    }
}