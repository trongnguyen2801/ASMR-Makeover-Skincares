using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MasksOb : MonoBehaviour
{
   // public List<GameObject> masks;//containing a spriteRenderer
    public List<MaskOb> newMasks;
    public int curIndexMaskOb;
    public Transform posAppear, posDisAppear;

    public void Move(Transform pos, float duration, float delay, Ease ease, bool isActiveEnd, Action callBack = null)
    {
        if (pos == null)
        {
            gameObject.SetActive(isActiveEnd);
            callBack?.Invoke();
            return;
        }

        gameObject.SetActive(true);
        Vector3 targetPos = pos.position;
        targetPos.z = transform.position.z;
        transform.DOMove(targetPos, duration).SetEase(ease).SetDelay(delay).OnComplete(() =>
        {
            callBack?.Invoke();
            gameObject.SetActive(isActiveEnd);
        });
    }
}
