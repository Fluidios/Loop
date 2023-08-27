using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugAction : ActionComponent
{
    [SerializeField] private string _log;
    public override void Execute()
    {
        Debug.Log(_log, gameObject);
    }
}
