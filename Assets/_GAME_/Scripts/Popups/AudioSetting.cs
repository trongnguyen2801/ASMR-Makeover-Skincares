using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class AudioSetting : MonoBehaviour
{
    [SerializeField] Image panelState;
    [SerializeField] GameObject onText;
    [SerializeField] GameObject offText;
    [SerializeField] Image circleMoving;
    [SerializeField] List<Sprite> panelStates;

    public void SetState(bool state)
    {
        panelState.sprite = state ? panelStates[1] : panelStates[0];
        onText.gameObject.SetActive(state);
        offText.gameObject.SetActive(!state);
        Tween tween = circleMoving.transform.DOLocalMoveX(state ? 66.5f : -66.5f, 0.1f);
    }
}
