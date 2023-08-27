using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionsRunner
{
    private Character _character;
    private CharacterPlan _currentlyRunningPlan;
    private Action _planExecutionEndsCallback;
    public void Init(Character character)
    {
        _character = character;
    }
    public void Execute(CharacterPlan plan, Action executionEnds)
    {
        _currentlyRunningPlan = plan;
        _planExecutionEndsCallback = executionEnds;
        if(_currentlyRunningPlan != null && _currentlyRunningPlan.Actions.Count > 0)
        {
            ExecuteAction(_currentlyRunningPlan.Actions[0], ContinuePlanExecution);
        }
        else
        {
            executionEnds();
        }
    }
    public void ExecuteAction(CharacterActionLogic action, Action executionEnds)
    {
        if (action.InstantAction)
        {
            action.ExecuteAction(_character, null);
            ContinuePlanExecution();
        }
        else
            action.ExecuteAction(_character, executionEnds);

    }

    private void ContinuePlanExecution()
    {
        _currentlyRunningPlan.Actions.RemoveAt(0);
        if(_currentlyRunningPlan.Actions.Count > 0)
        {
            ExecuteAction(_currentlyRunningPlan.Actions[0], ContinuePlanExecution);
        }
        else
        {
            _planExecutionEndsCallback.Invoke();
        }
    }
}
