using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ActionsCollection
{
    [SerializeField] private ActionComponent[] _actions;

    public void Execute()
    {
        foreach (ActionComponent action in _actions)
        {
            action.Execute();
        }
    }
}
