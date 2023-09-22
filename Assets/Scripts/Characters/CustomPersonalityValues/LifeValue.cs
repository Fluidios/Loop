using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/PersonalityValues/LifeValue")]
public class LifeValue : PersonalityValue
{
    public override int GetValue(Character character)
    {
        return Mathf.RoundToInt(character.Stats.Health.Value/character.Stats.Health.MaxTValue*100);
    }
}
