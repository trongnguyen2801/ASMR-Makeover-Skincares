using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelElsaCake : LevelStateCtrl
{
    public static LevelElsaCake Instance;
    [SerializeField] protected List<Chicken> chickensInScene;
    [SerializeField] protected List<ArrangeOb> collectedEggs, eggsToCrack, siftersFlour;
    [SerializeField] protected GameObject scene1, scene2;
    [SerializeField] protected GameObject alnumen, bowlBrown, bowlBlue;
    [SerializeField] protected List<GameObject> listEggYolk;
    [SerializeField] protected List<GameObject> listObAppear1, listObAppear2, listObAppear3, listObAppear4, listObAppear5;
    [SerializeField] protected SpriteRenderer textureAlbumen1, textureAlbumen2;
    [SerializeField] protected Transform posMoveSpatulaBowlBlue;
    private Vector3 oriPosBowlBlue, oriPosBowlBrown;

    private void Awake()
    {
        Instance = this;
    }

    protected override void InitDicStepAction()
    {
        _stepActions = new Dictionary<StepType, MainStep.StepAction>()
        {
            { StepType.Click, ClickStep },
            { StepType.Arrange, ArrangeStep },
        };
    }

    public int numClick = 4;
    private void ClickStep(ref Action onEnter, ref Action onExecute, ref Action onExit)
    {
        List<Chicken> availableChickens = new(chickensInScene);

        onEnter = () =>
        {
            isFinishStep = false;
            foreach (var chicken in chickensInScene)
            {
                chicken.Init(this);
            }
        };

        onExecute = () =>
        {
            if (isFinishStep) return;
            if (!Input.GetMouseButtonUp(0)) return;
            if (curBaseOb == null) return;
            var ob = curBaseOb as Chicken;
            if (ob.hasLaidEgg)
            {
                availableChickens.Remove(ob);
                curBaseOb = null;
            }
            if (availableChickens.Count > 0) return;
            isFinishStep = true;
            DOVirtual.DelayedCall(0.1f, CheckWinOrNextState);
        };

        onExit = () => { };
    }

    public int numOfClick = 3;
    private void SiftersFlourStep(ref Action onEnter, ref Action onExecute, ref Action onExit)
    {
        onEnter = () =>
        {
            isFinishStep = false;
            BaseStep curStep = steps[curStepIndex];
            curStepToolOb = curStep.stepTool;
            var col = siftersFlour[0].GetComponent<Collider2D>();
            if (col != null) col.enabled = true;
        };

        onExecute = () =>
        {
            if (isFinishStep) return;
            if (!Input.GetMouseButtonUp(0)) return;
            if (numOfClick > 0) return;
            isFinishStep = true;
            DOVirtual.DelayedCall(0.1f, CheckWinOrNextState);
        };

        onExit = () => { };
    }

    private void ArrangeStep(ref Action onEnter, ref Action onExecute, ref Action onExit)
    {
        List<ArrangeOb> curLiAs = null;
        float sumOfItem = 0;

        onEnter = () =>
        {
            isFinishStep = false;
            BaseStep curStep = steps[curStepIndex];
            curStepToolOb = curStep.stepTool;

            void HandleListArrange(List<ArrangeOb> listItem)
            {
                foreach (var item in listItem)
                {
                    item.Init(this);
                    var col = item.GetComponent<Collider2D>();
                    if (col != null) col.enabled = true;
                }
            }

            switch (curStepIndex)
            {
                case 1:
                    curLiAs = collectedEggs.ToList();
                    HandleListArrange(curLiAs);
                    break;
                case 2:
                    DOVirtual.DelayedCall(0.2f, () =>
                    {
                        foreach (var item in listObAppear1)
                            item.SetActive(false);
                        curLiAs = eggsToCrack.ToList();
                        HandleListArrange(curLiAs);
                        DoFadeAllSpriteRenderers(scene1, 0f, 0.5f, () =>
                        {
                            scene1.SetActive(false);
                            MoveDownAndFadeInSequential(listObAppear1, 0.5f, true, 0.2f, () =>
                            {
                                oriPosBowlBlue = bowlBlue.transform.position;
                                oriPosBowlBrown = bowlBrown.transform.position;
                            });
                        });
                    });
                    break;
                case 5:
                    DOVirtual.DelayedCall(1f, () =>
                    {
                        MoveDownAndFadeInSequential(listObAppear2, 0.5f, false, 0.2f, () =>
                        {
                            MoveDownAndFadeInSequential(listObAppear3, 0.5f, true, 0.2f);
                            curLiAs = siftersFlour.ToList();
                            HandleListArrange(curLiAs);
                        });
                    });
                    break;

            }
            if (curLiAs != null) sumOfItem = curLiAs.Count;
        };

        onExecute = () =>
        {
            if (isFinishStep) return;
            if (!Input.GetMouseButtonUp(0)) return;
            Debug.Log("Assemble step excute" + curStepIndex);
            if (curBaseOb == null) return;
            var ob = curBaseOb as ArrangeOb;
            if (ob == null) return;
            if (ob.CanAssembleMultiplePlaces())
            {
                curLiAs?.Remove(ob);
                ob?.MoveInMultiplePlaces();
                SetPercentStep((sumOfItem - (curLiAs?.Count ?? 0)) / sumOfItem);
                curBaseOb = null;
            }
            else
            {
                if (ob != null) ob.MoveBack();
                curBaseOb = null;
            }
            if (curLiAs != null && curLiAs.Count > 0) return;
            isFinishStep = true;
            DOVirtual.DelayedCall(0.2f, CheckWinOrNextState);
        };

        onExit = () =>
        {
        };
    }

    protected void PourStep(ref Action onEnter, ref Action onExecute, ref Action onExit)
    {
        ToolPour tool = null;
        onEnter = () =>
        {
            isFinishStep = false;
            BaseStep curStep = steps[curStepIndex];
            switch (curStepIndex)
            {
                case 3:
                    bowlBrown.transform.SetParent(bowlBlue.transform, worldPositionStays: true);
                    bowlBlue.transform.DOMove(new Vector3(0, 0, 90), 1f)
                        .SetEase(Ease.InOutQuad)
                        .SetDelay(3f)
                        .OnComplete(() =>
                        {
                            curStepToolOb = curStep.stepTool;
                            tool = curStepToolOb as ToolPour;
                            MoveDownAndFadeInSequential(listObAppear1[2], 0.5f, false, 0.2f, () =>
                            {
                                MoveDownAndFadeInSequential(listObAppear2, 0.5f, true, 0.2f, ()=>
                                {
                                    var anim = listObAppear2[1]?.GetComponent<Animator>();
                                    if (anim != null) anim.enabled = true;
                                });
                            });
                        });
                    bowlBlue.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 1f).SetEase(Ease.InOutQuad).SetDelay(3f);
                    bowlBrown.transform.DOScale(new Vector3(0.9f, 0.9f, 0.9f), 1f).SetEase(Ease.InOutQuad).SetDelay(3f);
                    break;
                case 8:
                    DOVirtual.DelayedCall(1.2f, () =>
                    {
                        MoveDownAndFadeInSequential(listObAppear3, 0.5f, false, 0.2f, () =>
                        {
                            MoveDownAndFadeInSequential(listObAppear4, 0.5f, true, 0.2f);
                            curStepToolOb = curStep.stepTool;
                            tool = curStepToolOb as ToolPour;
                        });
                    });
                    break;
                case 4:
                case 6:
                case 11:
                    curStepToolOb = curStep.stepTool;
                    tool = curStepToolOb as ToolPour;
                    break;
            }
        };

        onExecute = () =>
        {
            if (tool == null) return;
            if (tool.checkMoveToPoint)
                tool.SetCanUseWithCol(false);
            if (isFinishStep) return;
            if (!Input.GetMouseButtonUp(0)) return;
            if (curToolOb == null) return;
            var toolUse = curToolOb as ToolPour;
            if (toolUse == null) return;
            if (toolUse.CanMoveToOb() && curToolOb == curStepToolOb)
            {
                isUsingCorrectTool = false;
                toolUse.checkMoveToPoint = true;
                isFinishStep = true;
                SetPercentStep(1f);
                toolUse.MoveToPoint(() =>
                {
                    // AudioManager.Instance.StopEffect();
                    DOVirtual.DelayedCall(0.2f, CheckWinOrNextState);
                    toolUse.MoveBack();
                });
            }
            else
            {
                curToolOb.MoveBack();
                curToolOb = null;
            }
        };

        onExit = () =>
        {
        };
    }

    protected void HandMixerStep(ref Action onEnter, ref Action onExecute, ref Action onExit)
    {
        ToolHandMixer tool = null;
        Animator animator = null;

        onEnter = () =>
        {
            isFinishStep = false;
            BaseStep curStep = steps[curStepIndex];
            switch (curStepIndex)
            {
                case 9:
                    DOVirtual.DelayedCall(0.3f, () =>
                    {
                        curStepToolOb = curStep.stepTool;
                        tool = curStepToolOb as ToolHandMixer;
                        animator = curStepToolOb.GetComponent<Animator>();
                    });
                    break;
                case 14:
                    bowlBrown.transform.SetParent(bowlBlue.transform);
                    curStepToolOb = curStep.stepTool;
                    tool = curStepToolOb as ToolHandMixer;
                    tool.transform.SetParent(bowlBlue.transform);
                    bowlBlue.transform.DOMove(new Vector3(0, 0, 90), 1f)
                        .SetEase(Ease.InOutQuad)
                        .SetDelay(1f)
                        .OnComplete(() =>
                        {
                            animator = curStepToolOb.GetComponent<Animator>();
                            if (animator != null) animator.enabled = true;

                            tool.inBowl = true;
                            tool.Init(this);
                            tool.ChangeAnim("Blend");
                            animator.speed = 0;
                        });
                    bowlBlue.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 1f).SetEase(Ease.InOutQuad).SetDelay(1f);
                    bowlBrown.transform.DOScale(new Vector3(0.9f, 0.9f, 0.9f), 1f).SetEase(Ease.InOutQuad).SetDelay(1f);
                    break;
            }
        };

        onExecute = () =>
        {
            if (isFinishStep) return;
            if (Input.GetMouseButton(0))
            {
                if (animator != null)
                {
                    AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                if (curToolOb == null) return;
                var tool = curToolOb as ToolHandMixer;
                if (tool == null) return;
                if (tool.CanMoveToOb() && curToolOb == curStepToolOb && curStepIndex == 9)
                    tool.MoveToPoint(() =>
                    {
                        isUsingCorrectTool = false;
                    });
                if (animator != null)
                {
                    AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                    Debug.Log("check time: " + stateInfo.normalizedTime);
                    if (stateInfo.normalizedTime >= 0.9)
                    {
                        isFinishStep = true;
                        var anim = tool.GetComponent<Animator>();
                        anim.enabled = false;
                        if (curStepIndex == 14) tool.gameObject.SetActive(false);
                        else tool.MoveBack();
                        CheckWinOrNextState();
                    }
                }
            }
        };

        onExit = () =>
        {
        };
    }

    protected void BlendMachineStep(ref Action onEnter, ref Action onExecute, ref Action onExit)
    {
        ToolMachineMixer tool = null;
        onEnter = () =>
        {
            isFinishStep = false;
            BaseStep curStep = steps[curStepIndex];

            switch (curStepIndex)
            {
                case 10:
                    bowlBrown.transform.SetParent(scene2.transform, worldPositionStays: true);
                    bowlBlue.transform.SetParent(bowlBrown.transform, worldPositionStays: true);
                    bowlBrown.transform.DOMove(new Vector3(0, 0, 90), 1f)
                        .SetEase(Ease.InOutQuad)
                        .OnComplete(() =>
                        {
                            MoveDownAndFadeInSequential(listObAppear4, 0.5f, false, 0.2f, () =>
                            {
                                MoveDownAndFadeInSequential(listObAppear5, 0.5f, true, 0.2f);
                            });
                            HandleDisplaySp(textureAlbumen1, 0f);
                            textureAlbumen1.gameObject.SetActive(true);
                            curStepToolOb = curStep.stepTool;
                            tool = curStepToolOb as ToolMachineMixer;
                            tool.timeSpentInArea = 0f;
                            var col = bowlBrown.GetComponent<Collider2D>();
                            if (col != null) col.enabled = true;
                        });
                    bowlBrown.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 1f).SetEase(Ease.InOutQuad);
                    bowlBlue.transform.DOScale(new Vector3(0.9f, 0.9f, 0.9f), 1f).SetEase(Ease.InOutQuad);
                    break;
                case 12:
                    HandleDisplaySp(textureAlbumen2, 0f);
                    textureAlbumen2.gameObject.SetActive(true);
                    curStepToolOb = curStep.stepTool;
                    tool = curStepToolOb as ToolMachineMixer;
                    tool.timeSpentInArea = 0f;
                    var col = bowlBrown.GetComponent<Collider2D>();
                    if (col != null) col.enabled = true;
                    break;
            }
        };

        onExecute = () =>
        {
            if (isFinishStep) return;
            if (tool == null) return;
            if (Input.GetMouseButton(0))
            {
                //SetPercentStep(tool.GetPercentStep());
                if (curStepIndex == 10)
                {
                    var color = textureAlbumen1.color;
                    color.a = Math.Clamp(tool.GetPercentStep(), 0, 1f);
                    textureAlbumen1.color = color;
                }
                if (curStepIndex == 12)
                {
                    var color = textureAlbumen2.color;
                    color.a = Math.Clamp(tool.GetPercentStep(), 0, 1f);
                    textureAlbumen2.color = color;
                }
            }
            if (!Input.GetMouseButtonUp(0)) return;
            if (tool.CheckCompleteSpraying())
            {
                SetPercentStep(1f);
                isFinishStep = true;
                var col = bowlBrown.GetComponent<Collider2D>();
                if (col != null) col.enabled = false;
                DOVirtual.DelayedCall(0.2f, () =>
                {
                    tool.MoveBack();
                    CheckWinOrNextState();
                });
            }
        };

        onExit = () =>
        {

        };
    }

    private void ScoopStep(ref Action onEnter, ref Action onExecute, ref Action onExit)
    {
        onEnter = () =>
        {
            bowlBlue.transform.SetParent(scene2.transform, worldPositionStays: true);
            bowlBrown.transform.DOMove(oriPosBowlBrown, 1f)
                .SetEase(Ease.InOutQuad)
                .SetDelay(1f).OnComplete(() =>
                {
                    isFinishStep = false;
                    BaseStep curStep = steps[curStepIndex];
                    curStepToolOb = curStep.stepTool;
                });

            bowlBlue.transform.DOMove(oriPosBowlBlue, 1f)
                .SetEase(Ease.InOutQuad)
                .SetDelay(1f);

            bowlBrown.transform.DOScale(new Vector3(1f, 1f, 1f), 1f).SetEase(Ease.InOutQuad).SetDelay(1f);
        };

        onExecute = () =>
        {
            if (isFinishStep) return;
            if (!Input.GetMouseButtonUp(0)) return;
            if (curToolOb == null) return;
            var tool = curToolOb as ToolSpatula;
            if (tool == null) return;
            if (tool.CanMoveToOb() && curToolOb == curStepToolOb)
            {
                isFinishStep = true;
                //SetPercentStep(1f);
                tool.MoveToPoint(() =>
                {
                    isUsingCorrectTool = false;
                    DOVirtual.DelayedCall(0.7f,()=> { CheckWinOrNextState(); });
                });
            }
            else
            {
                curToolOb.MoveBack();
                curToolOb = null;
            }
        };

        onExit = () =>
        {
        };
    }

    private int scaleCount = 0;
    public void HandleAlnumenInBowl()
    {
        alnumen.SetActive(true);
        scaleCount++;
        float currentScale = 0.8f + (0.05f * scaleCount);
        alnumen.transform.DOScale(Vector3.one * currentScale, 0.2f)
            .SetEase(Ease.InOutQuad);
        Debug.Log($"Scale {scaleCount}: {currentScale}");
    }

    public void HandleEggYolkInBowl()
    {
        listEggYolk[scaleCount - 1].SetActive(true);
        ToggleColEgg(true);
    }

    public void HandleBowlEgg()
    {
        bowlBrown.transform.rotation = Quaternion.Euler(0, 0, 0);

        Sequence shakeSeq = DOTween.Sequence();

        shakeSeq.Append(bowlBrown.transform.DORotate(new Vector3(0, 0, -5), 0.05f))
                .Append(bowlBrown.transform.DORotate(new Vector3(0, 0, 5), 0.1f))
                .Append(bowlBrown.transform.DORotate(new Vector3(0, 0, -3), 0.08f))
                .Append(bowlBrown.transform.DORotate(new Vector3(0, 0, 3), 0.06f))
                .Append(bowlBrown.transform.DORotate(Vector3.zero, 0.05f))
                .SetEase(Ease.InOutSine);
    }

    public void ToggleColEgg(bool toggle)
    {
        foreach (var item in eggsToCrack)
        {
            var col = item.GetComponent<Collider2D>();
            if (col != null) col.enabled = toggle;
        }
    }

    //Next state
    //public void Init(LevelElsaCakeFull levelElsaCakeFull)
    //{
    //    base.Init();
    //}
    protected override void CheckWinOrNextState()
    {
        base.CheckWinOrNextState();
        GameManager.Instance.uiManager.GetScreen<GamePlayScreen>().ActiveIconCorrect();
        Debug.Log("Check Next State 345");
    }


    public override void HandleNextState( LevelStateCtrl curState, LevelStateCtrl nextState)
    {
        nextState.gameObject.SetActive(true);
        curState.transform.DOMoveX(curState.transform.position.x - 18f, 1.2f);
        nextState.transform.DOMoveX(0, 1f).OnComplete(() =>
        {
            levelCtrlFull.OnChangeNextState();
        });
    }
}