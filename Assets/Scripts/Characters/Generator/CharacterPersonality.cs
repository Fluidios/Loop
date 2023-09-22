using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterPersonality
{
    public string Name;
    public string[] ArchetypeDependenciesNames;
    public int[] ArchetypeDependenciesValues;

    public string[] PersonalityValuesNames;
    public int[] PersonalityValuesCurrentValues;

    public string CurrentLocation;

    public KnownCharacter[] KnownCharacters;

    public CharacterPersonality() { }
    public CharacterPersonality(string name)
    {
        Name = name;
        ArchetypeDependenciesNames = new string[0];
        ArchetypeDependenciesValues = new int[0];
        PersonalityValuesNames = new string[0];
        PersonalityValuesCurrentValues = new int[0];
        KnownCharacters = new KnownCharacter[0];
    }
}
