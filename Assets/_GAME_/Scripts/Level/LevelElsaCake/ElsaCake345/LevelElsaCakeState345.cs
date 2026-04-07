using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelElsaCakeState345 : LevelStateCtrl
{
    public override void Init(BaseLevelCtrl baseLevelCtrl)
    {
        base.Init(baseLevelCtrl);
        stoveElsaCake.Init(this);
    }

    protected override void InitDicStepAction()
    {
        _stepActions = new Dictionary<StepType, MainStep.StepAction>()
        {
            { StepType.Click, ClickStep },
            { StepType.Arrange, ArrangeStep },
            { StepType.Open, OpenStoveStep },
            { StepType.Close, CloseStoveStep },
            { StepType.TurnOn, TurnOnStep },
            { StepType.Clean,  CleanStep},
        };
    }

    public ArrangeOb mold;
    public ArrangeActiveOb decoElsa;
    public List<ArrangeActiveOb> decorOthers;
    public SpriteRenderer shadowDecorOthers;
    protected void ArrangeStep(ref Action onEnter, ref Action onExecute, ref Action onExit)
    {
        float sumOfItem = 0;
        float sumOfCompletetdItem = 0;

        onEnter = () =>
        {
            isFinishStep = false;
            BaseStep curStep = steps[curStepIndex];
            curStepBaseOb = curStep.stepRequiedOb;
            sumOfCompletetdItem = 0;
            switch (curStepIndex)
            {
                case 2:
                    mold.Init(this);
                    mold.SetCanUseWithCol(true);
                    sumOfItem = 1;
                    break;
                case 10:
                    decoElsa.Init(this);
                    decoElsa.SetCanUseWithCol(true);
                    sumOfItem = 1;
                    break;
                case 12:
                    shadowDecorOthers.gameObject.SetActive(true); 
                    shadowDecorOthers.DOFade(1f, 1f);
                    foreach (var decor in decorOthers)
                    {
                        decor.Init(this);
                        decor.SetCanUseWithCol(true);
                    }
                    sumOfItem = decorOthers.Count;
                    break;

            }
        };

        onExecute = () =>
        {
            if (isFinishStep) return;
            if (!Input.GetMouseButtonUp(0)) return;

            if (curBaseOb == null) return;
            var ob = curBaseOb as ArrangeOb;
            if (ob == null) return;
            Debug.Log("Assemble step excute" + curStepIndex);
            if (ob.CanAssembleMultiplePlaces())
            {
                Debug.Log("1Assemble step excute" + curStepIndex);
                //AudioManager.Instance.PlaySFX(AudioClipId.ThrowTrash);
                ob?.MoveInMultiplePlaces();
                SetPercentStep(sumOfCompletetdItem / sumOfItem);
                sumOfCompletetdItem++;
                curBaseOb = null;
            }
            else
            {
                if (ob != null) ob.BackDefaultState();
                curBaseOb = null;
            }
            if (sumOfCompletetdItem < sumOfItem) return;
            isFinishStep = true;
            GameManager.Instance.uiManager.GetScreen<GamePlayScreen>().ActiveIconCorrect();
            DOVirtual.DelayedCall(0.7f, CheckWinOrNextState);
        };

        onExit = () =>
        {

        };
    }

    public BaseOb bowl1;
    protected void ScoopStep(ref Action onEnter, ref Action onExecute, ref Action onExit)
    {
        ScoopTool scoopTool = null;
        onEnter = () =>
        {
            curStepToolOb = steps[curStepIndex].stepTool;
            scoopTool = curStepToolOb as ScoopTool;
            scoopTool.Appear();
        };

        onExecute = () =>
        {
            if (isFinishStep) return;
            // Debug.Log("Scoop step excute" + curStepIndex);
            if (scoopTool.CheckCanCatch())
            {
                scoopTool.Catch();
            }
            if (scoopTool.CheckCanDrop())
            {
                scoopTool.Drop();
            }
            if (scoopTool.CheckCompleted())
            {
                SetPercentStep(1f);
                isFinishStep = true;
                GameManager.Instance.uiManager.GetScreen<GamePlayScreen>().ActiveIconCorrect();
                DOVirtual.DelayedCall(0.7f, CheckWinOrNextState);
            }
        };

        onExit = () =>
        {
            if (bowl1 != null)
            {
                MoveObject(bowl1.gameObject, "left", 20f, 1f, false, default);
                curStepToolOb.Move(curStepToolOb.transform.position, curStepToolOb.transform.position + Vector3.left * 50f, 1f, default);
            }
        };
    }
    public StoveElsaCake stoveElsaCake;
    protected void OpenStoveStep(ref Action onEnter, ref Action onExecute, ref Action onExit)
    {
        onEnter = () =>
        {
            switch (curStepIndex)
            {
                case 1:
                    stoveElsaCake.doorStove.SetCanUseWithCol(true);
                    break;
            }

        };

        onExecute = () =>
        {
            if (isFinishStep) return;
            switch (curStepIndex)
            {
                case 1:
                    if (stoveElsaCake.doorStove.isOpen)
                    {
                        SetPercentStep(1f);
                        GameManager.Instance.uiManager.GetScreen<GamePlayScreen>().ActiveIconCorrect();
                        isFinishStep = true;
                        DOVirtual.DelayedCall(0.5f, CheckWinOrNextState);
                    }
                    break;
            }

        };

        onExit = () =>
        {

        };
    }
    protected void CloseStoveStep(ref Action onEnter, ref Action onExecute, ref Action onExit)
    {
        onEnter = () =>
        {
            stoveElsaCake.doorStove.SetCanUseWithCol(true);
        };

        onExecute = () =>
        {
            if (isFinishStep) return;
            if (!stoveElsaCake.doorStove.isOpen)
            {
                SetPercentStep(1f);
                GameManager.Instance.uiManager.GetScreen<GamePlayScreen>().ActiveIconCorrect();
                isFinishStep = true;
                DOVirtual.DelayedCall(0.5f, CheckWinOrNextState);
            }
        };

        onExit = () =>
        {

        };
    }

    public SwitchOb switchOb;
    public SpriteRenderer sprCakeCompleted;
    public SpriteRenderer lightStove;
    protected void TurnOnStep(ref Action onEnter, ref Action onExecute, ref Action onExit)
    {
        onEnter = () =>
        {
            switchOb.SetCanUseWithCol(true);
        };

        onExecute = () =>
        {
            if (isFinishStep) return;
            if (switchOb.isOpen)
            {
                SetPercentStep(1f);
                lightStove?.gameObject.SetActive(true);
                lightStove?.DOFade(1, 0.5f);
                GameManager.Instance.uiManager.GetScreen<GamePlayScreen>().ActiveIconCorrect();
                isFinishStep = true;
                // AudioManager.Instance.PlaySoundVfxRepeated(4f, 2.4f, AudioClipId.Fast_Clock);
                // AudioManager.Instance.PlaySoundVfxRepeated(4f, 4.1f, AudioClipId.Baking);
                sprCakeCompleted.gameObject.SetActive(true);
                sprCakeCompleted.DOFade(1, 5f);
                stoveElsaCake.Running(5f, () =>
                {
                    lightStove?.gameObject.SetActive(false);
                    DOVirtual.DelayedCall(1f, CheckWinOrNextState);
                });

            }
        };

        onExit = () =>
        {

        };
    }

    public GameObject scene3, scene4;
    public Moild2ClickOb moild2ClickOb;
    public Transform possMoldDropAppear;
    public MoilDropClickOb moilDropClickOb;
    public GameObject maskCakes;
    private void ClickStep(ref Action onEnter, ref Action onExecute, ref Action onExit)
    {
        onEnter = () =>
        {
            switch (curStepIndex)
            {
                case 5:
                    moild2ClickOb.Init(this);
                    moild2ClickOb.SetCanUseWithCol(true);
                    break;
                case 6:
                    moilDropClickOb.Init(this);
                    moilDropClickOb.transform.DOMove(possMoldDropAppear.position, 1f).SetEase(Ease.OutBack);
                    moilDropClickOb.SetCanUseWithCol(true);
                    break;

            }

        };

        onExecute = () =>
        {
            if (isFinishStep) return;
            switch (curStepIndex)
            {
                case 5:
                    if (moild2ClickOb.isCompleted)
                    {
                        stoveElsaCake.doorStove.Close();
                        SetPercentStep(1f);
                        GameManager.Instance.uiManager.GetScreen<GamePlayScreen>().ActiveIconCorrect();
                        isFinishStep = true;
                        DOVirtual.DelayedCall(2f, () =>
                        {
                            scene3.transform.DOLocalMoveX(-18f, 1f);
                            scene4.transform.DOLocalMoveX(0f, 1f).OnComplete(() =>
                            {
                                scene3.SetActive(false);
                                CheckWinOrNextState();
                            });
                        });
                    }
                    break;
                case 6:
                    if (moilDropClickOb.isCompleted)
                    {
                        SetPercentStep(1f);
                        GameManager.Instance.uiManager.GetScreen<GamePlayScreen>().ActiveIconCorrect();
                        isFinishStep = true;
                        DOVirtual.DelayedCall(2f, () =>
                        {
                            maskCakes.SetActive(true);
                            moilDropClickOb.gameObject.SetActive(false);
                            CheckWinOrNextState();
                        });
                    }
                    break;
            }

        };
        onExit = () =>
        {

        };
    }


    public GameObject wraptool;
    protected void CuttingStep(ref Action onEnter, ref Action onExecute, ref Action onExit)
    {
        KnifeElsa tool = null;
        onEnter = () =>
        {
            wraptool.transform.DOMove(new Vector3(0, 0, wraptool.transform.position.z), 1f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                curStepToolOb = steps[curStepIndex].stepTool;
                tool = curStepToolOb as KnifeElsa;
            });
        };

        onExecute = () =>
        {
            if (isFinishStep) return;
            if(tool == null) return;
            if (tool.CheckCanCutting())
            {
                tool.Cutting();
            }
            if (tool.CheckCompletedCutting())
            {
                SetPercentStep(1f);
                GameManager.Instance.uiManager.GetScreen<GamePlayScreen>().ActiveIconCorrect();
                isFinishStep = true;
                DOVirtual.DelayedCall(2f, () =>
                {
                    CheckWinOrNextState();
                    // tool.Move(tool.transform.position, tool.transform.position + Vector3.down * 50f, 1f, default);
                });
            }
        };

        onExit = () =>
        {
            tool.SetCanInterract(false);
            tool.Move(tool.transform.position, tool.transform.position + Vector3.down * 50f, 1f, default);
        };
    }

    public SpriteRenderer shadowDell1, shadowDell2;
    protected virtual void CleanStep(ref Action onEnter, ref Action onExecute, ref Action onExit)
    {
        BaseStep curStep = steps[curStepIndex];
        onEnter = () =>
        {
            isChooseToolLv = true;
            Debug.Log("0000CleanStep");
            spriteMask.gameObject.SetActive(true);
            canCheckPercent = true;
            isUseSpriteMaskInStep = true;
            curStep = steps[curStepIndex];
            spriteMask.SetUpdateTexture(true);
            curStepToolOb = curStep.stepTool;

            ChangeMasksOb(curStep, () => { CallBackStep(true); });
            curPercentWinStep = curStep.percentStepWin;

            switch (curStepIndex)
            {
                case 9:
                    shadowDell1.gameObject.SetActive(true);
                    shadowDell1.DOFade(1, 1f);
                    break;
                case 13:
                    shadowDell2.gameObject.SetActive(true);
                    shadowDell2.DOFade(1, 1f);
                    break;
            }
        };
        onExecute = () =>
        {
            if (isFinishStep) return;

            if (Input.GetMouseButtonUp(0))
            {
                if (isWinStep)
                {
                    isFinishStep = true;
                    curStepToolOb?.MoveBack();
                    CheckWinOrNextState();
                }
            }
            if (Input.GetMouseButton(0))
            {
                if (!isWinStep)
                {
                    SetPercentStep(spriteMask.percentDelete / curPercentWinStep);
                    if (spriteMask.percentDelete > curPercentWinStep)
                    {
                        GameManager.Instance.uiManager.GetScreen<GamePlayScreen>().ActiveIconCorrect();
                        isWinStep = true;
                    }
                }
            }

        };
        onExit = () =>
        {
            if (curMaskOb != null)
            {
                if (curMaskOb.isModeCleanOut)
                {
                        curMaskOb.disPlaySprRend.maskInteraction = SpriteMaskInteraction.None;
                }
                else
                    curMaskOb?.gameObject?.SetActive(false);
            }
            //if (lastMaskInOb != null)
            //    lastMaskInOb.gameObject.SetActive(false);
            curMaskObIndex++;
            curStepToolOb = null;
        };
    }

    public SpriteRenderer snowDecor;
    protected virtual void PourStep(ref Action onEnter, ref Action onExecute, ref Action onExit)
    {
        DropTool tool = null;

        onEnter = () =>
        {
            curStepToolOb = steps[curStepIndex].stepTool;
            Debug.Log("Pour step enter " + curStepToolOb.gameObject.name);
            tool = curStepToolOb as DropTool;
        };
        onExecute = () =>
        {
            if (isFinishStep) return;
            if (!Input.GetMouseButtonUp(0)) return;
            if (tool == null) return;
            if (tool.CanDrop())
            {
                SetPercentStep(1f);
                tool.Drop();
                tool.SetCanUseWithCol(false);
                // StartCoroutine(PlaySoundVfxCustom(1.1f, AudioClipId.Pouring));
                snowDecor.gameObject.SetActive(true);
                snowDecor.DOFade(1f, 5f);
                isFinishStep = true;
                DOVirtual.DelayedCall(6f, () =>
                {
                    GameManager.Instance.uiManager.GetScreen<GamePlayScreen>().ActiveIconCorrect();
                    tool.SetCanUseWithCol(true);
                    tool.isPouring = false;
                    tool.MoveBack();
                    CheckWinOrNextState();
                    curStepToolOb = null;
                });
            }
        };
        onExit = () =>
        {
        };
    }

    public List<BaseTool> toolDisAppears;
    public SpriteRenderer bgWin;
    protected override void Win()
    {
        _mainStep.onExit?.Invoke();
        var delay = 0f;
        for (int i = 0; i < toolDisAppears.Count; i++)
        {
            var tool = toolDisAppears[i];
            tool.Move(tool.transform.position, tool.transform.position + Vector3.down * 50f, 1f,delay, () =>
            {
                tool.gameObject.SetActive(false);
            });
            delay += 0.1f*i;
        }
        bgWin.gameObject.SetActive(true);
        bgWin.DOFade(1f, 1f);
        maskCakes.transform.DOMoveY(0,1f).SetEase(Ease.Linear).OnComplete(() =>
        {
            vfxWin.transform.position = maskCakes.transform.position;
            vfxWin.gameObject.SetActive(true);
            // AudioManager.Instance.PlaySFX(AudioClipId.Blink);

        });
        DOVirtual.DelayedCall(8f, () =>
        {
            base.Win();
        });
    }
}

