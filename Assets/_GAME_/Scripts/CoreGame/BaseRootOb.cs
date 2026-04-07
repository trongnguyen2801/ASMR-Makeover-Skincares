using System.Collections.Generic;
using UnityEngine;

public class BaseRootOb : MonoBehaviour
{
    public List<SpriteRenderer> liSprRend;
    public List<string> liSprRendDefaultLayer;
    [SerializeField] protected string sortingLayerPress = "HighestLayer";
    [SerializeField] private List<int> liSprRendDefaultOrderLayer;
    [SerializeField] protected Vector2 _offSetDrag;
    [SerializeField] protected Animator anim;
    protected string _animName;

    public virtual void InitDefaultLayer()
    {
        if (liSprRend == null || liSprRend.Count == 0) return;

        if (liSprRendDefaultLayer == null)
        {
            liSprRendDefaultLayer = new List<string>(liSprRend.Count);
        }
        else
        {
            liSprRendDefaultLayer.Clear();
        }

        for (var index = 0; index < liSprRend.Count; index++)
        {
            var spr = liSprRend[index];
            liSprRendDefaultLayer.Add(spr.sortingLayerName);
        }
    }

    public virtual void InitDefaultOrderInLayer()
    {
        if (liSprRend == null || liSprRend.Count == 0) return;

        if (liSprRendDefaultOrderLayer == null)
        {
            liSprRendDefaultOrderLayer = new List<int>(liSprRend.Count);
        }
        else
        {
            liSprRendDefaultOrderLayer.Clear();
        }

        for (var index = 0; index < liSprRend.Count; index++)
        {
            var spr = liSprRend[index];
            liSprRendDefaultOrderLayer.Add(spr.sortingOrder);
        }
    }

    public virtual void SetAllSprLayer()
    {
        if (liSprRend == null || liSprRend.Count == 0) return;

        foreach (var spr in liSprRend)
        {
            spr.sortingLayerName = sortingLayerPress;
        }
    }

    public virtual void SetAllSprLayer(string layerName)
    {
        if (liSprRend == null || liSprRend.Count == 0) return;

        foreach (var spr in liSprRend)
        {
            spr.sortingLayerName = layerName;
        }
    }

    public virtual void SetDefaultLayer()
    {
        if (liSprRend == null || liSprRend.Count == 0 || liSprRendDefaultLayer == null) return;
        if (liSprRendDefaultLayer.Count != liSprRend.Count) return;

        for (int i = 0; i < liSprRend.Count; i++)
        {
            liSprRend[i].sortingLayerName = liSprRendDefaultLayer[i];
        }
    }

    public virtual void SetMaskInteraction(SpriteMaskInteraction maskInteraction)
    {
        if (liSprRend == null || liSprRend.Count == 0) return;

        foreach (var spr in liSprRend)
        {
            spr.maskInteraction = maskInteraction;
        }
    }

    public virtual void SetOrderInLayer(List<int> liOrderSorting)
    {
        if (liSprRend == null || liOrderSorting == null) return;
        if (liSprRend.Count != liOrderSorting.Count) return;

        for (var index = 0; index < liSprRend.Count; index++)
        {
            var spr = liSprRend[index];
            spr.sortingOrder = liOrderSorting[index];
        }
    }

    public virtual void SetPosZ(float z)
    {
        var pos = transform.position;
        pos.z = z;
        transform.position = pos;
    }

    public virtual void SetLocalPosZ(float z)
    {
        var pos = transform.localPosition;
        pos.z = z;
        transform.localPosition = pos;
    }

    public virtual void BackToDefaulState()
    {
    }

    public void ChangeAnim(string animationName)
    {
        if (_animName == animationName || anim == null) return;
        if (string.IsNullOrEmpty(animationName)) return;

        if (!string.IsNullOrEmpty(_animName))
        {
            anim.ResetTrigger(_animName);
        }

        _animName = animationName;
        anim.SetTrigger(_animName);
    }
}
