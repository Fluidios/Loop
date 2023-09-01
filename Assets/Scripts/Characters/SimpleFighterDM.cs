using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFighterDM : DecisionMaker
{
    [SerializeField] private AttackSelectedUnit _attackAction;
    public override void Init(Character character)
    {
    }
    public override void DecideBehaviour(Character character, Action<CharacterPlan> decisionProcessEnds)
    {
        _attackAction.Target = null;
        foreach (var charactersInSight in character.Memory.GetIEnumerableOfCharacters())
        {
            if(charactersInSight.name != character.name)
            {
                _attackAction.Target = charactersInSight;
                break;
            }
        }
        if (_attackAction.Target != null)
        {
            decisionProcessEnds(new CharacterPlan(_attackAction));
        }
        else
        {
            decisionProcessEnds(null);
        }
    }
}
