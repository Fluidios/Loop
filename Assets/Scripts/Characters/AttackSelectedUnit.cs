using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSelectedUnit : CharacterActionLogic, IDemandTarget<Character>
{
    [SerializeField] private MeetingEventSide _searchForTargetAtSide; 
    private Character _target;
    public MeetingEventSide WhichSideToSearchForTarget { get { return _searchForTargetAtSide; } }
    public Character Target { get { return _target; } set { _target = value; } }
    public override bool InstantAction => false;

    public override bool ExecuteAction(Character character, Action executionEndsCallback = null)
    {
        StartCoroutine(Hit(executionEndsCallback));
        return true;
    }

    IEnumerator Hit(Action executionEndsCallback)
    {
        yield return new WaitForSeconds(1);
        executionEndsCallback();
    }
}
