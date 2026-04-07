using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moild2ClickOb : ClickOb
{
    public Transform posAppear;
    public GameObject vfxStar;
    protected override void OnMouseDown()
    {
        // AudioManager.Instance.PlaySFX(pickUpSound);
        SetAllSprLayer();
        SetLocalPosZ(-7f);
        var pos = posAppear.position;
        pos.z = transform.position.z;
        transform.DOMove(pos, 1.5f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            // AudioManager.Instance.PlaySFX(AudioClipId.Blink);
            vfxStar.SetActive(true);
            isCompleted = true;
        });
        transform.DOScale(1f, 1.5f);
        SetCanUseWithCol(false);
    }
    protected override void OnMouseUp()
    {

    }
    protected override void OnMouseDrag()
    {

    }
}
