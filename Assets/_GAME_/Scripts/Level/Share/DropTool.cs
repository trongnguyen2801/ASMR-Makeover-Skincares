
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DropTool : BaseTool
{
    public Transform posCheckCanDrop;
    public Transform posDrop;
    public AudioClip sfxEnd;
    public bool isPouring;
    public float  threshSold = 1.5f;
    public float timePouring = 2.5f;
    public override void Init(LevelStateCtrl baseCustomLv)
    {
        base.Init(baseCustomLv);
        InitDefaultLayer();
    }
    protected override void OnMouseDown()
    {
        SetAllSprLayer();
        base.OnMouseDown();
    }
    protected override void OnMouseUp()
    {
        if (CanDrop() && this == lvCtrl.curStepToolOb) return; 
        base.OnMouseUp();
        BackDefaultState();
    }
    public override void MoveBack()
    {
        base.MoveBack();
    }
    public bool CanDrop()
    {
        return Vector2.Distance(transform.position, posCheckCanDrop.position) < threshSold;
    }

    public void Drop()
    {
        isPouring = true;
        if (posDrop == null)
        {
            posDrop = posCheckCanDrop;
        }
        transform.DOMove(posDrop.position, 0.1f).OnComplete(() => { ChangeAnim("drop"); });
        DOVirtual.DelayedCall(timePouring, () =>
        {
            isPouring = false;
            if(sfxEnd != null)
            {
                // AudioManager.Instance.PlaySFX(sfxEnd);
            }
        });
        //1.1s
    }

    protected override void Update()
    {
        if (isPouring)
        {
            if (GameManager.GameState != GameState.Playing)
            {
                return;
            }
            _timer += Time.deltaTime;
            if (_timer < timeRepeatSound) return;

            // AudioManager.Instance.PlaySFX(soundToolRepeat);
            _timer = 0;
        }
    }
}

