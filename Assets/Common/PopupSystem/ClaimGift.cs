using System;
using System.Collections;
using System.Collections.Generic;
using Popup;
using UnityEngine;

public class ClaimGift : MonoBehaviour
{
    // Action ClaimItem;
    // private void OnEnable()
    // {
    //     ActionController.OnClaimItem += ClaimGiftClick;
    // }

    // private void OnDisable()
    // {
    //     ActionController.OnClaimItem -= ClaimGiftClick;
    // }


    // private void ClaimGiftClick(List<ItemData> datas)
    // {
    //     int i = 0;
    //     ClaimItem = () =>
    //     {
    //         var popupClone = Popup.PopupSystem.GetOpenBuilder().SetType(PopupType.PopupClaim)
    //             .SetCurrentPopupBehaviour(CurrentPopupBehaviour.KeepShowing)
    //             .Open<PopupClaim>();
    //         popupClone.ShowReward(datas[i]);
    //         i++;
    //         if (i < datas.Count) popupClone.PostAnimateHideEvent = ClaimItem;
    //     };
    //     ClaimItem?.Invoke();

    // }

    // public void AddItem(List<ItemData> datas)
    // {
    //     for (int i = 0; i < datas.Count; i++)
    //     {
    //         switch (datas[i].type)
    //         {
    //             case ItemType.SkinBackGround:
    //                 UserData.CurrentData.ad(datas[i].value, "reward gift");
    //                 break;
    //             case ItemType.DiamondScrew:
    //                 UserData.CurrentData.AddDiamonScrew(datas[i].value, "reward gift");
    //                 break;
    //         }
    //     }
    // }
}
