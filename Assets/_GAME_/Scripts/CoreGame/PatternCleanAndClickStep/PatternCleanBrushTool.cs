using UnityEngine;

namespace _GAME_.Scripts.CoreGame
{
    public class PatternCleanBrushTool : CleanTool
    {
        [SerializeField] private PatternSelectionCleanStepController stepController;

        public PatternSelectionCleanStepController StepController => stepController;

        public override void Init(LevelStateCtrl levelCtrl)
        {
            base.Init(levelCtrl);
            stepController?.Init(levelCtrl);
        }

        protected override void OnMouseDown()
        {
            stepController?.OnBrushPicked(this);
            base.OnMouseDown();
        }

        public override float GetPerCentInStep()
        {
            if (stepController == null)
            {
                return base.GetPerCentInStep();
            }

            return stepController.GetCurrentProgress();
        }
    }
}