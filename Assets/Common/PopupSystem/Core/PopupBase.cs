using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace Popup
{
    public class PopupBase : MonoBehaviour
    {
        public CurrencyInfo info;

        public Action PostAnimateShowEvent;
        public Action PreAnimateHideEvent;
        public Action PostAnimateHideEvent;
        public Action OnCloseEvent;
        public Action<object> AcceptEvent;
        public Action<object> DenyEvent;
        public bool cancelable = true;

        protected Transform cachedTransform;
        protected CanvasGroup canvasGroup;
        protected bool canClose;
        protected internal PopupType type;

        protected void Awake()
        {
            cachedTransform = transform;
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public virtual bool CanClose()
        {
            return canClose && cancelable;
        }

        public virtual void Show()
        {
        }

        public virtual void Close(bool forceDestroying = true)
        {
            TerminateInternal(forceDestroying);
        }

        protected void TerminateInternal(bool forceDestroying = true)
        {
            if (forceDestroying)
                Destroy(gameObject);
            else
                gameObject.SetActive(false);

            PopupSystem.Instance.OnPopupTerminate();
            OnCloseEvent?.Invoke();
        }

        public void CloseInternal()
        {
            PopupSystem.Instance.ClosePopup();
        }

        private IEnumerator delayDo(YieldInstruction instruction, UnityAction unityAction)
        {
            yield return instruction;
            if (unityAction != null)
            {
                unityAction();
            }

            yield break;
        }

        private IEnumerator delayDo(CustomYieldInstruction instruction, UnityAction unityAction)
        {
            yield return instruction;
            if (unityAction != null)
            {
                unityAction();
            }

            yield break;
        }

        private IEnumerator loopDelayDo(Func<bool> func, UnityAction unityAction)
        {
            while (func())
            {
                yield return waitForEndOfFrame;
            }

            if (unityAction != null)
            {
                unityAction();
            }

            yield break;
        }

        protected IEnumerator StartUnityWeb(UnityWebRequest unityWebRequest, UnityAction<DownloadHandler> unityAction)
        {
            yield return unityWebRequest.SendWebRequest();
            if (unityAction != null)
            {
                unityAction(unityWebRequest.downloadHandler);
            }

            yield break;
        }

        protected IEnumerator StartUnityWeb(UnityWebRequest unityWebRequest, UnityAction<UnityWebRequest> unityAction)
        {
            yield return unityWebRequest.SendWebRequest();
            if (unityAction != null)
            {
                unityAction(unityWebRequest);
            }

            yield break;
        }

        protected void DelayDo(UnityAction unityAction)
        {
            if (base.gameObject.activeSelf)
            {
                base.StartCoroutine(this.delayDo(waitForEndOfFrame, unityAction));
            }
        }

        protected void LoopDelayDo(Func<bool> func, UnityAction unityAction)
        {
            if (base.gameObject.activeSelf)
            {
                base.StartCoroutine(this.loopDelayDo(func, unityAction));
            }
        }

        protected void DelayDo(YieldInstruction instruction, UnityAction unityAction)
        {
            if (base.gameObject.activeSelf)
            {
                base.StartCoroutine(this.delayDo(instruction, unityAction));
            }
        }

        protected void DelayDo(CustomYieldInstruction instruction, UnityAction unityAction)
        {
            if (base.gameObject.activeSelf)
            {
                base.StartCoroutine(this.delayDo(instruction, unityAction));
            }
        }

        private static WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
    }
}

[Serializable]
public class CurrencyInfo
{
    public bool showCurrency;
    public bool showSetting;
    public bool showMoreGem;
    public bool showMoreCoin;
}