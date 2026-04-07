using DG.Tweening;
using System;
using UnityEngine;

public class ToolVanilla : ToolPour
{
    [SerializeField] protected GameObject jar;
    [SerializeField] protected Transform toolsParent;
    [SerializeField] protected SpriteRenderer textureInTool;

    public override void Init(LevelStateCtrl levelCtrl)
    {
        base.Init(levelCtrl);
        InitDefaultLayer();
    }

    protected override void OnMouseDown()
    {
        base.OnMouseDown();
        if (this == lvCtrl.curStepToolOb)
        {
            jar.transform.SetParent(toolsParent);
            jar.transform.localScale = Vector3.one;
        }
    }

    public override void HandlePour(Action action = null)
    {
        base.HandlePour(action);

        Vector2 currentSize = textureInTool.size;
        float targetHeight = 0f;
        float duration = 1.5f;

        DOTween.To(() => textureInTool.size,
                   x => textureInTool.size = x,
                   new Vector2(currentSize.x, targetHeight),
                   duration);

        // AudioManager.Instance.PlaySFX(audioClipID);

        textureInBowl.transform.DOScale(1f, 1.5f).SetEase(Ease.InOutQuad).OnComplete(() =>
        {
            checkMoveToPoint = false;
            ResetState();
            action?.Invoke();
        });
    }
    public override void MoveBack()
    {
        MoveWithSpeedCallback(transform, defaultPosition, () =>
        {
            SetDefaultLayer();
            jar.transform.SetParent(this.transform);
        });
    }
}
