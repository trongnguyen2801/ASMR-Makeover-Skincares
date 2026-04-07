using UnityEngine;

public class LayeredCleanBrushTool : CleanTool
{
    [SerializeField] private LayeredCleanBrushType brushType = LayeredCleanBrushType.Brush1;
    [SerializeField] private LayeredCleanMaskController layeredMaskController;

    public LayeredCleanBrushType BrushType => brushType;
    public LayeredCleanMaskController LayeredMaskController => layeredMaskController;

    public override void Init(LevelStateCtrl levelCtrl)
    {
        base.Init(levelCtrl);
        layeredMaskController?.Init(levelCtrl);
    }

    protected override void OnMouseDown()
    {
        layeredMaskController?.SelectBrush(this);
        base.OnMouseDown();
    }

    protected override void OnMouseUp()
    {
        base.OnMouseUp();
        spriteMask = layeredMaskController != null ? layeredMaskController.GetLayer(brushType)?.maskHandler : spriteMask;
    }

    public override float GetPerCentInStep()
    {
        if (layeredMaskController == null)
        {
            return base.GetPerCentInStep();
        }

        return layeredMaskController.GetTotalProgress();
    }
}
