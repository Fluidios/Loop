using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speak : CharacterActionLogic
{
    /// <summary>
    /// describes how long speach buuble would be shown, to let player read it.
    /// </summary>
    private const float c_perCharacterBubbleLifetime = 0.1f;
    [SerializeField] private string _speakFormatter = "I see {0}";
    [SerializeField] private SpeachBuble _speachBublePrefab;
    private MeetingEventGraphics _eventGraphics;
    public override bool InstantAction => false;
    public string Phrase { get; set; }
    private void Start()
    {
        _eventGraphics = SystemsManager.GetSystemOfType<EventsVisualizer>().ActiveEventGraphics as MeetingEventGraphics;
        if (_eventGraphics == null) throw new Exception("Speak action failed to get link to current event graphics. Speach bubles would not been shown.");
    }

    public override bool ExecuteAction(Character character, Action executionEndsCallback = null)
    {
        if(_eventGraphics == null)
        {
            executionEndsCallback();
            return true;
        }
        var buble = character.Stats.BelongToPlayerTeam ? _eventGraphics.AddUiForPlayer(character, _speachBublePrefab) : _eventGraphics.AddUiForNPC(character, _speachBublePrefab);
        buble.Text = string.Format(_speakFormatter, Phrase);
        executionEndsCallback += () =>
        {
            Destroy(buble.gameObject);
        };
        StartCoroutine(ShowBubble(executionEndsCallback));
        return true;
    }

    IEnumerator ShowBubble(Action executionEndsCallback)
    {
        yield return new WaitForSeconds(c_perCharacterBubbleLifetime * Phrase.Length);
        executionEndsCallback();
    }
}
