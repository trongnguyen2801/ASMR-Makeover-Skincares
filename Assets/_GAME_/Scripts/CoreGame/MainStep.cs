using System;
using UnityEngine;

public class MainStep
{
#if UNITY_EDITOR
    //cai nay de biet duoc no dang o state nao
    public string name;
#endif

    public delegate void StepAction(ref Action onEnter, ref Action onExecute, ref Action onExit);
    public Action onEnter, onExecute, onExit;

    public void Execute()
    {
        onExecute?.Invoke();
    }

    public void ChangeStep(StepAction stepAction)
    {
        if (stepAction == null) return;

        onExit?.Invoke();
        stepAction(ref onEnter, ref onExecute, ref onExit);

        onEnter?.Invoke();

#if UNITY_EDITOR
        //cai nay de biet duoc no dang o state nao
        name = stepAction.Method.Name;
#endif
    }
}

[System.Serializable]
public class BaseStep
{
    public StepType stepType;
    public BaseTool stepTool;
    public BaseOb stepRequiedOb;
    public MasksOb stepMasksOb;
    public int percentStepWin = 80;
    public ParticleSystem vfxEnd;
}

public enum StepType
{
    None,
    Clean,
    Arrange,
    Click,
    Open,
    Close,
    TurnOn,
    TurnOff,
    FillTime,
    PatternClean,
}
