using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Popup;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayUICtrl : MonoBehaviour
{
    [SerializeField] Text levelText;
    private static GamePlayUICtrl instance;

    public static GamePlayUICtrl Instance
    {
        get { return instance; }
    }

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        // PopupSystem.Instance.InstallCoin();
    }


    public void OnClickSetting()
    {
        Popup.PopupSystem.GetOpenBuilder().SetType(PopupType.PopupSetting)
            .SetCurrentPopupBehaviour(CurrentPopupBehaviour.Close)
            .Open();
    }

    public void UpdateCoinText()
    {
        ActionController.GetCurCoinText();
    }
}