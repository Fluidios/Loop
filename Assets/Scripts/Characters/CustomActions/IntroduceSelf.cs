using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroduceSelf : CharacterActionLogic
{
    [SerializeField] private Speak _speak;
    public override bool InstantAction => false;

    public override bool ExecuteAction(Character character, Action executionEndsCallback = null)
    {
        if(character.Ai.Memory.TryGetGeneric("Personality", out CharacterPersonality personality, null))
        {
            _speak.Phrase = "Hello, My name is " + personality.Name;
            _speak.ExecuteAction(character, executionEndsCallback);
            return true;
        }
        return false;
    }
}
