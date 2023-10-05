using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leave : CharacterActionLogic
{
    public override bool InstantAction => true;

    public override bool ExecuteAction(Character character, Action executionEndsCallback = null)
    {
        LeaveCurrentEvent(character);
        return true;
    }

    private void LeaveCurrentEvent(Character character)
    {
        MeetingEvent meetingEvent = MeetingEvent.ActiveMeetingEvent;
        if(meetingEvent != null)
        {
            if(meetingEvent.EventNpcMembers.Contains(character))
            {
                meetingEvent.EventNpcMembers.Remove(character);
                character.Disable();
            }
            else if(meetingEvent.EventPlayerSideMembers.Contains(character))
            {
                meetingEvent.EventPlayerSideMembers.Remove(character);
                character.Disable();
            }
            else
            {
                Debug.LogError(string.Format("Event {0} does not contain {1}, but he wants to leave." +
                    " Are we had more than one opened event or not cleared characters?", meetingEvent.name, character.name));
            }
        }
        else
        {
            Debug.LogError(character.name + " failed to leave. No Active meeting event!");
        }
    }
}
