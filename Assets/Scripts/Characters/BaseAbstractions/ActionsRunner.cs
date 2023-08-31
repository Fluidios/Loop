using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sequently execute all actions in the provided plan
/// </summary>
public class ActionsRunner
{
    /// <summary>
    /// served character link
    /// </summary>
    private Character _character;
    private CharacterPlan _currentlyRunningPlan;
    private Action _planExecutionEndsCallback;
    /// <summary>
    /// Init runner with character link
    /// </summary>
    /// <param name="character"></param>
    public void Init(Character character)
    {
        _character = character;
    }

    /// <summary>
    /// Execute actions from the plan one by one, if action failed ends execution imidiately
    /// </summary>
    /// <param name="plan"></param>
    /// <param name="executionEnds"></param>
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
    private void ExecuteAction(CharacterActionLogic action, Action executionEnds)
    {
        if (action.InstantAction)
        {
            if (action.ExecuteAction(_character, null))
                ContinuePlanExecution();
            else
                _planExecutionEndsCallback();
        }
        else
        {
            if (!action.ExecuteAction(_character, ContinuePlanExecution))
                _planExecutionEndsCallback();
        }

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
