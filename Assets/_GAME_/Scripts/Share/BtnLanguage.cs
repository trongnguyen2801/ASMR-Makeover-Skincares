using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class BtnLanguage : MonoBehaviour
{
    [SerializeField] private LanguageType languageType;
    [SerializeField] private Button btnChangeLanguage;
    [SerializeField] private Text languageText;
    [SerializeField] private Image btn;
    [SerializeField] private Sprite stateChooseBtn;
    [SerializeField] private Sprite stateUnChooseBtn;

    public void Init(LanguageType type, Action<LanguageType, BtnLanguage> onChangeLanguage)
    {
        languageType = type;
        languageText.text = type.ToString();
        if (PlayerData.current.languageType == type)
            btn.sprite = stateChooseBtn;
        btnChangeLanguage.onClick.AddListener(() =>
        {
            onChangeLanguage(languageType, this);
            btn.sprite = stateChooseBtn;
            CustomLocalization.Load();
        });

        PopupAnimationUtility.AnimateScale(transform, Ease.OutBack, 0.25f, 1f, 0.5f, 0.15f).SetUpdate(true);
    }

    public void UnChoosingLanguage()
    {
        btn.sprite = stateUnChooseBtn;
    }
}
