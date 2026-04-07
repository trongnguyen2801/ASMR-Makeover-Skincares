using System;
using System.Collections.Generic;
using UnityEngine;

public class BaseLevelCtrl : MonoBehaviour
{
    public LevelStateCtrl CurInstanceStateLv;
    public List<LevelStateCtrl> levelCtrlStates = new List<LevelStateCtrl>();
    protected int curStateIndex = 0;

    public virtual void Init()
    {
        curStateIndex = 0;
        int levelStateCount = levelCtrlStates.Count;
        if (levelStateCount == 0)
        {
            Debug.LogError("LevelCtrlState is empty, please add level states to the list.");
            return;
        }

        levelCtrlStates[levelStateCount - 1].isLastState = true;
        CurInstanceStateLv = levelCtrlStates[curStateIndex];
        CurInstanceStateLv.Init(this);
    }

    public virtual void OnChangeNextState()
    {
        curStateIndex++;
        if (curStateIndex >= levelCtrlStates.Count)
        {
            Debug.LogWarning("No more states to change to.");
            return;
        }
        CurInstanceStateLv = levelCtrlStates[curStateIndex];
        CurInstanceStateLv.Init(this);
    }
    public virtual void ChangeState(Action<LevelStateCtrl, LevelStateCtrl> changeStateAction)
    {
        if (changeStateAction == null) return;
        if (CurInstanceStateLv == null) return;
        if (curStateIndex + 1 >= levelCtrlStates.Count)
        {
            Debug.LogWarning("No next state available.");
            return;
        }

        var curState = CurInstanceStateLv;
        var nextState = levelCtrlStates[curStateIndex + 1];
        changeStateAction?.Invoke(curState, nextState);
    }
}
