using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneSideIsDead : ExitEventCondition<MeetingEvent> 
{
    public override bool CheckCondition(MeetingEvent eventToCheck)
    {
        bool sideDied = true;
        foreach (var item in eventToCheck.EventNpcMembers)
        {
            if(!item.Stats.IsDead)
            {
                sideDied = false;
                break;
            }
        }
        if(sideDied) return true;
        
        sideDied = true;
        foreach (var item in eventToCheck.EventPlayerSideMembers)
        {
            if(!item.Stats.IsDead)
            {
                sideDied = false;
                break;
            }
        }
        return sideDied;
    }
}
