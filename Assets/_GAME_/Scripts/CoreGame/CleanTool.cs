using UnityEngine;

public class CleanTool : BaseTool
{
    public Vector2 offsetSpawn;
    public int erSize = 56;
    public HandleSpriteMask spriteMask;
    protected override void OnMouseDown()
    {
        base.OnMouseDown();
        SetAllSprLayer();
        ChangeAnim("using");
    }
    protected override void OnMouseUp()
    {
        base.OnMouseUp();
        BackDefaultState();
        ChangeAnim("default");
        // AudioManager.Instance.StopSound(AudioManager.Instance.effectsSourceTool);
    }
    protected override void OnMouseDrag()
    {
        base.OnMouseDrag();
        if (spriteMask == null || anim == null) return;

        anim.speed = spriteMask.drawing ? 1f : 0f;
    }

    protected override void Update()
    {
        if (spriteMask == null)
        {
            base.Update();
            return;
        }

        if (spriteMask.drawing)
        {
            base.Update();
            return;
        }

        _timer = timeRepeatSound;
    }
}
