using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Popup;
using UnityEngine;
using UnityEngine.UI;

public class PopupSettings : PopupBase
{
    [SerializeField] Slider music;
    [SerializeField] Slider sound;
    [SerializeField] AudioSetting vibrate;

    public override void Show()
    {
        base.Show();
        canClose = false;
        Tween tween = PopupAnimationUtility
            .AnimateScale(transform, Ease.OutBack, 0.1f, 1, 0.5f, 0f, true).SetUpdate(true)
            .OnComplete(() => { });
    }

    public override void Close(bool forceDestroying = true)
    {
        float curScale = transform.localScale.x;
        Tween tween = PopupAnimationUtility
            .AnimateScale(transform, Ease.InBack, curScale + 0.05f, curScale - 0.2f, 0.1f, 0f, true).SetUpdate(true)
            .OnComplete(() => { base.Close(forceDestroying); });
        // AudioManager.Instance.PlaySFX(AudioClipId.ButtonClick);
        PreAnimateHideEvent?.Invoke();
        PostAnimateHideEvent?.Invoke();
    }

    void Start()
    {
        var data = PlayerData.current.gameSetting;
        sound.value = data.soundValue;
        music.value = data.musicValue;
        vibrate.SetState(data.vibrateState);
    }

    public void ButtonSound()
    {
        var data = PlayerData.current.gameSetting;
        // AudioManager.Instance.GetEffectValue(sound.value);
        data.soundValue = sound.value;
        Model.Instance.Save();
    }

    public void ButtonMusic()
    {
        var data = PlayerData.current.gameSetting;
        // AudioManager.Instance.GetMusicValue(music.value);
        data.musicValue = music.value;
        Model.Instance.Save();
    }

    public void ButtonVibrate()
    {
        // AudioManager.Instance.PlaySFX(AudioClipId.ButtonClick);
        var data = PlayerData.current.gameSetting;
        data.vibrateState = !data.vibrateState;
        vibrate.SetState(data.vibrateState);
        Model.Instance.Save();
    }

    public void OnClickClose()
    {
        canClose = true;
        CloseInternal();
    }

    public void BtnHome()
    {
        // AudioManager.Instance.PlaySFX(AudioClipId.ButtonClick);
        canClose = true;
        CloseInternal();
        GameManager.Instance.uiManager.ActiveScreen<HomeScreen>();
    }
    public void BtnReplay()
    {
        // AudioManager.Instance.PlaySFX(AudioClipId.ButtonClick);
        canClose = true;
        CloseInternal();
        GameManager.Instance.uiManager.ActiveScreen<GamePlayScreen>();
    }

    public void BtnSupport()
    {
        string email = "hangoclan0990@gmail.com";
        string subject = MyEscapeURL("Question on Find It Out - Hidden Objects");
        string body = MyEscapeURL("I have a question on this awesome game:\r\nPackage: " + Application.identifier +
                                  "\r\nVersion: " + Application.version + "\r\nMy question is:\r\n");
        Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);
    }

    string MyEscapeURL(string url)
    {
        return WWW.EscapeURL(url).Replace("+", "%20");
    }
}