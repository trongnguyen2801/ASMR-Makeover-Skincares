using DG.Tweening;
using System;
using UnityEngine;

public class BaseOb : BaseRootOb
{
    private static Camera _cachedMainCamera;

    public LevelStateCtrl lvCtrl;
    [SerializeField] protected Collider2D col;
    [SerializeField] protected Rigidbody2D rig;
    // [SerializeField] protected AudioClipId pickUpSound;
    // [SerializeField] protected AudioClipId putDownSound;
    [SerializeField] protected Transform defaultPosition, startPos;
    [SerializeField] protected Vector3 defaultScale = new(1f, 1f, 1f);
    [SerializeField] protected Vector3 pressScale = new(1.2f, 1.2f, 1f);
    [SerializeField] protected Vector3 pressRo = new(0f, 0f, 0f);
    protected Quaternion defaultRo;
    protected Transform cachedTransform;

    protected Vector2 lastPos, curPos;
    protected Vector2 offsetDistance;
    [HideInInspector] public bool isDragging;

    protected int IdOb;

    public virtual void Init(LevelStateCtrl levelCtrl)
    {
        lvCtrl = levelCtrl;
        cachedTransform = transform;
        col ??= GetComponent<Collider2D>();
        rig ??= GetComponent<Rigidbody2D>();
        anim ??= GetComponent<Animator>();

        defaultScale = cachedTransform.localScale;
        defaultRo = cachedTransform.localRotation;
        cachedTransform.localScale = defaultScale;
        InitDefaultLayer();
    }

    public virtual void Appear()
    {
    }

    public virtual void DisAppear()
    {
    }

    public virtual void SetCanUseWithCol(bool canUse)
    {
        if (col != null) col.enabled = canUse;
    }

    public virtual void SetCanDrag(bool canDrag)
    {
        isDragging = canDrag;
    }

    public virtual void SetCanInterract(bool canInterract)
    {
        if (col != null) col.enabled = canInterract;
        isDragging = canInterract;
    }

    protected Transform CachedTransform => cachedTransform != null ? cachedTransform : transform;

    protected Camera MainCamera
    {
        get
        {
            if (_cachedMainCamera == null || !_cachedMainCamera.isActiveAndEnabled)
            {
                _cachedMainCamera = Camera.main;
            }

            return _cachedMainCamera;
        }
    }

    protected virtual void OnMouseDown()
    {
        if (GameManager.GameState != GameState.Playing) return;

        var cam = MainCamera;
        if (cam == null) return;

        // AudioManager.Instance.PlaySFX(pickUpSound);
        isDragging = true;
        if (lvCtrl != null) lvCtrl.curBaseOb = this;

        var pointerWorldPosition = cam.ScreenToWorldPoint(Input.mousePosition);
        lastPos = pointerWorldPosition;
        offsetDistance = pointerWorldPosition - CachedTransform.position;
        CachedTransform.localScale = pressScale;
        CachedTransform.rotation = Quaternion.Euler(pressRo);
    }

    protected virtual void OnMouseUp()
    {
        if (GameManager.GameState != GameState.Playing) return;

        isDragging = false;
        var cam = MainCamera;
        if (cam != null)
        {
            curPos = cam.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    protected virtual void OnMouseDrag()
    {
        if (GameManager.GameState != GameState.Playing) return;
        if (!isDragging) return;

        var cam = MainCamera;
        if (cam == null) return;

        var worldPosition = cam.ScreenToWorldPoint(Input.mousePosition);
        worldPosition.z = CachedTransform.position.z;
        var pos = worldPosition - (Vector3)offsetDistance + (Vector3)_offSetDrag;
        if (rig != null)
            rig.MovePosition(pos);
        else
            CachedTransform.position = pos;
    }

    public virtual void BackDefaultState(Action action = null)
    {
        // AudioManager.Instance.PlaySFX(putDownSound);
        CachedTransform.localScale = defaultScale;
        CachedTransform.rotation = defaultRo;
        action?.Invoke();
    }

    public virtual void Move(Vector3 s, Vector3 e, float dur = 0.5f, Action completed = null)
    {
        SetCanInterract(false);
        CachedTransform.position = s;
        CachedTransform.DOMove(e, dur)
            .OnComplete(() =>
            {
                SetCanInterract(true);
                completed?.Invoke();
            });
    }

    public virtual void MoveBack(Action completed = null)
    {
        if (defaultPosition == null)
        {
            BackDefaultState(completed);
            return;
        }

        Move(CachedTransform.position, defaultPosition.position, 0.5f, () =>
        {
            BackDefaultState();
            completed?.Invoke();
        });
    }

    public void MoveOutFollowDir(float force = 5f)
    {
        if (!gameObject.activeSelf || rig == null) return;

        var dir = (curPos - lastPos).normalized * force;
        rig.velocity = dir;
    }

    public void MoveOutUpOrDown(float force)
    {
        if (!gameObject.activeSelf || rig == null) return;

        var dir = curPos.y > lastPos.y ? Vector2.up : Vector2.down;
        rig.velocity = dir * force;
    }

    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        //if (!col.CompareTag("Edge")) return;
        //Debug.Log("Trigger DeActive baseOb");
        //gameObject.SetActive(false);
    }
}
