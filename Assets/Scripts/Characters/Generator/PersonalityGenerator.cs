using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PersonalityGenerator
{
    public static CharacterPersonality GenerateNew(GeneratorPreset generatorPreset, MixedArchetype archetype, CharactePersonalitiesList personalitiesList, string specificLocation = null)
    {
        //Name
        string name = generatorPreset.GetRandomName(personalitiesList.SavedPersonalities);

        //Archetype
        var personality = new CharacterPersonality(name);
        personality.ArchetypeDependenciesNames = MixedArchetype.BasicArchetypes;
        personality.ArchetypeDependenciesValues = new int[MixedArchetype.BasicArchetypes.Length];
        for (int i = 0; i < archetype.Dependency.Length; i++)
        {
            personality.ArchetypeDependenciesValues[i] =
                Math.Clamp(
                    value: archetype.Dependency[i] +
                                UnityEngine.Random.Range(-generatorPreset.GeneratedPersonalityVariationsRange, generatorPreset.GeneratedPersonalityVariationsRange),
                min: 0,
                max: 100
                );
        }

        //Values
        personality.PersonalityValuesNames = new string[archetype.PersonalityValues.Length];
        personality.PersonalityValuesCurrentValues = new int[archetype.PersonalityValues.Length];
        for (int i = 0; i < personality.PersonalityValuesNames.Length; i++)
        {
            personality.PersonalityValuesNames[i] = archetype.PersonalityValues[i].name;
            personality.PersonalityValuesCurrentValues[i] = UnityEngine.Random.Range(0, 100);
        }

        //Current location
        if (specificLocation != null)
        {
            personality.CurrentLocation = specificLocation;
        }
        else
        {
            if (archetype.Habitats.Length > 0)
            {
                personality.CurrentLocation = archetype.Habitats
                    [UnityEngine.Random.Range(0, archetype.Habitats.Length)].name;
                if (personality.CurrentLocation == "NULL")
                {
                    Debug.LogError("Can't generate character. Archetype contains null habitats.");
                    return null;
                }
            }
            else
            {
                var allLocationsOfTheWorld = generatorPreset.GetLocationsNames();
                if (allLocationsOfTheWorld.Length > 0)
                {
                    personality.CurrentLocation = allLocationsOfTheWorld[UnityEngine.Random.Range(0, allLocationsOfTheWorld.Length)];
                }
                else
                {
                    Debug.LogError("Can't generate character. No LoactionReferences in project to put character into.");
                    return null;
                }
            }
        }

        //Known characters
        List<KnownCharacter> knownCharacters = new List<KnownCharacter>();
        CharacterPersonality otherPersonality;
        foreach (var personalityName in personalitiesList.SavedPersonalities)
        {
            otherPersonality = personalitiesList.LoadPersonality(personalityName);
            if (UnityEngine.Random.value < generatorPreset.ChanceToKnowOtherCharacter)
            {
                knownCharacters.Add(new KnownCharacter()
                {
                    ReferenceName = personalityName,
                    KnownName = (UnityEngine.Random.value < generatorPreset.ChanceToKnowTheName) ? otherPersonality.Name : string.Empty,
                    ConnectedExperience = Mathf.RoundToInt((UnityEngine.Random.value * 2 - 1) * 100), //this should be smarter
                    Importance = Mathf.RoundToInt(UnityEngine.Random.value * 100),
                    LastKnownLocation = (UnityEngine.Random.value < generatorPreset.ChanceToKnowActualLocation) ? otherPersonality.CurrentLocation : string.Empty
                });
            }
        }
        personality.KnownCharacters = knownCharacters.ToArray();

        return personality;
    }
}
