using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeElsa : BaseTool
{
    public Transform posCutting1, posCutting2;
    public GameObject cakePiece1, cakePiece2;
    public Transform posTargetCutting;
    public float thresholdDistance = 1.5f;
    private bool isCutting = false;

    protected override void OnMouseDown()
    {
        base.OnMouseDown();
        SetAllSprLayer();
    }
    public bool CheckCanCutting()
    {
        if (isCutting) return false;
        if (!posCutting1.gameObject.activeSelf && !posCutting2.gameObject.activeSelf) return false;

        if (posCutting1.gameObject.activeSelf == false)
        {
            posTargetCutting = posCutting2;
        }
        else if (posCutting2.gameObject.activeSelf == false)
        {
            posTargetCutting = posCutting1;
        } else if(posCutting1.gameObject.activeSelf && posCutting2.gameObject.activeSelf)
        {
            var dis1 = Mathf.Infinity;
            var dis2 = Mathf.Infinity;
            dis1 = Vector2.Distance(transform.position, posCutting1.position);
            dis2 = Vector2.Distance(transform.position, posCutting2.position);
            if (dis1 < dis2)
            {
                posTargetCutting = posCutting1;
            }
            else
            {
                posTargetCutting = posCutting2;
            }
        }

        return Vector2.Distance(transform.position, posTargetCutting.position) < 1.5f;
    }

    public void Cutting()
    {
        if (isCutting) return;
        isCutting = true;
        posTargetCutting.gameObject.SetActive(false);
        SetDefaultLayer();
        SetCanInterract(false);
        Move(transform.position, posTargetCutting.position, 0.2f, () =>
        {
            SetCanInterract(false);
            if (posTargetCutting == posCutting1)
            {
                // AudioManager.Instance.PlaySFX(AudioClipId.Cutting);
                ChangeAnim("cutting1");
                DOVirtual.DelayedCall(1.5f, () =>
                {
                    cakePiece1.transform.DOMoveX(cakePiece1.transform.position.x + 50f, 1.2f).SetEase(Ease.Linear).OnComplete(() =>
                    {
                        cakePiece1.gameObject.SetActive(false);

                    });
                    isCutting = false;
                    SetCanInterract(true);
                });
            }
            else if (posTargetCutting == posCutting2)
            {
                // AudioManager.Instance.PlaySFX(AudioClipId.Cutting);
                ChangeAnim("cutting2");
                DOVirtual.DelayedCall(1.5f, () =>
                {
                    cakePiece2.transform.DOMoveX(cakePiece2.transform.position.x - 50f, 1.2f).SetEase(Ease.Linear).OnComplete(() =>
                    {
                        cakePiece2.gameObject.SetActive(false);

                    });
                    isCutting = false;
                    SetCanInterract(true);
                });
            }
        });
    }

    public bool CheckCompletedCutting()
    {
        return !posCutting1.gameObject.activeSelf && !posCutting2.gameObject.activeSelf;
    }
}
