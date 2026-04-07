using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveElsaCake : BaseTool
{
    public DoorStove doorStove;
    public SwitchOb switchOb;
    public SpriteRenderer sprSlider;
    public GameObject animSmokeOpen;
    public bool isCompleted; 
    public override void Init(LevelStateCtrl levelCtrl)
    {
        base.Init(levelCtrl);
        doorStove.Init(levelCtrl);
        switchOb.Init(levelCtrl);
        doorStove.SetCanUseWithCol(false);
    }
    public void Running( float dur = 5f, Action completed = null)
    {
        transform.DOShakePosition(dur, new Vector3(0.05f, 0.1f,0), 50, 90, false, true);
        sprSlider.transform.DOScaleX(1, dur).SetEase(Ease.Linear).OnComplete(() =>
        {
            // AudioManager.Instance.PlaySFX(AudioClipId.Ping);
            animSmokeOpen.SetActive(true);
            doorStove.Open();
            completed?.Invoke();
            isCompleted = true;
        });
        switchOb.Running(dur);
    }
}
