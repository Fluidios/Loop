using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterActionLogic
{
    public string Name
    {
        get { return this.GetType().Name; }
    }
    public abstract bool InstantAction { get; }
    public abstract void ExecuteAction(Character character, Action executionEndsCallback = null);
}
