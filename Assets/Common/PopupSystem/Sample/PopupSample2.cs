using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Popup;
using UnityEngine;

public class PopupSample2 : PopupBase
{
    public override void Show()
    {
        canClose = false;
        PopupAnimationUtility.AnimateAlpha(canvasGroup, Ease.Flash, 0.25f, 1f, 0.25f, 0f)
            .OnComplete((() => canClose = true));
    }

    public override void Close(bool forceDestroying = true)
    {
        base.Close(forceDestroying);
    }
}
