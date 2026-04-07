using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ArrangeOb : BaseOb
{
    // [SerializeField] protected AudioClipId putInToSound;
    [SerializeField] protected float limitDistance = 0.4f;
    [HideInInspector] public Transform posCorrect;
    public Transform posDefault;
    public List<Transform> posCorrects;
    public bool isArranged;
    public int orderAssemble;

    public override void Init(LevelStateCtrl levelCtrl)
    {
        base.Init(levelCtrl);
    }
    
    public float GetLimitDistance() => limitDistance;
    public List<Transform> GetPosCorrects() => posCorrects;
    public Transform GetFirstTransPosCorrect => posCorrects[0];

    protected override void OnMouseDown()
    {
        base.OnMouseDown();
        if (lvCtrl == null) return;

        SetAllSprLayer();
        float minZ = lvCtrl.minZOb;
        if (!(transform.localPosition.z >= minZ)) return;
        Vector3 pos = transform.localPosition;
        minZ -= 0.01f;
        pos.z = minZ;
        transform.localPosition = pos;
        lvCtrl.minZOb = minZ;
    }

    protected override void OnMouseUp()
    {
        base.OnMouseUp();
        if (isArranged) isArranged = false;
    }

    public virtual bool CanAssembleMultiplePlaces()
    {
        if (posCorrects == null || posCorrects.Count == 0) return false;

        float minDistance = Vector2.Distance(transform.position, posCorrects[0].position);
        posCorrect = posCorrects[0];
        for (int i = 0; i < posCorrects.Count; i++)
        {
            if (!posCorrects[i].gameObject.activeSelf) continue;
            float curDistance = Vector2.Distance(transform.position, posCorrects[i].position);
            if (!(curDistance < minDistance)) continue;
            minDistance = curDistance;
            posCorrect = posCorrects[i];
        }
        Debug.Log($"CanAssembleMultiplePlaces: {minDistance} : {limitDistance}");
        return (minDistance < limitDistance) && posCorrect.gameObject.activeSelf;
    }


    public virtual void MoveInMultiplePlaces(bool canPlacesManyTime = false, float timeMoving = 0.2f, Action action = null)
    {
        // AudioManager.Instance.PlaySFX(putInToSound);
        if (!canPlacesManyTime)
        {
            if (col != null) col.enabled = false;
            isDragging = false;
        }
        if (posCorrect == null)
        {
            if (posCorrects == null || posCorrects.Count == 0) return;
            posCorrect = posCorrects[0];
        }

        transform.position = new Vector3(transform.position.x, transform.position.y, posCorrect.position.z);
        transform.DOMove(posCorrect.position, timeMoving);
        transform.localScale = posCorrect.localScale;
        transform.localRotation = posCorrect.rotation;

        isArranged = true;
        posCorrect.gameObject.SetActive(false);
        SetDefaultLayer();
        action?.Invoke();
    }

    public virtual void MoveBack(bool isMoving = true)
    {
        if (posDefault == null) return;
        if (col != null) col.enabled = false;

        transform.DOMove(posDefault.position, 0.5f).SetEase(Ease.InOutQuad)
            .OnComplete(() =>
            {
                if (col != null) col.enabled = isMoving;
                SetDefaultLayer();
            });
        BackDefaultState();
    }
    
    public void MoveBackDisabled()
    {
        if (posDefault == null) return;
        if (col != null) col.enabled = false;

        transform.DOMove(posDefault.position, 0.5f).SetEase(Ease.InOutQuad)
            .OnComplete(() =>
            {
                if (col != null) col.enabled = false;
                SetDefaultLayer();
            });
        BackDefaultState();
    }

    public override void BackDefaultState(Action action = null)
    {
        base.BackDefaultState(action);
    }
}
