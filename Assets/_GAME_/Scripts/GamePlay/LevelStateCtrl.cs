using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using Sequence = DG.Tweening.Sequence;

public class LevelStateCtrl : MonoBehaviour
{
    //public  LevelCtrl InstanceLv;
    [Header("Data Hint")]
    [SerializeField] protected Sprite hintImg;
    [SerializeField] protected int hintTextSize = 45;
    [SerializeField] protected Vector2 hintTextPos;

    public BaseLevelCtrl levelCtrlFull;

    [Header("Handle Step")]
    public int curPercentWinStep = 80;
    public bool isFinishLv, isFinishStep, isWinStep;
    public bool isUsingCorrectTool, isChooseToolLv;
    public bool canCheckPercent = true, isUseSpriteMaskInStep = true;

    public BaseOb curStepBaseOb, curBaseOb;
    public BaseTool curStepToolOb, curToolOb;
    protected int numOfFailTool;
    protected float percentStep;

    public int NumOfFailTool
    {
        get => numOfFailTool;
        set
        {
            numOfFailTool = value;
            onToolFail?.Invoke(numOfFailTool, maxNumOfFailTool);
        }
    }

    public int maxNumOfFailTool = -1;
    private Action<int, int> onToolFail;
    public float minZOb = -10f;
    public bool isLastState = false;

    [SerializeField] protected List<BaseStep> steps;
    [SerializeField] protected List<BaseTool> listTools;

    [SerializeField] protected HandleSpriteMask spriteMask;
    
    [SerializeField] protected ParticleSystem vfxWin;
    [SerializeField] protected MasksOb curMasksOb;
    [SerializeField] protected MaskOb curMaskOb;
    [SerializeField] protected int curStepIndex;
    [SerializeField] protected int curMaskObIndex;
    [SerializeField] protected Transform startPosTool, defaultPosTool;
    [SerializeField] private GameObject animWin;
    protected readonly MainStep _mainStep = new();
    protected Dictionary<StepType, MainStep.StepAction> _stepActions;

    public virtual void Init(BaseLevelCtrl levelCtrlFull = null)
    {
        if(levelCtrlFull != null)
            this.levelCtrlFull = levelCtrlFull;
       // InstanceLv = this;
        Debug.Log("Init Step");
        InitDicStepAction();
        // curStepIndex = 0;
        curMaskObIndex = 0;
        spriteMask?.Init(this);
        foreach (var tool in listTools)
        {
            tool.Init(this);
        }
        StartStep();
    }
    protected virtual void InitDicStepAction()
    {

    }
    protected virtual void Update()
    {
        if (GameManager.GameState != GameState.Playing) return;
        _mainStep?.Execute();
    }

    protected virtual void StartStep()
    {
        isWinStep = false;
        isFinishStep = false;
        SetPercentStep(0);
        BaseStep nextStep = steps[curStepIndex];
        _mainStep?.ChangeStep(_stepActions[nextStep.stepType]);
    }

    public virtual void Skip()
    {
        Win();
    }

    protected virtual void Win()
    {
        if (isFinishLv) return;
        Debug.Log("Win Lv");
        // BBEventDispatcher.Notify(BBEventId.OnWinLv);
        isFinishLv = true;
    }

    public virtual void Lose()
    {
        if (isFinishLv) return;
        // BBEventDispatcher.Notify(BBEventId.OnLoseLv);
        // AudioManager.Instance.StopEffect();
        isFinishLv = true;
    }

    public virtual void TimeUp()
    {
        if (isFinishLv) return;
        // BBEventDispatcher.Notify(BBEventId.OnTimeUp);
        // AudioManager.Instance.StopEffect();
        isFinishLv = true;
    }

    public virtual void Revive()
    {
        isFinishLv = false;
        NumOfFailTool = 0;
    }

    public virtual void Hint()
    {
        GameManager.Instance.SwitchGameState(GameState.Pause);
        //var popup = Popup.PopupSystem.GetOpenBuilder()
        //    .SetType(PopupType.PopupHint)
        //    .SetCurrentPopupBehaviour(Popup.CurrentPopupBehaviour.KeepShowing).SetDelayTime(0f).Open<PopupHint>();
        //popup.OnCloseEvent += () => { GameManager.Instance.SwitchGameState(GameState.Playing); };
        //if (hintImg != null)
        //    popup.SetImg(hintImg, hintTextPos, hintTextSize);
    }

    protected IEnumerator NextStepVfx()
    {
        var curStep = steps[curStepIndex];
        if (curMaskOb != null)
            curMaskOb?.gameObject?.SetActive(false);
        curStep?.vfxEnd?.gameObject?.SetActive(true);
        yield return new WaitForSeconds(3.5f);
        curStep?.vfxEnd?.gameObject?.SetActive(false);
        curStepIndex++;
        StartStep();
    }

    protected virtual void ChangeMaskOb()
    {
        if (curMasksOb == null) return;

        curMaskOb = curMasksOb.newMasks[curMaskObIndex];
        if (curMaskOb != null)
            curMaskOb?.gameObject?.SetActive(true);

    }

    protected virtual void ChangeMasksOb(BaseStep nextStep, Action callBack = null)
    {
        if (nextStep.stepMasksOb != null && curMasksOb != nextStep.stepMasksOb)
        {
            curMasksOb = nextStep.stepMasksOb;
            curMaskObIndex = 0;
            curMasksOb?.gameObject?.SetActive(true);
            callBack?.Invoke();

        }
        else
        {
            callBack?.Invoke();
        }
    }

    protected void CallBackStep(bool IsVisibleInsideMask)
    {
        Debug.Log("0MoveTool");
        BaseStep curStep = steps[curStepIndex];
        ChangeMaskOb();
        //setupSpriteMask
        var sprite = curMaskOb.maskSprite;
        spriteMask.ResetSprite(sprite);
        Vector3 pos = curMaskOb.transform.position;
        pos.z = spriteMask.transform.position.z;
        spriteMask.transform.position = pos;
        spriteMask.transform.localScale = curMaskOb.transform.localScale;
        if (IsVisibleInsideMask && !curMaskOb.isModeCleanOut)
        {
            curMaskOb.disPlaySprRend.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        }
        else if (IsVisibleInsideMask && curMaskOb.isModeCleanOut)
        {
            curMaskOb.disPlaySprRend.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
        }
        //if (lastMaskInOb != null)
        //    lastMaskInOb.disPlaySprRend.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
    }
    protected virtual void NextStep()
    {
        curStepIndex++;
        StartStep();
    }

    protected virtual void CheckWinOrNextState()
    {
        if (isFinishLv) return;
        Debug.Log("Check Win Lv");
        if (curStepIndex == steps.Count - 1)
        {
            if (isLastState)
            {
                Win();
            }else
               levelCtrlFull.ChangeState(HandleNextState);
            return;
        }
        var curStep = steps[curStepIndex];
        if (curStep.vfxEnd != null)
            StartCoroutine(NextStepVfx());
        else
            NextStep();
    }

    public virtual void HandleNextState(LevelStateCtrl curState, LevelStateCtrl nextState)
    {
       
    }
    public float GetPercentStep()
    {
        return percentStep;
    }

    protected void SetPercentStep(float val)
    {
        percentStep = val;
    }

    public void OnToolFail(Action<int, int> onToolFail)
    {
        this.onToolFail = onToolFail;
    }

    protected void MoveObject(GameObject targetObject, string direction, float distance, float duration, bool isActive,
        Action callBack = null)
    {
        Vector3 currentPosition = targetObject.transform.position;
        Vector3 targetPosition = currentPosition;
        switch (direction.ToLower())
        {
            case "up":
                targetPosition += new Vector3(0, distance, 0);
                break;
            case "down":
                targetPosition += new Vector3(0, -distance, 0);
                break;
            case "left":
                targetPosition += new Vector3(-distance, 0, 0);
                break;
            case "right":
                targetPosition += new Vector3(distance, 0, 0);
                break;
            default:
                Debug.LogWarning("NoneDirrect");
                return;
        }

        targetObject.transform.DOMove(targetPosition, duration).OnComplete((() =>
        {
            targetObject.SetActive(isActive);
            callBack?.Invoke();
        }));
    }

    protected virtual void DoFadeAllSpriteRenderers(GameObject parent, float targetAlpha, float duration, Action action = null)
    {
        SpriteRenderer[] sprites = parent.GetComponentsInChildren<SpriteRenderer>(true);
        for (int i = 0; i < sprites.Length; i++)
        {
            var tween = sprites[i].DOFade(targetAlpha, duration).SetEase(Ease.InOutQuad);

            if (i == sprites.Length - 1)
                tween.OnComplete(() => action?.Invoke());
        }
    }
    
    protected virtual void DoFadeSpriteRenderers(GameObject parent, float targetAlpha, float duration, Action action = null)
    {
        SpriteRenderer sprites = parent.GetComponent<SpriteRenderer>();
            var tween = sprites.DOFade(targetAlpha, duration).SetEase(Ease.InOutQuad);
            tween.OnComplete(() => action?.Invoke());
    }

    public void MoveDownAndFadeInSequential(object input, float moveDistance = 1f, bool fadeIn = true, float duration = 0.5f, Action action = null)
    {
        Sequence fullSequence = DOTween.Sequence();

        List<GameObject> objects = new();

        if (input is GameObject go && go != null)
            objects.Add(go);
        else if (input is List<GameObject> goList && goList != null)
            objects = goList;
        else
            return;

        foreach (var obj in objects)
        {
            obj.SetActive(true);
            List<SpriteRenderer> sprites = new(obj.GetComponentsInChildren<SpriteRenderer>());
            List<SpriteRenderer> tempSprites = sprites;
            if (sprites == null || sprites.Count == 0) continue;

            foreach (var sr in tempSprites)
            {
                if (!fadeIn) break;
                Color c = sr.color;
                c.a = 0;
                sr.color = c;
            }

            Sequence objSeq = DOTween.Sequence();

            foreach (var sr in sprites)
            {
                objSeq.Join(sr.DOFade(fadeIn ? 1f : 0f, duration).SetEase(Ease.InOutQuad));
            }

            objSeq.Join(obj.transform.DOMoveY(obj.transform.position.y - moveDistance, duration).SetEase(Ease.InOutQuad));
            if (!fadeIn)
            {
                objSeq.OnComplete(() => obj.SetActive(false));
                var col = obj.GetComponent<Collider2D>();
                if (col != null)
                    Destroy(col);
            }
            fullSequence.Append(objSeq);
        }
        fullSequence.OnComplete(() => action?.Invoke());
    }

    protected void ToggleActiveTools(bool toggle)
    {
        foreach (var tool in listTools)
            tool?.gameObject.SetActive(toggle);
    }

    protected virtual void HandleDisplaySp(SpriteRenderer sp, float fade)
    {
        Color c = sp.color;
        c.a = fade;
        sp.color = c;
    }
}
