using Popup;

namespace PopupSystem
{
    public static class PopupUtility
    {
        public static PopupLiteMessage popupLiteMessage;

        public static bool isAvailable => Popup.PopupSystem.Instance != null;

        public static void OpenPopupLiteMessage(string messageString)
        {
            if (!isAvailable) return;
            if (popupLiteMessage == null) popupLiteMessage = GetPopupLiteMessage();
            popupLiteMessage.SetText(messageString);
        }

        public static void ForceClosePopupLiteMessage()
        {
            if (popupLiteMessage == null) popupLiteMessage = GetPopupLiteMessage();

            popupLiteMessage.ForceClose();
        }

        private static PopupLiteMessage GetPopupLiteMessage()
        {
            return !isAvailable
                ? null
                : Popup.PopupSystem.Instance.addOnTransform.GetChild(0).GetComponent<PopupLiteMessage>();
        }

        public static void ShowPopupNoConnection()
        {
            Popup.PopupSystem.Instance.ShowPopup(PopupType.PopupNoConnection, CurrentPopupBehaviour.HideTemporary, true)
                .Show();
        }
    }
}