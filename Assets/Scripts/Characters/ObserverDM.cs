using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObserverDM : DecisionMaker
{
    [SerializeField] private DebugSpeakCharacterAction _speakAction;
    public override void Init(Character character) { }
    public override CharacterPlan DecideBehaviour(Character character)
    {
        List<CharacterActionLogic> actions = new List<CharacterActionLogic>();
        foreach (var item in character.Memory.GetIEnumerableOfCharacters())
        {
            var action = new DebugSpeakCharacterAction(_speakAction);
            action.Phrase = string.Format("I see {0}", item.name);
            actions.Add(action);
        }
        return new CharacterPlan(actions);
    }
}
