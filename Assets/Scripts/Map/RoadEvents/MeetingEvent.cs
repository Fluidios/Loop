using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeetingEvent : GraphicRoadEvent<MeetingEventGraphics>
{
    [SerializeField] private Character[] _eventNpcMembers;
    [SerializeField] private ExitEventCondition<MeetingEvent>[] _exitConditions;
    public HashSet<Character> EventNpcMembers { get; private set; }
    public HashSet<Character> EventPlayerSideMembers { get; private set; }
    private MeetingEventGraphics _myGraphics;
    /// <summary>
    /// all characters in event and also custom world of event since we dont handle anything except characters
    /// </summary>
    private Character[] _allCharacters;
    private int _currentCharacter;
    private WaitForSeconds _timerBetweenUnitsUpdate = new WaitForSeconds(1);

    protected override void InitializeGraphics(MeetingEventGraphics graphics)
    {
        List<Character> characters = new List<Character>();
        EventNpcMembers = graphics.SpawnNPCs(_eventNpcMembers);
        EventPlayerSideMembers = graphics.SpawnPlayerSquad(PlayerController.PlayerSquadPrefabs);
        
        foreach (Character character in EventPlayerSideMembers) { character.Stats.BelongToPlayerTeam = true; }

        characters.AddRange(EventNpcMembers);
        characters.AddRange(EventPlayerSideMembers);

        _allCharacters = characters.ToArray();

        _myGraphics = graphics;

        StartCoroutine(DoWithDelay(1, UpdateEvent));
    }

    private void UpdateEvent()
    {
        if (EventPassed())
        {
            SystemsManager.GetSystemOfType<EventsVisualizer>().HideEventVisual(_myGraphics, () => Passed = true); 
        }
        else StartCoroutine(UpdateEventRoutine());
    }
    private bool EventPassed()
    {
        foreach (var condition in _exitConditions)
        {
            if(condition.CheckCondition(this))
            {
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// for the saftey reason, since all characters could have only imidiate actions, which would cause endless loop of updates in one frame
    /// </summary>
    /// <returns></returns>
    IEnumerator UpdateEventRoutine()
    {
        yield return _timerBetweenUnitsUpdate;
        _allCharacters[_currentCharacter].Ai.UpdateLogic(UpdateEvent, _allCharacters);
        _currentCharacter = (_currentCharacter + 1) % _allCharacters.Length;
    }
    IEnumerator DoWithDelay(float delay, Action action)
    {
        yield return new WaitForSeconds(delay);
        action();
    }
}
