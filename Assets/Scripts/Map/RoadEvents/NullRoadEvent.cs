using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Nothing's going to happen
/// </summary>
public class NullRoadEvent : RoadEvent
{
    private void Start()
    {
        Passed = true;
    }
}
