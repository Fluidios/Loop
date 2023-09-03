using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ExitEventCondition<T> : MonoBehaviour where T : RoadEvent
{
    public abstract bool CheckCondition(T eventToCheck);
}