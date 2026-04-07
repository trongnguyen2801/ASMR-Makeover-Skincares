using UnityEngine;

public class CatchAndDropTool : BaseTool
{
    public Transform posCatch, posDrop, posAppear;
    public int numOfCompletetd = 0;
    public int thresholdCompletetd = 3;
    public float  thresholdDis = 1f;
    protected bool isCatching = false;

    public virtual bool CheckCanCatch()
    {
        if (isCatching || posCatch == null) return false;
        return Vector2.Distance(transform.position, posCatch.position) < thresholdDis;
    }

    public virtual bool CheckCanDrop()
    {
        if (!isCatching || posDrop == null) return false;
        return Vector2.Distance(transform.position, posDrop.position) < thresholdDis;
    }

    public virtual bool CheckCompleted()
    {
        return numOfCompletetd >= thresholdCompletetd;
    }

    public virtual void Catch()
    {
        if (CheckCompleted()) return;

        // AudioManager.Instance.PlaySFX(AudioClipId.ButtonClick);
        isCatching = true;
    }

    public virtual void Drop()
    {
        if (CheckCompleted()) return;

        // AudioManager.Instance.PlaySFX(AudioClipId.ButtonClick);
        numOfCompletetd++;
        isCatching = false;
    }
}
