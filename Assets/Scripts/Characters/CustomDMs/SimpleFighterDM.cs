using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class SimpleFighterDM : DecisionMaker
{
    [SerializeField] private Speak _speachAction;
    [SerializeField] private AttackSelectedUnit _attackAction;
    private bool _initFightSpeachBubbleShown;
    public override void Init(Character character)
    {
    }
    public override void DecideBehaviour(CharacterAi character, Action<CharacterPlan> decisionProcessEnds)
    {
        if (TryShowInitialPhrase())
        {
            decisionProcessEnds(new CharacterPlan(_speachAction));
            return;
        }
        if(TryFindTargetToAttack(character))
        {
            decisionProcessEnds(new CharacterPlan(_attackAction));
            return;
        }

        decisionProcessEnds(null);
    }

    private bool TryShowInitialPhrase()
    {
        if(_initFightSpeachBubbleShown) return false;
        _initFightSpeachBubbleShown = true;
        _speachAction.Phrase = "Prepare to die!";
        return true;
    }
    private bool TryFindTargetToAttack(CharacterAi character)
    {
        _attackAction.Target = null;
        foreach (var charactersInSight in character.Memory.GetIEnumerableOfCharacters())
        {
            if (charactersInSight.Stats.IsDead) continue;

            if (charactersInSight.name != character.name)
            {
                _attackAction.Target = charactersInSight;
                break;
            }
        }
        return _attackAction.Target != null;
    }
}
