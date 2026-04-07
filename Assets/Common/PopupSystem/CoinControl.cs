using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Popup;
using UnityEngine;
using UnityEngine.UI;

public class CoinControl : MonoBehaviour
{
    [SerializeField] private int coinNo;
    [SerializeField] private Text coinText;
    [SerializeField] private GameObject pileOfCoinParent;
    [SerializeField] private Transform iconCoin;
    [SerializeField] private Image lightAcross;
    [SerializeField] Vector3 temp;
    [SerializeField] private Vector3[] initialPos;
    [SerializeField] private Quaternion[] initialRotation;


    private void Start()
    {
        GetcoinText();
    }

    public void GetcoinText()
    {
        coinText.text = Format.FormatCount(PlayerData.current.coinCount);
    }

    private void OnEnable()
    {
        temp = lightAcross.rectTransform.localPosition;

        Initial();
        ActionController.UpdateCoinText += UpdateCoin;
        ActionController.ClaimCoin += RewardPileOfCoin;
        ActionController.GetCurCoinText += GetcoinText;

        StartCoroutine(BtnlightAcross());
    }

    void OnDisable()
    {
        ActionController.UpdateCoinText -= UpdateCoin;
        ActionController.ClaimCoin -= RewardPileOfCoin;
        ActionController.GetCurCoinText -= GetcoinText;
    }

    private void UpdateCoin(int count)
    {
        int curCoin = PlayerData.current.coinCount;
        DOTween.To(() => curCoin, x => curCoin = x, curCoin + count, 1.2f).SetAutoKill(true)
        .SetDelay(1f)
        .SetEase(Ease.Linear)
        .OnUpdate(() =>
        {
            coinText.text = coinText.text = Format.FormatCount(curCoin);
        });
    }

    private void Initial()
    {
        initialPos = new Vector3[coinNo];
        initialRotation = new Quaternion[coinNo];

        for (int i = 0; i < pileOfCoinParent.transform.childCount; i++)
        {
            initialPos[i] = pileOfCoinParent.transform.GetChild(i).localPosition;
            initialRotation[i] = pileOfCoinParent.transform.GetChild(i).localRotation;
        }
    }

    private void ResetValue(Transform startPos)
    {
        pileOfCoinParent.transform.position = startPos.position;
        for (int i = 0; i < pileOfCoinParent.transform.childCount; i++)
        {
            pileOfCoinParent.transform.GetChild(i).localPosition = initialPos[i];
            pileOfCoinParent.transform.GetChild(i).localRotation = initialRotation[i];
        }
    }

    public void RewardPileOfCoin(Transform startPos, Action action)
    {
        StartCoroutine(RewardCoin(startPos, action));
    }

    public IEnumerator RewardCoin(Transform startPos, Action action = null)
    {
        bool canClose = false;
        int completedColumns = 0; // Biến đếm số cột đã hoàn thành
        ResetValue(startPos);
        var delay = 0f;
        pileOfCoinParent.SetActive(true);
        // AudioManager.Instance.PlaySFX(AudioClipId.CoinFly);
        for (int i = 0; i < pileOfCoinParent.transform.childCount; i++)
        {
            int index = i;
            pileOfCoinParent.transform.GetChild(i).DOScale(1.2f, 0.3f).SetDelay(delay + 0.3f).SetEase(Ease.OutBack).SetUpdate(true).SetAutoKill(true);
            pileOfCoinParent.transform.GetChild(i).DOMove(iconCoin.position, 0.8f).SetDelay(delay + 0.5f).SetEase(Ease.InBack).SetUpdate(true).SetAutoKill(true)
            .OnComplete(() =>
            {
                // AudioManager.Instance.PlaySFX(AudioClipId.AddCoin);
                pileOfCoinParent.transform.GetChild(index).DOScale(0f, 0.3f).SetUpdate(true).SetEase(Ease.OutBack).SetAutoKill(true);
                completedColumns++; // Tăng số lượng cột đã hoàn thành
                if (completedColumns == pileOfCoinParent.transform.childCount) // Kiểm tra nếu tất cả các cột đã hoàn thành
                {
                    canClose = true; // Đặt canClose thành true để kết thúc vòng lặp
                }
            });
            pileOfCoinParent.transform.GetChild(i).DORotate(Vector3.zero, 0.5f).SetDelay(delay + 0.5f).SetEase(Ease.Flash).SetUpdate(true).SetAutoKill(true);
            delay += 0.1f;
        }
        yield return new WaitUntil(() => canClose);
        action?.Invoke();
    }

    private IEnumerator BtnlightAcross()
    {
        yield return new WaitForSeconds(2.5f);
        lightAcross.rectTransform.localPosition = temp;
        Tween tween = lightAcross.rectTransform.DOLocalMoveX(lightAcross.rectTransform.localPosition.x + 200f, 0.3f).SetAutoKill(true).SetEase(Ease.Linear).OnComplete(() =>
        {
            if (lightAcross.gameObject.activeInHierarchy)
                StartCoroutine(BtnlightAcross());
        });
    }

}

