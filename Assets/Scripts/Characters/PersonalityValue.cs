using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Something wich is valuable for personality, should be described in high/low paradigm from 0 to 100
/// </summary>
public abstract class PersonalityValue : ScriptableObject
{
    public abstract int GetValue(Character character);
}
