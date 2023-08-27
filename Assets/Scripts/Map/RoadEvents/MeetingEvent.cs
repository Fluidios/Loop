using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeetingEvent : GraphicRoadEvent<MeetingEventGraphics>
{
    [SerializeField] private Character[] _eventNpcMembers;
    public bool _eventPassed;
    /// <summary>
    /// all characters in event and also custom world of event since we dont handle anything except characters
    /// </summary>
    private Character[] _allCharacters;
    private int _currentCharacter;

    protected override void InitializeGraphics(MeetingEventGraphics graphics)
    {
        List<Character> characters = new List<Character>();
        characters.AddRange(graphics.SpawnNPCs(_eventNpcMembers));
        characters.AddRange(graphics.SpawnPlayerSquad(PlayerController.PlayerSquadPrefabs));

        _allCharacters = characters.ToArray();

        StartCoroutine(DoWithDelay(1.5f, UpdateEvent));
    }

    private void UpdateEvent()
    {
        if(_eventPassed) { Passed = true; }
        else StartCoroutine(UpdateEventRoutine());
    }
    /// <summary>
    /// for the saftey reason, since all characters could have only imidiate actions, which would cause endless loop of updates in one frame
    /// </summary>
    /// <returns></returns>
    IEnumerator UpdateEventRoutine()
    {
        yield return null;
        _allCharacters[_currentCharacter].UpdateLogic(UpdateEvent, _allCharacters);
        _currentCharacter = (_currentCharacter + 1) % _allCharacters.Length;
    }
    IEnumerator DoWithDelay(float delay, Action action)
    {
        yield return delay;
        action();
    }
}
