using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterActionLogic : MonoBehaviour
{
    public string Name
    {
        get { return this.GetType().Name; }
    }
    /// <summary>
    /// Does this action run in one frame or asyncroniosly?
    /// </summary>
    public abstract bool InstantAction { get; }
    /// <summary>
    /// Run an action
    /// </summary>
    /// <param name="character">character to run an action on</param>
    /// <param name="executionEndsCallback">would be provided if particular implementation runs asyncroniosly, should be called on the end of action </param>
    /// <returns></returns>
    public abstract bool ExecuteAction(Character character, Action executionEndsCallback = null);
}
