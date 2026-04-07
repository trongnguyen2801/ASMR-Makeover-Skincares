using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoilDropClickOb :ClickOb
{
    public override void Init(LevelStateCtrl levelCtrl)
    {
        base.Init(levelCtrl);
        anim.enabled = false;
    }
    protected override void OnMouseDown()
    {
        base.OnMouseDown();
        anim.enabled = true;
        isCompleted = true;
        DOVirtual.DelayedCall(1.25f, () =>
        {
            // AudioManager.Instance.PlaySFX(AudioClipId.ClickObj);
        });
    }
}
