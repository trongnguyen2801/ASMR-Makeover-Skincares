using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoopTool : CatchAndDropTool
{

    public BaseOb batterInBowl, batterInMoid;

    public override void Init(LevelStateCtrl levelCtrl)
    {
        base.Init(levelCtrl);
        batterInBowl.Init(levelCtrl);
        batterInMoid.Init(levelCtrl);
    }

    public override void Catch()
    {
        if(CheckCompleted())
        {
            return;
        }
        // AudioManager.Instance.PlaySFX(AudioClipId.Scoop);
        ChangeAnim("catch");
        switch (numOfCompletetd)
        {
            case 0:
                 batterInBowl.ChangeAnim("btCatch1");
                break;
            case 1:
                batterInBowl. ChangeAnim("btCatch2");
                break;
            case 2:
                batterInBowl.ChangeAnim("btCatch3");
                break;
        }
        isCatching = true;

    }

    public override void Drop()
    {
        if(CheckCompleted())
        {
            return;
        }   
        // AudioManager.Instance.PlaySFX(AudioClipId.Scoop);
        ChangeAnim("drop");
        switch (numOfCompletetd)
        {
            case 0:
                batterInMoid.ChangeAnim("btDrop1");
                break;
            case 1:
                batterInMoid.ChangeAnim("btDrop2");
                break;
            case 2:
                batterInMoid. ChangeAnim("btDrop3");
                break;
        }
        numOfCompletetd++;
        isCatching = false;
    }


    public override void Appear()
    {
        Vector3 pos = (Vector3) posAppear.position;
        pos.z = transform.position.z; // Keep the original z position
        if (posAppear != null)
        {
            transform.DOMove(pos, 1f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                
            });
        }
    }  
    //private void HandleSprBatterBeCatched()
    //{
    //    switch(numOfCompletetd)
    //    {
    //        case 1:
    //            sprBatterInBowl.transform.DOScale(new Vector3(0.9f, 0.9f, 0.9f), 0.5f).SetEase(Ease.OutBack);
    //            break;
    //        case 2:
    //            ssprBatterInBowl.transform.DOScale(new Vector3(0.9f, 0.9f, 0.9f), 0.5f).SetEase(Ease.OutBack);
    //            break;
    //        case 3:
    //            sprBatterInMoid.enabled = false;
    //            sprBatterInBowl.enabled = false;
    //            break;
    //    }
    //}
}
