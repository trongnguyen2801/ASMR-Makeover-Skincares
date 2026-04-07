using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Popup;
using UnityEngine;
using UnityEngine.UI;

public class PopupGameWin : PopupBase
{
    // [SerializeField] Transform blinks;
    [SerializeField] Transform ribonWin;
    [SerializeField] CanvasGroup btnContinue;
    [SerializeField] Transform victory;
    [SerializeField] Transform lightFx;
    [SerializeField] Text bounusText;
    [SerializeField] Transform bonus;
    Sequence sequence;
    // Coroutine effect;
    public override void Show()
    {
        var btn = btnContinue.GetComponent<Button>();
        btn.interactable = false;
        PopupAnimationUtility.AnimateScale(ribonWin, Ease.OutBack, 0.1f, 1.3f, 0.5f, 0f).OnComplete(() =>
        {
            // DOVirtual.DelayedCall(0.5f, () => AudioManager.Instance.PlaySFX(AudioClipId.Popup));
        });
        PopupAnimationUtility.AnimateScale(victory, Ease.OutBack, 0f, 1.3f, 0.5f, 0.3f);
        PopupAnimationUtility.AnimateScale(bonus, Ease.OutBack, 0f, 1, 0.5f, 1f);
        PopupAnimationUtility.AnimateAlpha(btnContinue, Ease.Linear, 0f, 1f, 1f, 2f).OnComplete(() =>
        {
            btn.interactable = true;
        });
        lightFx.DORotate(new Vector3(0, 045), 0.5f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(0, LoopType.Yoyo);
        base.Show();
        canClose = false;
        // if (blinks) effect = StartCoroutine(StarEffect(blinks));
        // AudioManager.Instance.PauseMusic();
        // AudioManager.Instance.PlaySFX(AudioClipId.GameWin);
        bounusText.text = "+50";

        //set infor lv
        LevelDataInfor levelDataInfor = PlayerData.current.gameData.GetLevelDataInforById(PlayerData.current.curLevelId);
        levelDataInfor.stateLv = StateLv.Completed;

        int idLevelUnlockNext = PlayerData.current.gameData.FindLevelToUnlock();
        if (idLevelUnlockNext > 0)
        {
            PlayerData.current.gameData.UnlockLevel(idLevelUnlockNext);
            Model.Instance.Save();
        }
    }

    // void OnDisable()
    // {
    //     StopCoroutine(effect);
    // }

    public override void Close(bool forceDestroying = true)
    {
        base.Close(forceDestroying);
        PreAnimateHideEvent?.Invoke();
        PostAnimateHideEvent?.Invoke();
        // AudioManager.Instance.musicSource.UnPause();
    }

    public void OnClickContinue()
    {
        btnContinue.gameObject.SetActive(false);
        PlayerData.current.AddCoin(50);
        bonus.gameObject.SetActive(false);
        /*ActionController.ClaimCoin(bonus.transform, () =>
        {
            AudioManager.Instance.StopEffect();
            canClose = true;
            CloseInternal();
        });*/
        canClose = true;
        CloseInternal();
    }

    public IEnumerator StarEffect(Transform parent)
    {
        foreach (Transform child in parent)
        {
            child.localScale = Vector3.zero;
        }
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform st = parent.GetChild(i);
            sequence = DOTween.Sequence();

            sequence.Append(st.DOScale(Vector3.one, 0.7f))
                .OnComplete(() =>
                {
                    st.DOScale(Vector3.zero, 0.7f);
                })
                .Append(st.GetComponent<Image>().DOFade(0, 0.5f))
                .AppendInterval(Random.Range(0f, 0.5f))
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutQuad)
                .SetAutoKill(true);
            yield return new WaitForSeconds(1f);
        }
    }

    public void BtnHome()
    {
        BtnClose();
        GameManager.Instance.uiManager.ActiveScreen<HomeScreen>();
    }
    public void BtnReplay()
    {
        BtnClose();
        GameManager.Instance.uiManager.ActiveScreen<GamePlayScreen>();
    }
    public void BtnNext()
    {
        BtnClose();
        GameManager.Instance.uiManager.ActiveScreen<HomeScreen>();
    }
    public void BtnClose()
    {
        canClose = true;
        CloseInternal();
    }
}
