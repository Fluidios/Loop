using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRoadEvent : RoadEvent
{

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Passed = true;
        }
    }
}
