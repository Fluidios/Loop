using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiDependentDM : DecisionMaker
{
    [SerializeField] private CharacterCombatUi _uiPrefab;
    [SerializeField] private CharacterActionLogic[] _availableActions;
    private EventsVisualizer _visualizer;
    Action<CharacterPlan> _planDecidedCallback;
    private CharacterActionLogic _selectedAction;
    private IDemandTarget<Character> _selectedActionAsInteface;
    private CharacterCombatUi _ui;
    public override void Init(Character character)
    {
        _visualizer = SystemsManager.GetSystemOfType<EventsVisualizer>();
        _ui = _visualizer.AddUI(_uiPrefab);
        _ui.Init(character.name, _availableActions, ConfirmAction);
        _ui.gameObject.SetActive(false);
    }
    public override void DecideBehaviour(Character character, Action<CharacterPlan> decisionProcessEnds)
    {
        _planDecidedCallback = null;
        _planDecidedCallback = decisionProcessEnds;
        _planDecidedCallback += (plan) => _ui.gameObject.SetActive(false);
        _ui.gameObject.SetActive(true);
    }
    private void ConfirmAction(CharacterActionLogic selectedAction)
    {
        Debug.Log(selectedAction.Name + " - selected.");
        _selectedActionAsInteface = selectedAction as IDemandTarget<Character>;
        if (_selectedActionAsInteface != null)
        {
            _selectedAction = selectedAction;
            if (_visualizer.ActiveEventGraphics != null)
            {
                var meetingEventVisual = _visualizer.ActiveEventGraphics as MeetingEventGraphics;
                if (meetingEventVisual != null)
                {
                    meetingEventVisual.StartTargetCharacterSelection(_selectedActionAsInteface.WhichSideToSearchForTarget, ConfirmTargetCharacter);
                }
                else
                    throw new Exception("Active event graphics is not a meeting event graphics!");
            }
            else
                throw new Exception("Active event graphics is null! Cant select action target.");
        }
        else
            _planDecidedCallback(new CharacterPlan(selectedAction));
    }
    private void ConfirmTargetCharacter(Character selectedCharacter)
    {
        Debug.Log("Target for action " +_selectedAction.Name + " - selected: "+ selectedCharacter.name);
        _selectedActionAsInteface.Target = selectedCharacter;
        _planDecidedCallback(new CharacterPlan(_selectedAction));
    }
}
