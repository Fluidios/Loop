using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : CharacterActionLogic
{
    public override bool InstantAction => true;

    public override bool ExecuteAction(Character character, Action executionEndsCallback = null)
    {
        if(executionEndsCallback != null)
            executionEndsCallback();
        return true;
    }
}
