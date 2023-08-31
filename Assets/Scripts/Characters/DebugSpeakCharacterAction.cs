using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DebugSpeakCharacterAction : CharacterActionLogic
{
    [SerializeField] private string _speakFormatter = "I see {0}";
    public string Phrase { get; set; }
    public void CopyFrom(DebugSpeakCharacterAction action)
    {
        _speakFormatter = action._speakFormatter;
    }
    public override bool InstantAction => true;
    public override bool ExecuteAction(Character character, Action executionEndsCallback = null)
    {
        Debug.Log(string.Format("{0}({1}):{2}",character.name, character.GetInstanceID(), Phrase));
        return true;
    }
}
