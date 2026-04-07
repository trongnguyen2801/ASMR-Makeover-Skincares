using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchOb : OpenCloseOb
{
    public override void Open()
    {
        base.Open();
        transform.localRotation = Quaternion.Euler(0, 0, -90);
    }
    public void Running(float dur = 5f)
    {
        
        transform.DORotate(new Vector3(0, 0, 0), dur).SetEase(Ease.Linear).OnComplete(() =>
        {
            isOpen = false;
        });
    }
}
