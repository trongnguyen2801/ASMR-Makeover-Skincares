using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickOb : BaseOb
{
    public bool isCompleted;
    
    public Sprite baseSprite;
    public Sprite pressSprite;
    public SpriteRenderer spriteRenderer;
    public SpriteRenderer iconRenderer;
        
    protected override void OnMouseDown()
    {
        if (GameManager.GameState != GameState.Playing) return;
        if (lvCtrl != null) lvCtrl.curBaseOb = this;
        if (spriteRenderer != null) spriteRenderer.sprite = pressSprite;
        // AudioManager.Instance.PlaySFX(pickUpSound);
    }

    public void ResetVisual()
    {
        if (spriteRenderer != null) spriteRenderer.sprite = baseSprite;
    }
    
    public void SetPressVisual()
    {
        if (spriteRenderer != null) spriteRenderer.sprite = pressSprite;
    }

    public void SetVisualIcon(Sprite iconVisual)
    {
        if (iconRenderer != null) iconRenderer.sprite = iconVisual;
    }
    protected override void OnMouseUp()
    {
    }
    protected override void OnMouseDrag()
    {
    }
}
