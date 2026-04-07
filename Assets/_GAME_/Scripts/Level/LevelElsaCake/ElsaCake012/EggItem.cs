using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggItem : ArrangeOb
{
    [SerializeField] protected Transform posMoveEgg;

    public override void Init(LevelStateCtrl levelCtrl)
    {
        base.Init(levelCtrl);
        posDefault.position = this.transform.position;
    }

    public override void MoveInMultiplePlaces(bool canPlacesManyTime = false, float timeMoving = 0.2f, Action action = null)
    {
        // AudioManager.Instance.PlaySFX(putInToSound);
        if (posCorrect == null) posCorrect = posCorrects[0];
        Vector3 pos = posCorrect.position;
        transform.position = new Vector3(transform.position.x, transform.position.y, posCorrect.position.z);
        if (posMoveEgg != null)
        {
            // AudioManager.Instance.PlaySFX(AudioClipId.DapVoTrung);
            LevelElsaCake.Instance.ToggleColEgg(false);
            col.enabled = false;
            transform.DOMove(posMoveEgg.position, timeMoving).SetEase(Ease.InOutQuad);
            var anim = this.GetComponent<Animator>();
            if (anim != null) anim.SetTrigger("Using");
        }
        else
            transform.DOMove(posCorrect.position, timeMoving).SetEase(Ease.InOutQuad);

        Vector3 scale = posCorrect.localScale;
        transform.localScale = scale;

        Quaternion ro = posCorrect.rotation;
        transform.localRotation = ro;

        if (!canPlacesManyTime)
        {
            col.enabled = false;
            isDragging = false;
        }

        isArranged = true;
        posCorrect.gameObject.SetActive(false);
        SetDefaultLayer();

        if (action != null) action?.Invoke();
    }

    public void HandleEndAnimEgg()
    {
        var anim = this.GetComponent<Animator>();
        if (anim != null) anim.SetTrigger("Def");
        col.enabled = false;
        this.gameObject.SetActive(false);
    }

    public void HandleAlbumen()
    {
        LevelElsaCake.Instance.HandleAlnumenInBowl();
    }

    public void HandleEggYolk()
    {
        LevelElsaCake.Instance.HandleEggYolkInBowl();
    }

    public void HandleBowl()
    {
        LevelElsaCake.Instance.HandleBowlEgg();
    }
}
