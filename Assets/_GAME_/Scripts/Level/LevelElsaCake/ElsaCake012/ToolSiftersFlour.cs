using DG.Tweening;
using UnityEngine;

public class ToolSiftersFlour : ArrangeOb
{
    [SerializeField] protected GameObject flourInSifters, flourInBowl;

    protected override void OnMouseDown()
    {
        if (isArranged && LevelElsaCake.Instance.numOfClick > 0)
        {
            isDragging = false;
            col.enabled = false;
            // AudioManager.Instance.PlaySFX(AudioClipId.RayBot);
            return;
        }
        base.OnMouseDown();
    }

    protected override void OnMouseUp()
    {
        if (isArranged)
        {
            LevelElsaCake.Instance.numOfClick--;
            float targetScale = 1f - ((float)LevelElsaCake.Instance.numOfClick / 3f);
            this.transform.rotation = Quaternion.Euler(0, 0, 0);

            Sequence shakeSeq = DOTween.Sequence();

            shakeSeq.Append(this.transform.DORotate(new Vector3(0, 0, -20), 0.05f))
                    .Append(this.transform.DORotate(new Vector3(0, 0, 20), 0.1f))
                    .Append(this.transform.DORotate(new Vector3(0, 0, -3), 0.08f))
                    .Append(this.transform.DORotate(new Vector3(0, 0, 3), 0.06f))
                    .Append(this.transform.DORotate(Vector3.zero, 0.05f))
                    .SetEase(Ease.InOutSine)
                   .OnComplete(() =>
                   {
                       if (LevelElsaCake.Instance.numOfClick > 0)
                           col.enabled = true;
                       else
                           this.MoveBack(false);
                   });

            flourInSifters.transform.DOScale(Vector3.one * LevelElsaCake.Instance.numOfClick / 3, 0.2f).SetEase(Ease.OutBack);
            flourInBowl.transform.DOScale(Vector3.one * targetScale, 0.2f).SetEase(Ease.OutBack);
            return;
        }
        ;
        base.OnMouseUp();
    }

    protected override void OnMouseDrag()
    {
        if (isArranged) return;
        base.OnMouseDrag();
    }
}
