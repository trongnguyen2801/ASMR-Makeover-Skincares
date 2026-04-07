using DG.Tweening;
using System;
using UnityEngine;

public class ToolSpatula : ToolHandMixer
{
    [SerializeField] protected ToolSpatula toolSpatulaOther;
    [SerializeField] protected GameObject ob;

    public override void Init(LevelStateCtrl levelCtrl)
    {
        base.Init(levelCtrl);
        InitDefaultLayer();
        if (anim == null) return;
        stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.normalizedTime >= 1) return;
        anim.speed = 1;

        if (inBowl)
        {
            Color c = textureBlendFlour.color;
            c.a = 0f;
            textureBlendFlour.color = c;
            textureBlendFlour.gameObject.SetActive(true);
            pressScale = new Vector3(1, 1, 1);
        }
    }

    public override void MoveToPoint(Action action = null)
    {
        if (col != null) col.enabled = false;
        Vector3 scale = posMove.localScale;
        transform.localScale = scale;

        Quaternion ro = posMove.rotation;
        transform.DORotate(ro.eulerAngles, 0.1f);

        Vector3 pos = posMove.position;
        transform.position = new Vector3(transform.position.x, transform.position.y, posMove.position.z);

        transform.DOKill(true);
        transform.DOMove(posMove.position, 0.1f).SetEase(Ease.InOutQuad)
            .OnComplete(() =>
            {
                HandleActionMoveToPoint();
                action?.Invoke();
            });
    }

    protected override void HandleActionMoveToPoint()
    {
        anim.enabled = true;
        ChangeAnim("Scoop");
        pressScale = new Vector3(1, 1, 1);
    }

    protected override void OnMouseDrag()
    {
        if (inBowl)
        {
            stateInfo = anim.GetCurrentAnimatorStateInfo(0);
            float alpha = Mathf.Clamp01(stateInfo.normalizedTime);
            Color currentColor = textureBlendFlour.color;
            currentColor.a = alpha;
            textureBlendFlour.color = currentColor;
        }
        else
        {
            base.OnMouseDrag();
        }
    }

    protected override void OnMouseUp()
    {
        if (GameManager.GameState != GameState.Playing) return;
        if (CanMoveToOb() && this == lvCtrl.curStepToolOb) return;

        isDragging = false;

        try
        {
            if (lvCtrl.isChooseToolLv && lvCtrl.isUsingCorrectTool == false)
            {
                MoveBack();
            }
            lvCtrl.isUsingCorrectTool = false;
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }

        if (inBowl)
        {
            if (anim != null)
            {
                AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
                if (stateInfo.normalizedTime < 1)
                    anim.speed = 0;
            }
        }

        // AudioManager.Instance.StopEffect();
        BackDefaultState();
    }

    public void HandleScoopEnd()
    {
        BackDefaultState();
        toolSpatulaOther.gameObject.SetActive(true);
        var col = toolSpatulaOther.GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        this.gameObject.SetActive(false);
        toolSpatulaOther.InitDefaultLayer();
        toolSpatulaOther.SetAllSprLayer();
    }

    public override void MoveBack()
    {
        BackDefaultState();
        MoveWithSpeedCallback(transform, defaultPosition, () =>
        {
            SetDefaultLayer();
        });

    }

    public void ActiveGameObject()
    {
        if (ob != null) ob.SetActive(true);
    }
}
