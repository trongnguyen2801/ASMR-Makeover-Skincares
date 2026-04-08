using UnityEngine;

public class CleanTool : BaseTool
{
    public Vector2 offsetSpawn;
    public int erSize = 56;
    public HandleSpriteMask spriteMask;
    [SerializeField] private Transform cleanPoint;
    [SerializeField] private Collider2D cleanPointCollider;
    [SerializeField] private float cleanPointPadding = 0.02f;

    public bool TryGetCleanWorldPosition(out Vector2 worldPosition)
    {
        if (cleanPoint != null)
        {
            worldPosition = cleanPoint.position;
            return true;
        }

        if (HasActiveCleanPointCollider())
        {
            worldPosition = cleanPointCollider.bounds.center;
            return true;
        }

        Vector2 offSetTool = new(offsetSpawn.x * factorX, offsetSpawn.y);
        worldPosition = (Vector2)CachedTransform.position + offSetTool;
        return true;
    }

    public bool IsCleanPointOverlapping(Collider2D targetCollider)
    {
        if (targetCollider == null) return false;

        if (HasActiveCleanPointCollider())
        {
            ColliderDistance2D distance = cleanPointCollider.Distance(targetCollider);
            return distance.isOverlapped || distance.distance <= cleanPointPadding;
        }

        return TryGetCleanWorldPosition(out var worldPosition) && targetCollider.OverlapPoint(worldPosition);
    }

    private bool HasActiveCleanPointCollider()
    {
        return cleanPointCollider != null && cleanPointCollider.enabled && cleanPointCollider.gameObject.activeInHierarchy;
    }

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
