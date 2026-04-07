using System;
using UnityEngine;

public class HandleSpriteMask : MonoBehaviour
{
    [SerializeField] private LevelStateCtrl _lvCtrl;
    private Color32[] maskColor;
    private Color32[] oriMaskColor;
    private Texture2D textureMask;
    private float sumOfPixel, sumOfOriColor, sumOfColor, sumOfOriTran, sumOfUpdateColor;
    private Vector2 lastPosMouse, worldPosMouse;
    private int wMask, hMask;
    private int skip = 30;
    private Vector2Int lastPosMask, PMask;
    public bool drawing;
    public bool isUpdateTexture = true;
    private BoxCollider2D col;
    private bool _isSetSprite;
    public float percentDelete;
    public SpriteMask spriteMask;
    public bool isTapeCleanMode;

    private void LateUpdate()
    {
        if (GameManager.GameState != GameState.Playing) return;
        if (!_isSetSprite) return;
        if (_lvCtrl == null || !_lvCtrl.isUseSpriteMaskInStep) return;

        if (isTapeCleanMode)
        {
            if (_lvCtrl.curToolOb != _lvCtrl.curStepToolOb)
            {
                drawing = false;
                return;
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (!TryGetCurrentCleanTool(out var tool, out worldPosMouse))
                {
                    drawing = false;
                    return;
                }

                if (!_lvCtrl.canCheckPercent)
                {
                    drawing = false;
                    return;
                }
                if (!col.OverlapPoint(worldPosMouse))
                {
                    drawing = false;
                    return;
                }

                lastPosMouse = worldPosMouse;
                UpdateTextureMask(worldPosMouse, tool.erSize);
                drawing = true;
            }
            else
            {
                drawing = false;
            }
            return;
        }

        if (!_lvCtrl.isUsingCorrectTool)
        {
            drawing = false;
            return;
        }
        if (Input.GetMouseButton(0))
        {
            if (!TryGetCurrentCleanTool(out var tool, out worldPosMouse))
            {
                drawing = false;
                return;
            }

            if (!_lvCtrl.canCheckPercent)
            {
                drawing = false;
                return;
            }
            if (col.OverlapPoint(worldPosMouse) && Vector2.Distance(lastPosMouse, worldPosMouse) > 0.01f)
            {
                lastPosMouse = worldPosMouse;
                UpdateTextureMask(worldPosMouse, tool.erSize);
                drawing = true;
            }
        }
        else
        {
            drawing = false;
        }
    }
    public Color32 GetColor(int index)
    {
        return maskColor[index];
    }

    private bool TryGetCurrentCleanTool(out CleanTool tool, out Vector2 toolWorldPosition)
    {
        tool = _lvCtrl.curStepToolOb as CleanTool;
        toolWorldPosition = default;
        if (tool == null || _lvCtrl.curStepToolOb == null || col == null) return false;

        Vector2 offSetTool = new(tool.offsetSpawn.x * tool.factorX, tool.offsetSpawn.y);
        toolWorldPosition = (Vector2)_lvCtrl.curStepToolOb.transform.position + offSetTool;
        return true;
    }

    private void SetAlpha0Color(int index)
    {
        maskColor[index].a = 0;
    }

    public void Init(LevelStateCtrl baseCustomLv, Sprite startSprite)
    {
        _lvCtrl = baseCustomLv;
        col ??= GetComponent<BoxCollider2D>();
        ResetSprite(startSprite);
    }

    public void Init(LevelStateCtrl levelCtrl)
    {
        _lvCtrl = levelCtrl;
        col ??= GetComponent<BoxCollider2D>();
    }

    public void ResetSprite(Sprite sprite)
    {
        if (spriteMask == null)
        {
            Debug.LogWarning("SpriteMask component is missing.");
            return;
        }

        col ??= GetComponent<BoxCollider2D>();
        maskColor = null;
        oriMaskColor = null;
        if (sprite == null)
        {
            Debug.Log(" Mask not found  Sprite have a sprite!");
            return;
        }
        Debug.Log("ReSetSpriteMask " + sprite.name);
        _isSetSprite = false;
        Texture2D texture = sprite.texture;
        textureMask = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, false);
        maskColor = texture.GetPixels32();
        textureMask.SetPixels32(maskColor);
        textureMask.Apply();
        oriMaskColor = (Color32[])maskColor.Clone();
        Debug.Log("Reset Mask Sprite with w= " + textureMask.width + "h " + textureMask.height);
        Rect spriteRect = new Rect(0, 0, textureMask.width, textureMask.height);
        Sprite newSprite = Sprite.Create(textureMask, spriteRect, new Vector2(0.5f, 0.5f));
        spriteMask.sprite = newSprite;
        wMask = textureMask.width;
        hMask = textureMask.height;
        if (col != null)
        {
            col.size = new Vector2(wMask / 90f, hMask / 90f);
        }

        CalInit(skip);
        ResetPercentDelete();
        _isSetSprite = true;
    }

    public float CalPercentWinLargeMask(float scaleMask)
    {
        float wSr = Screen.width, hSr = Screen.height;
        float percentNeededToWin = ((wSr * hSr) / skip - sumOfOriTran * scaleMask * scaleMask) / ((wMask * hMask * scaleMask * scaleMask / skip) - sumOfOriTran * scaleMask * scaleMask) * 100f;
        Debug.Log("percentLargeMask= " + percentNeededToWin + " scaleMask= " + scaleMask + " wSr= " + wSr + " hSr= " + hSr + " wM= " + wMask + " hM= " + hMask);
        return percentNeededToWin;
    }

    private void UpdateTextureMask(Vector2 worldPosMouse, int erSize)
    {
        if (spriteMask == null || spriteMask.sprite == null) return;

        Vector2 spriteSize = spriteMask.sprite.bounds.size;
        Vector2 worldPosMouseOffset = worldPosMouse;
        Vector2 localMouse = (Vector2)spriteMask.transform.InverseTransformPoint(worldPosMouseOffset) + new Vector2(spriteSize.x / 2, spriteSize.y / 2);
        localMouse.x *= wMask / spriteSize.x;
        localMouse.y *= hMask / spriteSize.y;
        PMask = new Vector2Int((int)localMouse.x, (int)localMouse.y);
        Vector2Int startMask = new Vector2Int();
        Vector2Int endMask = new Vector2Int();
        if (!drawing) lastPosMask = PMask;
        startMask.x = Mathf.Clamp(Mathf.Min(PMask.x, lastPosMask.x) - erSize, 0, wMask);
        startMask.y = Mathf.Clamp(Mathf.Min(PMask.y, lastPosMask.y) - erSize, 0, hMask);
        endMask.x = Mathf.Clamp(Mathf.Max(PMask.x, lastPosMask.x) + erSize, 0, wMask);
        endMask.y = Mathf.Clamp(Mathf.Max(PMask.y, lastPosMask.y) + erSize, 0, hMask);
        Vector2 dir = PMask - lastPosMask;
        float sqrDir = dir.sqrMagnitude;
        for (int x = startMask.x; x < endMask.x; x++)
        {
            for (int y = startMask.y; y < endMask.y; y++)
            {
                UpdateColor(x, y, drawing, dir, sqrDir, lastPosMask, PMask, erSize);
            }
        }
        lastPosMask = PMask;
        if (!isUpdateTexture) return;
        UpdateTextureMask();
    }

    public void SetUpdateTexture(bool isUpdate)
    {
        isUpdateTexture = isUpdate;
    }


    private void UpdateColor(int x, int y, bool drawing, Vector2 dir, float sqrDir, Vector2Int lastPosMask, Vector2Int PMask, int erSize)
    {
        if (maskColor == null) return;

        int index = x + y * wMask;
        if (maskColor[index].a < 10f) return;
        Vector2 pixel = new Vector2(x, y);
        Vector2 linePos = PMask;
        if (drawing)
        {
            float d = sqrDir > 0f ? Vector2.Dot(pixel - lastPosMask, dir) / sqrDir : 0f;
            d = Mathf.Clamp01(d);
            linePos = Vector2.Lerp(lastPosMask, PMask, d);
        }
        if ((pixel - linePos).sqrMagnitude <= erSize * erSize)
        {
            int alpha = maskColor[index].a;
            if (alpha > 10)
            {
                SetAlpha0Color(index);
                sumOfUpdateColor++;
                percentDelete = sumOfUpdateColor / sumOfOriColor * 100f;
            }
        }
    }

    private void UpdateTextureMask()
    {
        if (textureMask == null || maskColor == null) return;
        textureMask.SetPixels32(maskColor);
        textureMask.Apply();
    }

    public void RefillTextureMask()
    {
        if (oriMaskColor == null) return;
        if (textureMask == null || oriMaskColor?.Length == 0) return;
        int expectedSize = textureMask.width * textureMask.height;
        if (oriMaskColor.Length != expectedSize)
        {
            Debug.LogError("failSize RefillSpriteMask");
            return;
        }
        try
        {
            textureMask.SetPixels32(oriMaskColor);
            textureMask.Apply();
            maskColor = (Color32[])oriMaskColor.Clone();
            ResetPercentDelete();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    public void ResetPercentDelete()
    {
        sumOfUpdateColor = 0;
        percentDelete = 0;
    }

    public void RevealFully()
    {
        if (maskColor == null) return;

        for (int i = 0; i < maskColor.Length; i++)
        {
            maskColor[i].a = 0;
        }

        sumOfUpdateColor = sumOfOriColor;
        percentDelete = 100f;
        UpdateTextureMask();
    }

    public void HideFully()
    {
        RefillTextureMask();
    }

    private void CalInit(int skip)
    {
        sumOfPixel = wMask * hMask * 1f / skip;
        sumOfOriColor = 0;
        for (int i = 0; i < oriMaskColor.Length; i += skip)
        {
            if (oriMaskColor[i].a > 10)
            {
                sumOfOriColor++;
            }
        }
        sumOfOriTran = sumOfPixel - sumOfOriColor;
        sumOfOriColor *= 30;
        Debug.Log("sumOfPixel= " + sumOfPixel + " sumOfOriColor= " + sumOfOriColor + " sumOfOriTran= " + sumOfOriTran);
    }

    private float PercentDelete(int skip)
    {
        sumOfColor = 0;
        for (int i = 0; i < maskColor.Length; i += skip)
        {
            if (maskColor[i].a > 10)
            {
                sumOfColor++;
            }
        }
        float percent = (sumOfOriColor - sumOfColor) / sumOfOriColor * 100;
        Debug.Log("percent=== " + percent);
        return percent;
    }
    public void ResetSpriteNotCal(Sprite sprite)
    {
        if (spriteMask == null)
        {
            Debug.LogWarning("SpriteMask component is missing.");
            return;
        }

        col ??= GetComponent<BoxCollider2D>();
        if (sprite == null)
        {
            Debug.Log(" Mask not found Sprite have a sprite!");
            return;
        }
        Debug.Log("ReSetSpriteMask " + sprite.name);
        _isSetSprite = false;
        Texture2D texture = sprite.texture;
        textureMask = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, false);
        textureMask.SetPixels32(texture.GetPixels32());
        textureMask.Apply();
        maskColor = textureMask.GetPixels32();
        oriMaskColor = (Color32[])maskColor.Clone();
        Debug.Log("Reset Mask Sprite with w= " + textureMask.width + "h " + textureMask.height);
        Rect spriteRect = new Rect(0, 0, textureMask.width, textureMask.height);
        Sprite newSprite = Sprite.Create(textureMask, spriteRect, new Vector2(0.5f, 0.5f));
        spriteMask.sprite = newSprite;
        wMask = textureMask.width;
        hMask = textureMask.height;
        if (col != null)
        {
            col.size = new Vector2(wMask / 100f, hMask / 100f);
        }

        _isSetSprite = true;
    }
}
