using System;
using System.Collections.Generic;
using UnityEngine;

namespace _GAME_.Scripts.CoreGame
{
    [Serializable]
    public class PatternSelectionCleanLayer
    {
        public int optionId;
        public PatternOptionClickOb optionClick;
        public HandleSpriteMask maskHandler;
        public GameObject visualRoot;
        public float percentRequired = 80f;

        [HideInInspector] public bool isCompleted;

        public float GetProgress()
        {
            if (maskHandler == null) return 0f;
            if (percentRequired <= 0f) return 1f;
            return Mathf.Clamp01(maskHandler.percentDelete / percentRequired);
        }
    }

    public class PatternSelectionCleanStepController : MonoBehaviour
    {
        [SerializeField] private LevelStateCtrl lvCtrl;
        [SerializeField] private PatternCleanBrushTool brushTool;
        [SerializeField] private List<PatternSelectionCleanLayer> patternLayers = new();
        [SerializeField] private bool resetMasksOnEnter = true;
        [SerializeField] private bool hideUnselectedLayers = true;
        [SerializeField] private bool requireClickSelectFirst = true;

        private PatternSelectionCleanLayer _selectedLayer;
        private PatternSelectionCleanLayer _appliedLayer;
        private bool _isInitialized;
        private bool _isStepCompleted;

        public bool IsStepCompleted => _isStepCompleted;
        public bool HasSelection => _selectedLayer != null;
        public PatternSelectionCleanLayer SelectedLayer => _selectedLayer;

        public void Init(LevelStateCtrl levelCtrl = null)
        {
            if (levelCtrl != null)
            {
                lvCtrl = levelCtrl;
            }

            for (int i = 0; i < patternLayers.Count; i++)
            {
                var layer = patternLayers[i];
                if (layer.optionClick != null)
                {
                    layer.optionClick.Init(lvCtrl);
                    layer.optionClick.SetCanUseWithCol(true);
                    layer.optionClick.SetSelected(false);
                }

                if (layer.maskHandler != null)
                {
                    layer.maskHandler.Init(lvCtrl);
                }

                ResetLayerVisual(layer);
            }

            if (brushTool != null)
            {
                brushTool.SetCanUseWithCol(!requireClickSelectFirst);
            }

            _selectedLayer = null;
            _appliedLayer = null;
            _isStepCompleted = false;
            _isInitialized = true;
        }

        public void EnterStep(LevelStateCtrl levelCtrl = null)
        {
            Init(levelCtrl);
        }

        public void ExitStep()
        {
            if (brushTool != null)
            {
                brushTool.SetCanDrag(false);
                brushTool.spriteMask = null;
            }
        }

        public void OnBrushPicked(PatternCleanBrushTool brush)
        {
            brushTool = brush;
            if (_selectedLayer == null)
            {
                brush.spriteMask = null;
                brush.SetCanUseWithCol(!requireClickSelectFirst);
                return;
            }

            brush.spriteMask = _selectedLayer.maskHandler;
        }

        public void SelectOption(PatternOptionClickOb option)
        {
            if (!_isInitialized)
            {
                Init(lvCtrl);
            }

            if (option == null) return;

            var nextLayer = GetLayer(option.OptionId);
            if (nextLayer == null) return;

            bool hadCompletedAppliedLayer = _appliedLayer != null && _appliedLayer.isCompleted;
            bool isSwitchingLayer = _selectedLayer != null && _selectedLayer != nextLayer;

            if (isSwitchingLayer && !hadCompletedAppliedLayer)
            {
                ResetLayerVisual(_selectedLayer);
            }

            _selectedLayer = nextLayer;
            SetSelectedOptionFx(option);
            RefreshVisibleLayers();

            if (brushTool != null)
            {
                brushTool.spriteMask = nextLayer.maskHandler;
                brushTool.SetCanUseWithCol(true);
            }

            if (hadCompletedAppliedLayer && _appliedLayer != nextLayer)
            {
                SwapCompletedStyle(nextLayer);
            }
        }

        public void Tick()
        {
            if (_selectedLayer == null) return;

            _appliedLayer = _selectedLayer;
            float progress = _selectedLayer.GetProgress();
            _selectedLayer.isCompleted = progress >= 1f;
            _isStepCompleted = _selectedLayer.isCompleted;
        }

        public float GetCurrentProgress()
        {
            if (_selectedLayer == null) return 0f;
            return _selectedLayer.GetProgress();
        }

        public float GetSelectionProgress()
        {
            return _selectedLayer != null ? 1f : 0f;
        }

        public float GetCombinedProgress()
        {
            if (_selectedLayer == null) return 0f;
            return (GetSelectionProgress() + GetCurrentProgress()) * 0.5f;
        }

        private PatternSelectionCleanLayer GetLayer(int optionId)
        {
            for (int i = 0; i < patternLayers.Count; i++)
            {
                if (patternLayers[i].optionId == optionId)
                {
                    return patternLayers[i];
                }
            }

            return null;
        }

        private void SwapCompletedStyle(PatternSelectionCleanLayer nextLayer)
        {
            if (_appliedLayer != null && _appliedLayer.maskHandler != null)
            {
                _appliedLayer.maskHandler.HideFully();
                _appliedLayer.isCompleted = false;
            }

            if (nextLayer.maskHandler != null)
            {
                nextLayer.maskHandler.RevealFully();
            }

            nextLayer.isCompleted = true;
            _appliedLayer = nextLayer;
            _isStepCompleted = true;
        }

        private void SetSelectedOptionFx(PatternOptionClickOb currentOption)
        {
            for (int i = 0; i < patternLayers.Count; i++)
            {
                var option = patternLayers[i].optionClick;
                if (option != null)
                {
                    option.SetSelected(option == currentOption);
                }
            }
        }

        private void RefreshVisibleLayers()
        {
            for (int i = 0; i < patternLayers.Count; i++)
            {
                var layer = patternLayers[i];
                bool isVisible = !hideUnselectedLayers || layer == _selectedLayer;
                if (layer.visualRoot != null)
                {
                    layer.visualRoot.SetActive(isVisible);
                }
                else if (layer.maskHandler != null)
                {
                    layer.maskHandler.gameObject.SetActive(isVisible);
                }
            }
        }

        private void ResetLayerVisual(PatternSelectionCleanLayer layer)
        {
            if (layer == null) return;

            if (layer.maskHandler != null)
            {
                if (resetMasksOnEnter)
                {
                    layer.maskHandler.RefillTextureMask();
                }

                layer.maskHandler.ResetPercentDelete();
            }

            layer.isCompleted = false;
            if (layer.visualRoot != null)
            {
                layer.visualRoot.SetActive(false);
            }
        }
    }

    [Serializable]
    public class EyelinerCleanLayer : PatternSelectionCleanLayer
    {
    }
}