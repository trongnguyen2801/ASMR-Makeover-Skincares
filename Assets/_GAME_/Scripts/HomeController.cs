using Popup;
using PopupSystem;
using UnityEngine;
using UnityEngine.UI;

namespace BB
{
    public class HomeController : MonoBehaviour
    {
        [SerializeField] private Text txtLevel;

        private void Start()
        {
            txtLevel.text = $"Level: {PlayerData.current.curLevelId}";
        }

        public void ShowInterAd()
        {
            // AdvertisementManager.Instance.ShowInterstitialAd();
        }

        public void ShowRewardedAd()
        {
        }

        public void ShowBanner()
        {
        }

        public void HideBanner()
        {
        }

        public void BtnToggleAds()
        {
            PlayerData.current.noAds = !PlayerData.current.noAds;
            PopupUtility.OpenPopupLiteMessage(PlayerData.current.noAds ? "Ads Disabled" : "Ads Enabled");
            Model.Instance.Save();
        }

        public void BtnShowSettings()
        {
            Popup.PopupSystem.GetOpenBuilder().SetType(PopupType.PopupSetting)
                .SetCurrentPopupBehaviour(CurrentPopupBehaviour.Close)
                .Open();
        }

        public void BtnShowPopupWin()
        {
            Popup.PopupSystem.Instance.ShowPopup(PopupType.PopupGameWin, CurrentPopupBehaviour.HideTemporary, true)
                .Show();
        }

        public void BtnShowPopupLose()
        {
            Popup.PopupSystem.Instance.ShowPopup(PopupType.PopupGameLose, CurrentPopupBehaviour.HideTemporary, true)
                .Show();
        }
    }
}