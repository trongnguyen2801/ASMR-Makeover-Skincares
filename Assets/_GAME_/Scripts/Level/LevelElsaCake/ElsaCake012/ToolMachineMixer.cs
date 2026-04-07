using DG.Tweening;
using UnityEngine;

public class ToolMachineMixer : BaseTool
{
    public float timeSpentInArea;
    public float timeRequired = 2.5f;
    public Collider2D colBowl;

    public override void Init(LevelStateCtrl lvCtrl)
    {
        base.Init(lvCtrl);
        InitDefaultLayer();
    }

    public bool CheckCompleteSpraying()
    {
        return timeSpentInArea > timeRequired;
    }

    public float GetPercentStep()
    {
        return timeSpentInArea / timeRequired;
    }

    protected override void OnMouseDown()
    {
        base.OnMouseDown();
        ChangeAnim("ChangeImage");
        SetAllSprLayer();
    }

    public override void MoveBack()
    {
        ChangeAnim("Def");
        MoveWithSpeedCallback(transform, defaultPosition, () =>
        {
            ChangeAnim("Def");
            SetDefaultLayer();
        });
    }

    protected override void OnMouseUp()
    {
        base.OnMouseUp();
        ChangeAnim("Def");
        lvCtrl.isUsingCorrectTool = false;
        // AudioManager.Instance.StopSound(AudioManager.Instance.effectsSourceTool);
    }
}
