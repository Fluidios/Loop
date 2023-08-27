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
        System.Type filterMemoriesBySensor = typeof(VisualSensor);
        if(character.Memory.HotMemoryData.TryGetValue(filterMemoriesBySensor, out Dictionary<string, MemoryNote> observablesList))
        {
            foreach (var item in observablesList)
            {
                var action = new DebugSpeakCharacterAction(_speakAction);
                action.Phrase = string.Format("I see {0}",item.Value.Type.Name);
                actions.Add(action);
            }
        }
        return new CharacterPlan(actions);
    }
}
