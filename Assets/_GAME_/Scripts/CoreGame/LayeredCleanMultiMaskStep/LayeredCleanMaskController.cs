using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LayeredCleanMaskLayer
{
    public LayeredCleanBrushType brushType;
    public HandleSpriteMask maskHandler;
    public GameObject layerRoot;
    public float percentRequired = 80f;
    [HideInInspector] public bool isCompleted;

    public float GetProgress()
    {
        if (maskHandler == null) return 0f;
        if (percentRequired <= 0f) return 1f;
        return Mathf.Clamp01(maskHandler.percentDelete / percentRequired);
    }
}

public class LayeredCleanMaskController : MonoBehaviour
{
    [SerializeField] private LevelStateCtrl lvCtrl;
    [SerializeField] private List<LayeredCleanMaskLayer> layers = new();
    [SerializeField] private bool hideAllOnStart = true;
    [SerializeField] private bool resetMasksOnInit = true;

    private LayeredCleanBrushTool _currentBrush;
    private bool _isInitialized;

    public LayeredCleanBrushTool CurrentBrush => _currentBrush;

    public void Init(LevelStateCtrl levelCtrl = null)
    {
        if (levelCtrl != null)
        {
            lvCtrl = levelCtrl;
        }

        foreach (var layer in layers)
        {
            if (layer.maskHandler == null) continue;

            layer.maskHandler.Init(lvCtrl);
            if (resetMasksOnInit)
            {
                layer.maskHandler.RefillTextureMask();
                layer.maskHandler.ResetPercentDelete();
            }

            layer.isCompleted = false;
            SetLayerVisible(layer, !hideAllOnStart && layer.brushType == LayeredCleanBrushType.None);
        }

        _currentBrush = null;
        _isInitialized = true;
    }

    public void SelectBrush(LayeredCleanBrushTool brush)
    {
        if (!_isInitialized)
        {
            Init(lvCtrl);
        }

        _currentBrush = brush;
        RefreshLayerVisibility();

        if (brush == null)
        {
            return;
        }

        var layer = GetLayer(brush.BrushType);
        brush.spriteMask = layer?.maskHandler;
    }

    public void ClearBrush()
    {
        SelectBrush(null);
    }

    public LayeredCleanMaskLayer GetLayer(LayeredCleanBrushType brushType)
    {
        for (int i = 0; i < layers.Count; i++)
        {
            if (layers[i].brushType == brushType)
            {
                return layers[i];
            }
        }

        return null;
    }

    public float GetCurrentLayerProgress()
    {
        if (_currentBrush == null) return 0f;

        var layer = GetLayer(_currentBrush.BrushType);
        if (layer == null) return 0f;

        UpdateCompletedState(layer);
        return layer.GetProgress();
    }

    public float GetTotalProgress()
    {
        if (layers.Count == 0) return 0f;

        float total = 0f;
        int validLayerCount = 0;
        for (int i = 0; i < layers.Count; i++)
        {
            var layer = layers[i];
            if (layer.maskHandler == null) continue;

            UpdateCompletedState(layer);
            total += layer.GetProgress();
            validLayerCount++;
        }

        if (validLayerCount == 0) return 0f;
        return total / validLayerCount;
    }

    public bool IsCurrentLayerCompleted()
    {
        if (_currentBrush == null) return false;

        var layer = GetLayer(_currentBrush.BrushType);
        if (layer == null) return false;

        UpdateCompletedState(layer);
        return layer.isCompleted;
    }

    public bool IsAllLayersCompleted()
    {
        if (layers.Count == 0) return false;

        bool hasValidLayer = false;
        for (int i = 0; i < layers.Count; i++)
        {
            var layer = layers[i];
            if (layer.maskHandler == null) continue;

            hasValidLayer = true;
            UpdateCompletedState(layer);
            if (!layer.isCompleted)
            {
                return false;
            }
        }

        return hasValidLayer;
    }

    public void RefreshLayerVisibility()
    {
        for (int i = 0; i < layers.Count; i++)
        {
            var layer = layers[i];
            bool shouldShow = _currentBrush != null && layer.brushType == _currentBrush.BrushType;
            SetLayerVisible(layer, shouldShow);
        }
    }

    public void ResetAllLayers()
    {
        for (int i = 0; i < layers.Count; i++)
        {
            var layer = layers[i];
            if (layer.maskHandler != null)
            {
                layer.maskHandler.RefillTextureMask();
                layer.maskHandler.ResetPercentDelete();
            }

            layer.isCompleted = false;
            SetLayerVisible(layer, false);
        }

        _currentBrush = null;
    }

    private void SetLayerVisible(LayeredCleanMaskLayer layer, bool isVisible)
    {
        if (layer.layerRoot != null)
        {
            layer.layerRoot.SetActive(isVisible);
            return;
        }

        if (layer.maskHandler != null)
        {
            layer.maskHandler.gameObject.SetActive(isVisible);
        }
    }

    private void UpdateCompletedState(LayeredCleanMaskLayer layer)
    {
        if (layer.maskHandler == null) return;
        layer.isCompleted = layer.percentRequired <= 0f || layer.maskHandler.percentDelete >= layer.percentRequired;
    }
}
