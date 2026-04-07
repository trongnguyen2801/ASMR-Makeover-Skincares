using DG.Tweening;
using System;
using UnityEngine;

public class ArrangeActiveOb : ArrangeOb
{
    public GameObject activeOb;
    public Transform parent;

    public override void MoveInMultiplePlaces(bool canPlacesManyTime = false, float timeMoving = 0.25f, Action action = null)
    {
        // AudioManager.Instance.PlaySFX(putInToSound);
        if (posCorrect == null)
            posCorrect = posCorrects[0];
        Vector3 pos = posCorrect.position;
        transform.position = new Vector3(transform.position.x, transform.position.y, posCorrect.position.z);
        transform.DOMove(posCorrect.position, timeMoving).OnComplete(() =>
        {
            if (activeOb != null)
            {
                activeOb.SetActive(true);
            }
            gameObject.SetActive(false);
        });
        Vector3 scale = posCorrect.localScale;
        transform.localScale = scale;
        Quaternion ro = posCorrect.rotation;
        transform.localRotation = ro;

        if (parent != null)
            transform.parent = parent;
        if (!canPlacesManyTime)
        {
            col.enabled = false;
            isDragging = false;
        }
        isArranged = true;
        posCorrect.gameObject.SetActive(false);
    }

    public override bool CanAssembleMultiplePlaces()
    {
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
        return (minDistance < limitDistance);
    }
}
