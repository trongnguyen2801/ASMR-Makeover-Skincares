using DG.Tweening;
using System;
using UnityEngine;

public class ToolHandMixer : BaseTool
{
    public float limitDistance = 0.8f;
    public Transform posMove, posCorrect;
    public bool inBowl;
    public bool activeImgAll;
    public SpriteRenderer textureBlendFlour;
    protected AnimatorStateInfo stateInfo;

    public override void Init(LevelStateCtrl levelCtrl)
    {
        base.Init(levelCtrl);
        InitDefaultLayer();
        if (anim == null) return;
        stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.normalizedTime >= 1) return;
        anim.speed = 1;
        col.enabled = true;
    }

    public bool CanMoveToOb()
    {
        if (inBowl) return false;
        float minDistance = Vector2.Distance(transform.position, posCorrect.position);
        return minDistance < limitDistance;
    }

    public virtual void MoveToPoint(Action action = null)
    {
        //AudioManager.Instance.PlaySFX(AudioClipId.PutInSound);
        if (col != null) col.enabled = false;
        Vector3 scale = posMove.localScale;
        transform.localScale = scale;

        Quaternion ro = posMove.rotation;
        transform.DORotate(ro.eulerAngles, 0.1f);

        Vector3 pos = posMove.position;
        transform.position = new Vector3(transform.position.x, transform.position.y, posMove.position.z);

        transform.DOKill(true);
        transform.DOMove(posMove.position, 0.2f).SetEase(Ease.InOutQuad)
            .OnComplete(() =>
            {
                HandleActionMoveToPoint();
                action?.Invoke();
            });
    }

    protected virtual void HandleActionMoveToPoint()
    {
        pressScale = new Vector3(1, 1, 1);
        defaultScale = pressScale;
        anim.enabled = true;
        col.enabled = true;
        inBowl = true;

        Color c = textureBlendFlour.color;
        c.a = 0f;
        textureBlendFlour.color = c;
        textureBlendFlour.gameObject.SetActive(true);
    }

    protected override void OnMouseDown()
    {
        SetAllSprLayer();
        base.OnMouseDown();
        if (anim != null)
        {
            stateInfo = anim.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.normalizedTime < 1)
                anim.speed = 1;
        }
        // if (inBowl)
            // AudioManager.Instance.PlaySFX(AudioClipId.HandFlourMachine);
    }

    protected override void OnMouseDrag()
    {
        base.OnMouseDrag();
        if (anim == null) return;
        stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        float alpha = Mathf.Clamp01(stateInfo.normalizedTime);
        Color currentColor = textureBlendFlour.color;
        currentColor.a = alpha;
        textureBlendFlour.color = currentColor;
    }

    protected override void OnMouseUp()
    {
        if (anim != null)
        {
            AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.normalizedTime < 1)
                anim.speed = 0;
        }
        if (CanMoveToOb() && this == lvCtrl.curStepToolOb) return;
        base.OnMouseUp();
        BackDefaultState();
        // AudioManager.Instance.StopEffect();
    }

    public override void MoveBack()
    {
        base.MoveBack();
        if (activeImgAll)
        {
            foreach (var sp in liSprRend)
            {
                sp.gameObject.SetActive(true);
            }
        }
    }
}
