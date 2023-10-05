using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneSideIsEmpty : ExitEventCondition<MeetingEvent>
{
    public override bool CheckCondition(MeetingEvent eventToCheck)
    {
        if(eventToCheck.EventNpcMembers.Count == 0) return true;
        if(eventToCheck.EventPlayerSideMembers.Count == 0) return true;
        return false;
    }
}
