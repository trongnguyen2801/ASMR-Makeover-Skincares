using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Popup;
using UnityEngine;
using UnityEngine.UI;

public class PopupGameLose : PopupBase
{
    [SerializeField] CanvasGroup ribonLose;
    [SerializeField] CanvasGroup dark;
    [SerializeField] CanvasGroup btnContinue;
    [SerializeField] Transform failedText;
    public override void Show()
    {
        var btn = btnContinue.GetComponent<Button>();
        btn.interactable = false;
        PopupAnimationUtility.AnimateAlpha(dark, Ease.Linear, 0f, 1f, 2f, 0.35f);
        PopupAnimationUtility.AnimateAlpha(ribonLose, Ease.Linear, 0f, 1f, 1f, 0.25f).OnComplete(() =>
        {
            failedText.DOLocalMoveY(207, 0.5f).SetEase(Ease.InOutElastic);
            // DOVirtual.DelayedCall(0.45f,()=> AudioManager.Instance.PlaySFX(AudioClipId.Fall));
        });
        PopupAnimationUtility.AnimateAlpha(btnContinue, Ease.Linear, 0f, 1f, 1f, 2f).OnComplete(() =>
        {
            btn.interactable = true;
        });
        base.Show();
        canClose = false;
        // AudioManager.Instance.PauseMusic();
        // AudioManager.Instance.PlaySFX(AudioClipId.GameFail);
    }

    public override void Close(bool forceDestroying = true)
    {
        base.Close(forceDestroying);
        PreAnimateHideEvent?.Invoke();
        PostAnimateHideEvent?.Invoke();
        // AudioManager.Instance.musicSource.UnPause();
    }

    public void OnClickReplay()
    {
        canClose = true;
        CloseInternal();
    }
}
