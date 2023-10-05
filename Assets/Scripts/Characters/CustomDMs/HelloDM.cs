using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HelloDM : DecisionMaker
{
    [SerializeField] private Speak _speachAction;
    [SerializeField] private Leave _leaveAction;
    private bool _helloMessageShown;
    private bool _strangerNameAsked;
    private CharacterPersonality _myPersonality;

    public override void Init(Character character)
    {
        if (!character.Ai.Memory.TryGetGeneric("Personality", out _myPersonality, null))
            Debug.LogError(gameObject.name + " failed to get its personality from memory");
    }
    public override void DecideBehaviour(CharacterAi character, Action<CharacterPlan> decisionProcessEnds)
    {
        if (TryShowHelloMessage(character))
        {
            if(_helloMessageShown)
                decisionProcessEnds(new CharacterPlan(new CharacterActionLogic[2] {_speachAction,_leaveAction}));
            else
                decisionProcessEnds(new CharacterPlan(_speachAction));
            return;
        }
        else
        {
            decisionProcessEnds(new CharacterPlan(_leaveAction));
        }
    }

    private bool TryShowHelloMessage(CharacterAi character)
    {
        if (_helloMessageShown) return false;

        CharacterPersonality strangerPersonality;
        if(TryFindPersonToSayHelloTo(character, out Character stranger))
        {
            if(stranger.Ai.Memory.TryGetGeneric("Personality", out strangerPersonality, null))
            {
                //TODO: If character A introduces himself by a name other than his own, personage B will not recognize him the next time they meet.
                //It is necessary to change the recognition mechanism to a more complex one.
                KnownCharacter memoriesAboutStranger = null;
                foreach (var item in _myPersonality.KnownCharacters)
                {
                    if (item.ReferenceName == strangerPersonality.Name)
                    {
                        memoriesAboutStranger = item;
                        break;
                    }
                }
                bool alreadySeenStrangerButDontKnowName = false;
                bool newMemoriesCreated = false;
                if (memoriesAboutStranger == null)
                {
                    memoriesAboutStranger = new KnownCharacter() { ReferenceName = strangerPersonality.Name };
                    newMemoriesCreated = true;
                }
                else if (memoriesAboutStranger.KnownName == string.Empty) alreadySeenStrangerButDontKnowName = true;

                if (!newMemoriesCreated && memoriesAboutStranger.KnownName != string.Empty) //find known character in memory with this name
                {
                    _speachAction.Phrase = "Hello " + memoriesAboutStranger.KnownName;
                    _helloMessageShown = true;
                }
                else
                {
                    //TODO:This is HACK; Rework informaton gathering. An auditory sensor is required
                    //Now we just get information from strangers brain
                    if (!_strangerNameAsked)
                    {
                        if(alreadySeenStrangerButDontKnowName)
                            _speachAction.Phrase = string.Format("I've seen you before, who are you?");
                        else
                            _speachAction.Phrase = string.Format("Hello who are you?");
                        _strangerNameAsked = true;
                    }
                    else
                    {
                        _speachAction.Phrase = string.Format("Nice to meet you. {0}", strangerPersonality.Name);
                        memoriesAboutStranger.KnownName = strangerPersonality.Name;
                        if(newMemoriesCreated)
                            _myPersonality.KnownCharacters = _myPersonality.KnownCharacters.Append(memoriesAboutStranger).ToArray();

                        _helloMessageShown = true;
                    }
                }
            }
            else
            {
                Debug.LogError(gameObject.name + " failed to get personality from " + stranger.name);
                _speachAction.Phrase = "I have nothing to talk with you.";
                _helloMessageShown = true;
            }
        }
        return true;
    }

    private bool TryFindPersonToSayHelloTo(CharacterAi speaker, out  Character sayHelloTo)
    {
        foreach (var charactersInSight in speaker.Memory.GetIEnumerableOfCharacters())
        {
            if (charactersInSight.Stats.IsDead) continue;

            if (charactersInSight.name != speaker.name)
            {
                sayHelloTo = charactersInSight;
                return true;
            }
        }
        sayHelloTo = null;
        return false; //no one to say hello to
    }
}
