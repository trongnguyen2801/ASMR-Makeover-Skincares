﻿﻿using System;
using System.Collections.Generic;
using System.Linq;
using _GAME_.Scripts.CoreGame;
using DG.Tweening;
using UnityEngine;

namespace _GAME_.Scripts.Level.LevelWatermelon
{
    public class LevelWatermelon : LevelStateCtrl
    {
        public static LevelWatermelon Instance;
        
        //-------------------//
        [Header("Pattern Click")]
        [SerializeField] protected ClickOb nextStep1Btn;
        [SerializeField] protected List<ClickOb> clickStepOptionButtons;
        [SerializeField] protected GameObject headbandListBtn;

        private int _spritesIconStep1Index = 0;
        private int _spritesIconStep2Index = 2;
        
        [SerializeField] protected List<GameObject> listFaceTowel;
        [SerializeField] protected List<GameObject> listOb2;
        
        [SerializeField] protected List<Sprite> spritesIconStep1;
        [SerializeField] protected List<Sprite> spritesIconStep2;
        [SerializeField] protected GameObject step0Ob;
        
        [Header("Pattern Arrange")]
        [SerializeField] protected ArrangeOb maskOb;
        [SerializeField] protected GameObject boxMaskOb;
        [SerializeField] protected GameObject step1Ob;
        
        [Header("Pattern Filltime")]
        [SerializeField] protected GameObject step2Ob;
        [SerializeField] protected GameObject faceMaskObFadeOut;
        
        [Header("Pattern Clean")]
        [SerializeField] protected PatternSelectionCleanStepController eyelinerPatternStep;
        [SerializeField] protected PatternSelectionCleanStepController lipPatternStep;
        [SerializeField] protected int eyelinerStepIndex = -1;
        [SerializeField] protected int lipStepIndex = -1;

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
                { StepType.FillTime, FillTimeStep },
                { StepType.PatternClean, PatternCleanStep },
                { StepType.Clean,  CleanStep},
            };
        }

        private void PatternCleanStep(ref Action onEnter, ref Action onExecute, ref Action onExit)
        {
            PatternSelectionCleanStepController patternController = null;
            PatternCleanBrushTool patternBrushTool = null;

            onEnter = () =>
            {
                isFinishStep = false;
                isChooseToolLv = true;

                BaseStep curStep = steps[curStepIndex];
                curStepToolOb = curStep.stepTool;
                patternBrushTool = curStepToolOb as PatternCleanBrushTool;
                patternController = GetPatternCleanController(curStepIndex);

                if (patternBrushTool != null)
                {
                    patternBrushTool.Init(this);
                }

                patternController?.EnterStep(this);
                SetPercentStep(0f);
            };

            onExecute = () =>
            {
                if (isFinishStep) return;
                if (patternController == null) return;

                patternController.Tick();
                SetPercentStep(patternController.GetCombinedProgress());
                if (!patternController.IsStepCompleted) return;

                isFinishStep = true;
                SetPercentStep(1f);
                DOVirtual.DelayedCall(0.2f, CheckWinOrNextState);
            };

            onExit = () =>
            {
                patternController?.ExitStep();
                curStepToolOb = null;
            };
        }

        private void FillTimeStep(ref Action onEnter, ref Action onExecute, ref Action onExit)
        {
            TimedFillTool tool = null;

            onEnter = () =>
            {
                isFinishStep = false;
                BaseStep curStep = steps[curStepIndex];
                curStepToolOb = curStep.stepTool;
                tool = curStepToolOb as TimedFillTool;
                if (tool == null) return;

                tool.ResetFill();
                tool.RunFill();
                
                SetPercentStep(0f);
                if (curStepIndex == 2)
                {
                    step2Ob.SetActive(true);
                    DoFadeSpriteRenderers(faceMaskObFadeOut,0,3f);
                }
            };

            onExecute = () =>
            {
                if (isFinishStep) return;
                if (tool == null) return;

                SetPercentStep(tool.GetPerCentInStep());
                if (!tool.IsCompleted) return;

                isFinishStep = true;
                SetPercentStep(1f);
                DOVirtual.DelayedCall(0.2f, CheckWinOrNextState);
                if (curStepIndex == 2)
                {
                    step2Ob.SetActive(false);
                }
            };

            onExit = () =>
            {
                tool?.StopFill();
            };
        }

        protected virtual void CleanStep(ref Action onEnter, ref Action onExecute, ref Action onExit)
        {
            BaseStep curStep = null;

            onEnter = () =>
            {
                curStep = steps[curStepIndex];
                isChooseToolLv = true;
                Debug.Log("0000CleanStep");
                spriteMask.gameObject.SetActive(true);
                canCheckPercent = true;
                isUseSpriteMaskInStep = true;
                spriteMask.SetUpdateTexture(true);
                curStepToolOb = curStep.stepTool;

                ChangeMasksOb(curStep, () => { CallBackStep(true); });
                curPercentWinStep = curStep.percentStepWin;

                switch (curStepIndex)
                {
                    case 9:
                        // shadowDell1.gameObject.SetActive(true);
                        // shadowDell1.DOFade(1, 1f);
                        break;
                    case 13:
                        // shadowDell2.gameObject.SetActive(true);
                        // shadowDell2.DOFade(1, 1f);
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

        private PatternSelectionCleanStepController GetPatternCleanController(int stepIndex)
        {
            if (stepIndex == eyelinerStepIndex)
            {
                return eyelinerPatternStep;
            }

            if (stepIndex == lipStepIndex)
            {
                return lipPatternStep;
            }

            return null;
        }

        private void ClickStep(ref Action onEnter, ref Action onExecute, ref Action onExit)
        {
            ClickOb selectedOption = null;

            onEnter = () =>
            {
                isFinishStep = false;
                selectedOption = null;
                curBaseOb = null;

                if (headbandListBtn != null)
                {
                    headbandListBtn.SetActive(true);
                }

                if (clickStepOptionButtons != null)
                {
                    foreach (var option in clickStepOptionButtons)
                    {
                        if (option == null) continue;
                        option.Init(this);
                        option.SetCanUseWithCol(true);
                    }
                }

                if (nextStep1Btn != null)
                {
                    nextStep1Btn.Init(this);
                    nextStep1Btn.gameObject.SetActive(false);
                    nextStep1Btn.SetCanUseWithCol(false);
                }

                //step 0 click enter
                if (curStepIndex == 0)
                {
                    if (clickStepOptionButtons == null) return;
                    for (int i = 0; i < clickStepOptionButtons.Count; i++)
                    {
                        clickStepOptionButtons[i].SetVisualIcon(spritesIconStep1[i]);
                    }
                }
            };

            onExecute = () =>
            {
                if (isFinishStep) return;
                if (!Input.GetMouseButtonUp(0)) return;
                if (curBaseOb == null) return;

                var clickedOb = curBaseOb as ClickOb;
                curBaseOb = null;
                if (clickedOb == null) return;

                if (nextStep1Btn != null && clickedOb == nextStep1Btn)
                {
                    if (selectedOption == null) return;

                    isFinishStep = true;
                    SetPercentStep(1f);
                    DOVirtual.DelayedCall(0.1f, CheckWinOrNextState);
                    return;
                }

                if (clickStepOptionButtons == null || !clickStepOptionButtons.Contains(clickedOb)) return;

                selectedOption = clickedOb;
                ApplyStepOptionVisual(clickedOb);
                SetPercentStep(0.5f);
                if (nextStep1Btn != null)
                {
                    nextStep1Btn.gameObject.SetActive(true);
                    nextStep1Btn.SetCanUseWithCol(true);
                }
            };

            onExit = () =>
            {
                if (nextStep1Btn != null)
                {
                    nextStep1Btn.gameObject.SetActive(false);
                    nextStep1Btn.SetCanUseWithCol(false);
                }
                if (headbandListBtn != null)
                {
                    headbandListBtn.SetActive(false);
                }
            };
        }

        private void ApplyStepOptionVisual(ClickOb clickedOb)
        {
            if (clickedOb == null || clickStepOptionButtons == null) return;
            
            foreach (var option in clickStepOptionButtons)
                option.ResetVisual();
            clickedOb.SetPressVisual();
            
            int optionIndex = clickStepOptionButtons.IndexOf(clickedOb);
            if (optionIndex < 0) return;

            List<GameObject> stepVisuals = GetClickStepVisualObjects(curStepIndex);
            if (stepVisuals == null || stepVisuals.Count == 0) return;

            for (int i = 0; i < stepVisuals.Count; i++)
            {
                if (stepVisuals[i] != null)
                {
                    stepVisuals[i].SetActive(i == optionIndex);
                }
            }
        }

        private List<GameObject> GetClickStepVisualObjects(int stepIndex)
        {
            if (stepIndex == _spritesIconStep1Index)
            {
                return listFaceTowel;
            }

            if (stepIndex == _spritesIconStep2Index)
            {
                return listOb2;
            }

            return null;
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
                        maskOb.Init(this);
                        step1Ob.SetActive(true);
                        maskOb.gameObject.SetActive(true);
                        break;
                    case 2:
                        DOVirtual.DelayedCall(0.2f, () =>
                        {
                            HandleListArrange(curLiAs);
                            // DoFadeAllSpriteRenderers(scene1, 0f, 0.5f, () =>
                            // {
                            //     MoveDownAndFadeInSequential(listObAppear1, 0.5f, true, 0.2f, () =>
                            //     {
                            //         oriPosBowlBlue = bowlBlue.transform.position;
                            //         oriPosBowlBrown = bowlBrown.transform.position;
                            //     });
                            // });
                        });
                        break;
                    case 5:
                        DOVirtual.DelayedCall(1f, () =>
                        {
                            // MoveDownAndFadeInSequential(listObAppear2, 0.5f, false, 0.2f, () =>
                            // {
                            //     MoveDownAndFadeInSequential(listObAppear3, 0.5f, true, 0.2f);
                            //     curLiAs = siftersFlour.ToList();
                            //     HandleListArrange(curLiAs);
                            // });
                        });
                        break;

                }
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
                    ob?.MoveInMultiplePlaces();
                    SetPercentStep(1);
                    curBaseOb = null;
                }
                else
                {
                    if (ob != null) ob.MoveBack();
                    curBaseOb = null;
                    return;
                }
                isFinishStep = true;
                DOVirtual.DelayedCall(0.2f, CheckWinOrNextState);
                if (curStepIndex == 1)
                {
                    step1Ob.SetActive(false);
                }
            };

            onExit = () =>
            {
            
            };
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
}
