using System;
using DG.Tweening;
using UnityEngine;

public class BaseTool : BaseOb
{
    [SerializeField] protected AudioClip soundToolRepeat;
    [SerializeField] protected float timeRepeatSound = 0.75f;
    [SerializeField] protected bool isVibration = false;
    [SerializeField] protected int vibraInten = 10;
    [SerializeField] protected float speedMove = 20f;
    protected float _timer = 0.75f;

    [HideInInspector] public int factorX = 1;

    protected Collider2D cachedCollider;

    protected override void OnMouseDown()
    {
        if (GameManager.GameState != GameState.Playing) return;
        if (lvCtrl == null) return;

        cachedTransform ??= transform;
        cachedCollider ??= col != null ? col : GetComponent<Collider2D>();

        isDragging = true;
        lvCtrl.curToolOb = this;
        if (this == lvCtrl.curStepToolOb)
        {
            lvCtrl.isUsingCorrectTool = true;
        }
        else
        {
            lvCtrl.NumOfFailTool++;
            //AudioManager.Instance.PlaySFX(AudioClipId.ToolWrong);
            GameManager.Instance.uiManager.GetScreen<GamePlayScreen>().ActiveIconWrong();
        }
        var cam = MainCamera;
        if (cam == null) return;

        offsetDistance = cam.ScreenToWorldPoint(Input.mousePosition) - cachedTransform.position;
        cachedTransform.localScale = pressScale;
        cachedTransform.rotation = Quaternion.Euler(pressRo);
        // AudioManager.Instance.PlaySFX(pickUpSound);
    }

    protected override void OnMouseUp()
    {
        if (GameManager.GameState != GameState.Playing) return;

        isDragging = false;
        if (lvCtrl == null) return;

        if (lvCtrl.isChooseToolLv && !lvCtrl.isUsingCorrectTool)
        {
            MoveBack();
        }

        lvCtrl.isUsingCorrectTool = false;
    }

    protected override void OnMouseDrag()
    {
        if (!isDragging) return;
        Camera cam = MainCamera;
        if (cam == null) return;
        if (GameManager.GameState != GameState.Playing) return;

        float heightBanner = 150f;
        Vector2 posTouch = Input.mousePosition;
        if (posTouch.y < heightBanner) posTouch.y = heightBanner;

        var worldPosition = cam.ScreenToWorldPoint(posTouch);
        var transformTool = cachedTransform != null ? cachedTransform : transform;
        var position = transformTool.position;
        worldPosition.z = position.z;
        position = worldPosition - (Vector3)offsetDistance;

        transformTool.position = position;
    }

    public virtual void MoveBack()
    {
        if (defaultPosition == null || lvCtrl == null) return;

        MoveWithSpeedCallback(transform, defaultPosition, () =>
        {
            SetDefaultLayer();
            lvCtrl.isUsingCorrectTool = false;
        });
    }

    public virtual void MoveWithSpeed(Transform posA, Transform posB, float speed = default, bool canUseEnd = true, bool isActiveEnd = true)
    {
        if (posA == null || posB == null) return;

        cachedTransform ??= transform;
        cachedCollider ??= col != null ? col : GetComponent<Collider2D>();
        gameObject.SetActive(true);
        base.isDragging = false;
        if (cachedCollider != null) cachedCollider.enabled = false;
        if (speed == default)
            speed = speedMove;
        var duration = Vector3.Distance(posA.position, posB.position) / speed;
        cachedTransform.position = posA.position;
        cachedTransform.DOMove(posB.position, duration).SetEase(Ease.Linear).OnComplete(() =>
        {
            if (cachedCollider != null) cachedCollider.enabled = canUseEnd;
            gameObject.SetActive(isActiveEnd);
        });
    }

    public virtual void MoveWithSpeedCallback(Transform posA, Transform posB, Action action = null, float speed = default, bool canUseEnd = true, bool isActiveEnd = true)
    {
        if (posA == null || posB == null) return;

        cachedTransform ??= transform;
        cachedCollider ??= col != null ? col : GetComponent<Collider2D>();
        base.isDragging = false;
        if (cachedCollider != null) cachedCollider.enabled = false;
        if (speed == default)
            speed = speedMove;
        var duration = Vector3.Distance(posA.position, posB.position) / speed;
        cachedTransform.position = posA.position;
        cachedTransform.DOMove(posB.position, duration).SetEase(Ease.Linear).OnComplete(() =>
        {
            if (cachedCollider != null) cachedCollider.enabled = canUseEnd;
            gameObject.SetActive(isActiveEnd);
            DOVirtual.DelayedCall(0, () => action?.Invoke());
        });
    }

    public override void Move(Vector3 s, Vector3 e, float time = 0.2f, Action action = null)
    {
        cachedTransform ??= transform;
        gameObject.SetActive(true);
        SetCanInterract(false);
        cachedTransform.position = s;
        cachedTransform.DOMove(e, time).SetEase(Ease.Linear).OnComplete(() =>
        {
            SetCanUseWithCol(true);
            gameObject.SetActive(true);
            DOVirtual.DelayedCall(0, () => action?.Invoke());
        });
    }

    public virtual void Move(Vector3 s, Vector3 e, float time = 0.2f, float delay = 0, Action action = null)
    {
        cachedTransform ??= transform;
        cachedCollider ??= col != null ? col : GetComponent<Collider2D>();
        gameObject.SetActive(true);
        base.isDragging = false;
        if (cachedCollider != null) cachedCollider.enabled = false;
        cachedTransform.position = s;
        cachedTransform.DOMove(e, time).SetEase(Ease.Linear).OnComplete(() =>
        {
            if (cachedCollider != null) cachedCollider.enabled = true;
            gameObject.SetActive(true);
            DOVirtual.DelayedCall(0, () => action?.Invoke());
        }).SetDelay(delay);
    }
    protected void CallVibration()
    {
        if (!isVibration) return;
      // CallVibrate(vibra);
    }

    //    private void CallVibrate(int t = 7)
    //    {
    //        if (PlayerData.current != null)
    //            if (!PlayerData.current.settingData.vibrateState) return;
    //#if UNITY_ANDROID
    //        try
    //        {
    //            Vibration.VibrateAndroid(t);
    //        }
    //        catch (Exception e)
    //        {
    //            Console.WriteLine(e);
    //        }
    //#elif UNITY_IPHONE || UNITY_IOS
    //        try
    //        {
    //            Vibration.VibratePeek();
    //        }
    //        catch (Exception e)
    //        {
    //            Console.WriteLine(e);
    //        }
    //#endif
    //    }

    protected virtual void Update()
    {
        if (GameManager.GameState != GameState.Playing)
        {
            isDragging = false;
            return;
        }

        if (!isDragging) return;
        if (lvCtrl == null) return;
        if (!lvCtrl.isUsingCorrectTool) return;
        CallVibration();
        _timer += Time.deltaTime;
        if (_timer < timeRepeatSound) return;

        // AudioManager.Instance.PlaySFX(AudioManager.Instance.effectsSourceTool, soundToolRepeat);
        _timer = 0;
    }

    private void OnEnable()
    {
        _timer = timeRepeatSound;
    }

    public virtual float GetPerCentInStep()
    {
        return 0;
    }
}
