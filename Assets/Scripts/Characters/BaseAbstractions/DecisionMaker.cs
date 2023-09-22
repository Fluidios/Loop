using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract class which goal to provide a plan of actions for a character, could implements any ai algorithms
/// </summary>
public abstract class DecisionMaker : MonoBehaviour
{
    public abstract void Init(Character character);
    /// <summary>
    /// Logic update, should provide a plan of actions based on character memory 
    /// </summary>
    /// <param name="character"></param>
    /// <returns></returns>
    public abstract void DecideBehaviour(CharacterAi character, Action<CharacterPlan> decisionProcessEnds);
}
