using _GAME_.Scripts.CoreGame;
using UnityEngine;

public class PatternOptionClickOb : ClickOb
{
    [SerializeField] private PatternSelectionCleanStepController stepController;
    [SerializeField] private int optionId;
    [SerializeField] private GameObject selectedFx;

    public int OptionId => optionId;

    public override void Init(LevelStateCtrl levelCtrl)
    {
        base.Init(levelCtrl);
        SetSelected(false);
    }

    protected override void OnMouseDown()
    {
        if (GameManager.GameState != GameState.Playing) return;
        stepController?.SelectOption(this);
    }

    public void SetSelected(bool isSelected)
    {
        if (selectedFx != null)
        {
            selectedFx.SetActive(isSelected);
        }
    }
}
