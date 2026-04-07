//#define CHEATING

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Popup;
using DG.Tweening;
using System.IO;
using System.Reflection;
using System;

public class PopupSetting : PopupBase
{
    [Header("Music")]
    public Sprite musicEnabledSprite;

    public Sprite musicDisabledSprite;

    public Button musicButton;

    [Header("SFX")]
    public Sprite sfxEnabledSprite;

    public Sprite sfxDisabledSprite;

    public Button sfxButton;

    [Header("Quit")]
    public Button quitButton;

    public Button replayButton;

    [Header("Cheat")]
    public GameObject cheatObject;

    public InputField inputField;
    public Button languageButton;

    private bool musicEnabled = false;

    private bool sfxEnabled = false;

    private bool replayEnabled = false;

    //public void ApplyCheating()
    //{
    //    string cheatingInput = inputField.text;

    //    if (cheatingInput.Equals("reset"))
    //    {
    //        PlayerData.current = null;
    //        PlayerData.current = new PlayerData();
    //        File.Delete(PlayerData.GetFilePath());
    //        return;
    //    }

    //    if (cheatingInput.Equals("vip"))
    //    {
    //        BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;
    //        ProductDefinition definition = new ProductDefinition("vip_hack", ProductType.Subscription);
    //        ProductMetadata metadata = new ProductMetadata("1$", "Senior Member", "Become Premium User", "USD", 1);
    //        string receipt = "test";
    //        Product product = (Product)Activator.CreateInstance(typeof(Product), flags, null, new object[] { definition, metadata, receipt }, null);
    //        PurchaseEventArgs args = (PurchaseEventArgs)Activator.CreateInstance(typeof(PurchaseEventArgs), flags, null, new object[] { product }, null);
    //        SeniorMemberManager.Instance.SubscriptionSuccess(args);
    //        return;
    //    }

    //    if (cheatingInput.Equals("vipg"))
    //    {
    //        PlayerData.current.nextFreeSeniorGiftTime = DateTimeUtility.GetNow().ToString();
    //        PlayerData.current.nextDiscountOutOfMoveTime = null;
    //        return;
    //    }

    //    var ips = cheatingInput.Split('_');

    //    if (ips.Length == 2 && ips[0].Length > 0 && ips[1].Length > 0)
    //    {
    //        string amountString = ips[1];
    //        string targetString = ips[0];

    //        int.TryParse(amountString, out int amount);

    //        var playerData = PlayerData.current;

    //        if (targetString.Equals("coin"))
    //        {
    //            playerData.cointCount = Mathf.Max(0, playerData.cointCount + amount);
    //            EventDispatcher<GlobalEventId>.Instance.NotifyEvent(GlobalEventId.CoinChange, playerData.cointCount);
    //        }
    //        else if (targetString.Equals("gem"))
    //        {
    //            playerData.gemCount = Mathf.Max(0, playerData.gemCount + amount);
    //            EventDispatcher<GlobalEventId>.Instance.NotifyEvent(GlobalEventId.GemChange, playerData.gemCount);
    //        }
    //        else if (targetString.Equals("level"))
    //        {
    //            playerData.match3Data.level = Mathf.Clamp(amount, 0, 800);
    //            MapData.main = new MapData(playerData.match3Data.level);
    //        }
    //        else if (targetString.Equals("room"))
    //        {
    //            PlayerData.current.homeDesignData.SetUnlockRoom(amount);
    //        }
    //    }
    //}

    public void OnEnable()
    {
        //cheatObject.SetActive(Hack.Get() != null);

        musicEnabled = PlayerPrefs.GetInt("music_enabled", 1) == 1 ? true : false;
        sfxEnabled = PlayerPrefs.GetInt("sfx_enabled", 1) == 1 ? true : false;

        if (musicEnabled)
        {
            musicButton.GetComponent<Image>().sprite = musicEnabledSprite;
        }
        else
        {
            musicButton.GetComponent<Image>().sprite = musicDisabledSprite;
        }

        if (sfxEnabled)
        {
            sfxButton.GetComponent<Image>().sprite = sfxEnabledSprite;
        }
        else
        {
            sfxButton.GetComponent<Image>().sprite = sfxDisabledSprite;
        }

        //if (LoadSceneUtility.CurrentSceneName.Equals(LoadSceneUtility.Match3SceneName))
        //{
        //    replayButton.gameObject.SetActive(true);
        //    quitButton.gameObject.SetActive(false);
        //    replayEnabled = true;
        //}
        //else
        //{
        //    replayButton.gameObject.SetActive(false);
        //    quitButton.gameObject.SetActive(true);
        //    replayEnabled = false;
        //}
    }

    public override void Show()
    {
        canClose = false;

        PopupAnimationUtility.AnimateScale(transform, Ease.OutBack, 0.25f, 1f, 0.25f, 0f);

        PopupAnimationUtility.AnimateScale(musicButton.transform, Ease.OutBack, 0.25f, 1f, 0.2f, 0.1f);
        PopupAnimationUtility.AnimateScale(sfxButton.transform, Ease.OutBack, 0.25f, 1f, 0.2f, 0.15f);
        //PopupAnimationUtility.AnimateScale(languageButton.transform, Ease.OutBack, 0.25f, 1f, 0.2f, 0.2f);
        PopupAnimationUtility.AnimateScale(replayEnabled ? replayButton.transform : quitButton.transform, Ease.OutBack, 0.25f, 1f, 0.2f, 0.2f).OnComplete(() => canClose = true);
    }

    public override void Close(bool forceDestroying = true)
    {
        PopupAnimationUtility.AnimateAlpha(GetComponent<CanvasGroup>(), Ease.Linear, 1f, 0f, 0.1f, 0f, false);
        PopupAnimationUtility.AnimateScale(transform, Ease.OutQuart, 1f, 0.8f, 0.1f, 0f)
            .OnComplete(() => TerminateInternal(forceDestroying));
    }

    public void ToggleMusic()
    {
        musicEnabled = !musicEnabled;

        if (musicEnabled)
        {
            musicButton.GetComponent<Image>().sprite = musicEnabledSprite;
        }
        else
        {
            musicButton.GetComponent<Image>().sprite = musicDisabledSprite;
        }

        PlayerPrefs.SetInt("music_enabled", musicEnabled == true ? 1 : 0);
    }

    public void ToggleSFX()
    {
        sfxEnabled = !sfxEnabled;

        if (sfxEnabled)
        {
            sfxButton.GetComponent<Image>().sprite = sfxEnabledSprite;
        }
        else
        {
            sfxButton.GetComponent<Image>().sprite = sfxDisabledSprite;
        }

        PlayerPrefs.SetInt("sfx_enabled", sfxEnabled == true ? 1 : 0);
    }

    public void Replay()
    {
        //PopupSystem.Instance.ShowPopup(PopupType.PopupGiveup, CurrentPopupBehaviour.Close, true, true);
    }

    public void LanguageButtonPress()
    {
        //PopupSystem.Instance.ShowPopup(PopupType.PopupLanguage, CurrentPopupBehaviour.KeepShowing, true, true);
    }

    public void SelectJewelPress()
    {
        //Popup.PopupSystem.Instance.ShowPopup(PopupType.PopupSelectJewel, Popup.CurrentPopupBehaviour.KeepShowing, true, true);
    }

    public void LikeFbButton()
    {
        //if (PlayerData.current.fbLiked)
        //{
        //    try
        //    {
        //        Application.OpenURL("fb://page/103778305311333");
        //    }
        //    catch (Exception e)
        //    {
        //        Application.OpenURL("https://www.facebook.com/gardendecor2021/?ref=like_fb_button_1st");
        //    }
        //    return;
        //}
        //Popup.PopupSystem.GetOpenBuilder().
        //   SetType(PopupType.PopupLikeFb).
        //   SetCurrentPopupBehaviour(Popup.CurrentPopupBehaviour.KeepShowing).
        //   Open();
    }

    public void subYoutubeButton()
    {
        //if (PlayerData.current.ytSub)
        //{
        //    Application.OpenURL("https://www.youtube.com/channel/UCEJkBF0qqKfAydzzwtVX0lQ");
        //    return;
        //}
        //Popup.PopupSystem.GetOpenBuilder().
        //   SetType(PopupType.PopupSubYoutube).
        //   SetCurrentPopupBehaviour(Popup.CurrentPopupBehaviour.KeepShowing).
        //   Open();
    }

    public void TermsOfUse()
    {
        Application.OpenURL("http://iriseve.homedesign.mobi/policy.html");
    }

    public void SupportButton()
    {
        //string email = "hangoclan0990@gmail.com";
        //string subject = MyEscapeURL("Question on this Awesome Game");
        //string body = MyEscapeURL("I have a question on this awesome game:\r\nPackage: " + Application.identifier + "\r\nVersion: " + Application.version + "\r\nLevel: " + PlayerData.current.match3Data.level + "\r\nRoom: " + PlayerData.current.homeDesignData.currentRoomId + "\r\nDeviceModel: " + SystemInfo.deviceModel + "\r\nDeviceName: " + SystemInfo.deviceName + "\r\nMy question is:\r\n");

        //Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);
    }

    string MyEscapeURL(string url)
    {
        return WWW.EscapeURL(url).Replace("+", "%20");
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
