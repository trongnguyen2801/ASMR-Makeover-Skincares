using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorStove : OpenCloseOb
{
    public override void Open()
    {
        base.Open();
        transform.DORotate(new Vector3(-150, 0, 0), 0.5f).SetEase(Ease.Linear);

    }
    public override void Close()
    {
        base.Close();
        transform.DORotate(new Vector3(0, 0, 0), 0.5f).SetEase(Ease.Linear);
        SetCanUseWithCol(false);
    }
}
