using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObserverDM : DecisionMaker
{
    [SerializeField] private DebugSpeak _speakAction;
    public override void Init(Character character) { }
    public override void DecideBehaviour(CharacterAi character, Action<CharacterPlan> decisionProcessEnds)
    {
        List<CharacterActionLogic> actions = new List<CharacterActionLogic>();
        foreach (var item in character.Memory.GetIEnumerableOfCharacters())
        {
            var action = Instantiate(_speakAction, character.transform);
            action.CopyFrom(_speakAction);
            action.Phrase = string.Format("I see {0}", item.name);
            actions.Add(action);
        }
        decisionProcessEnds(new CharacterPlan(actions));
    }
}
