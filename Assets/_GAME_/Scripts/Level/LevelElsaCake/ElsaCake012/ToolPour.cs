using DG.Tweening;
using System;
using UnityEngine;

public class ToolPour : BaseTool
{
    public float limitDistance = 0.8f;
    // public AudioClipId audioClipID;
    public GameObject texturePour, textureInBowl, fxPartical;
    public Transform posMove, posCorrect;
    public bool checkMoveToPoint;

    public override void Init(LevelStateCtrl levelCtrl)
    {
        base.Init(levelCtrl);
        InitDefaultLayer();
    }

    public bool CanMoveToOb()
    {
        float minDistance = Vector2.Distance(transform.position, posCorrect.position);
        return minDistance < limitDistance;
    }

    public virtual void MoveToPoint(Action action = null)
    {
        if (col != null) col.enabled = false;
        SetAllSprLayer();
        Vector3 scale = posMove.localScale;
        transform.localScale = scale;

        Quaternion ro = posMove.rotation;
        transform.DORotate(ro.eulerAngles, 0.1f);

        Vector3 pos = posMove.position;
        transform.position = new Vector3(transform.position.x, transform.position.y, posMove.position.z);

        transform.DOKill(true);
        transform.DOMove(posMove.position, 0.5f).SetEase(Ease.InOutQuad)
            .OnComplete(() =>
            {
                if (fxPartical != null) fxPartical.SetActive(true);
                // AudioManager.Instance.PlaySFX(AudioClipId.Put);
                SetAllSprLayer();
                HandlePour(action);
            });
    }

    public virtual void HandlePour(Action action = null)
    {
        if (anim != null) anim.SetTrigger("Using");
        if (texturePour != null)
        {
            // AudioManager.Instance.PlaySoundVfxRepeated(2f, 1.5f, audioClipID);
            texturePour.transform.DOScaleY(1f, 0.5f).SetEase(Ease.InOutQuad)
                .OnComplete(() =>
                {
                    textureInBowl.transform.DOScale(1f, 1.5f).SetEase(Ease.InOutQuad).OnComplete(() =>
                    {
                        checkMoveToPoint = false;
                        ResetState();
                        action?.Invoke();
                    });
                });
        }
        ;
    }

    protected override void OnMouseDown()
    {
        if (GameManager.GameState != GameState.Playing) return;
        if (anim != null) anim.SetTrigger("Using");
        SetAllSprLayer();
        base.OnMouseDown();
        transform.localScale = pressScale;
        transform.rotation = Quaternion.Euler(pressRo);
    }

    protected override void OnMouseUp()
    {
        if (CanMoveToOb() && this == lvCtrl.curStepToolOb) return;
        if (anim != null) anim.SetTrigger("Def");
        transform.localScale = defaultScale;
        if (checkMoveToPoint) return;
        base.OnMouseUp();
    }

    protected virtual void ResetState()
    {
        if (checkMoveToPoint) return;
        transform.localScale = defaultScale;
        transform.DORotate(Vector3.zero, 0.1f).SetEase(Ease.InOutQuad);
        if (texturePour != null) texturePour?.SetActive(false);
        if (fxPartical != null) fxPartical.SetActive(false);
    }

    public override void MoveBack()
    {
        if (anim != null) anim.SetTrigger("Def");
        base .MoveBack();
    }

    public override void MoveWithSpeed(Transform posA, Transform posB, float speed = 0, bool canUseEnd = true, bool isActiveEnd = true)
    {
        gameObject.SetActive(true);
        base.isDragging = false;
        var col = this.GetComponent<Collider2D>();
        if (col != null) col.enabled = false;
        if (speed == default)
            speed = speedMove; // Default speed if not specified
        var duration = Vector3.Distance(posA.position, posB.position) / speed;
        transform.position = posA.position;
        transform.DOMove(posB.position, duration).SetEase(Ease.Linear).OnComplete(() =>
        {
            if (col != null) col.enabled = canUseEnd;
            gameObject.SetActive(isActiveEnd);
            SetDefaultLayer();
        });
    }
}
