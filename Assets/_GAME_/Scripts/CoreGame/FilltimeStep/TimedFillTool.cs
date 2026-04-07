using System;
using DG.Tweening;
using UnityEngine;

public class TimedFillTool : BaseTool
{
    [Header("Fill Config")]
    [SerializeField] protected float timeRequired = 3f;
    [SerializeField] protected bool resetProgressOnInit = true;
    [SerializeField] protected bool autoStopWhenCompleted = true;

    [Header("Fill Visual")]
    [SerializeField] protected Transform fillTarget;
    [SerializeField] protected Vector3 fillStartScale = new(0f, 1f, 1f);
    [SerializeField] protected Vector3 fillEndScale = new(1f, 1f, 1f);

    protected float timeFilled;
    protected bool isFilling;
    protected bool isCompleted;
    protected Tween fillTween;

    public float TimeRequired => timeRequired;
    public float TimeFilled => timeFilled;
    public bool IsFilling => isFilling;
    public bool IsCompleted => isCompleted;

    public override void Init(LevelStateCtrl lvCtrl)
    {
        base.Init(lvCtrl);

        if (resetProgressOnInit)
        {
            ResetFill();
        }
        else
        {
            UpdateFillVisual(GetFillPercent());
        }
    }

    protected override void Update()
    {
        base.Update();
        if (!isFilling || isCompleted) return;

        AddFill(Time.deltaTime);
    }

    public virtual void StartFill()
    {
        if (isCompleted) return;

        isFilling = true;
    }

    public virtual void StopFill()
    {
        isFilling = false;
    }

    public virtual void ResetFill()
    {
        StopFill();
        isCompleted = false;
        timeFilled = 0f;
        UpdateFillVisual(0f);
    }

    public virtual void AddFill(float deltaTime, Action completed = null)
    {
        if (isCompleted) return;
        if (timeRequired <= 0f)
        {
            CompleteFill(completed);
            return;
        }

        timeFilled = Mathf.Min(timeFilled + Mathf.Max(0f, deltaTime), timeRequired);
        UpdateFillVisual(GetFillPercent());

        if (timeFilled < timeRequired) return;

        CompleteFill(completed);
    }

    public virtual void RunFill(float duration = -1f, Action completed = null)
    {
        float runDuration = duration > 0f ? duration : timeRequired;
        if (runDuration <= 0f)
        {
            CompleteFill(completed);
            return;
        }

        StopFill();
        isCompleted = false;
        fillTween?.Kill();
        float startPercent = GetFillPercent();
        fillTween = DOVirtual.Float(startPercent, 1f, runDuration, value =>
        {
            timeFilled = value * timeRequired;
            UpdateFillVisual(value);
        }).SetEase(Ease.Linear)
          .OnComplete(() => CompleteFill(completed));
    }

    public virtual void CompleteFill(Action completed = null)
    {
        if (isCompleted)
        {
            completed?.Invoke();
            return;
        }

        timeFilled = timeRequired;
        isCompleted = true;
        UpdateFillVisual(1f);

        if (autoStopWhenCompleted)
        {
            StopFill();
        }

        completed?.Invoke();
    }

    public virtual float GetFillPercent()
    {
        if (timeRequired <= 0f) return 1f;
        return Mathf.Clamp01(timeFilled / timeRequired);
    }

    public override float GetPerCentInStep()
    {
        return GetFillPercent();
    }

    protected virtual void UpdateFillVisual(float percent)
    {
        if (fillTarget == null) return;

        fillTarget.localScale = Vector3.LerpUnclamped(fillStartScale, fillEndScale, percent);
    }

    protected virtual void OnDisable()
    {
        fillTween?.Kill();
        fillTween = null;
    }
}
