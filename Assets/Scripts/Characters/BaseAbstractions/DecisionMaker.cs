using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DecisionMaker : MonoBehaviour
{
    public abstract void Init(Character character);
    public abstract CharacterPlan DecideBehaviour(Character character);
}
