using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RoadEvent : MonoBehaviour
{
    private bool _passed = false;
    public bool Passed
    {
        get { return _passed; }
        set { _passed = value; if (value) PlayerController.WaitAndTryGoNextRoadTile(); }
    }
    public PlayerController PlayerController { get; set; }
}
