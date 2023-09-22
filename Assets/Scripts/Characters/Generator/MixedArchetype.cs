using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixedArchetype : ScriptableObject
{
    private static string[] _basicArchetypes;
    public static string[] BasicArchetypes
    {
        get
        {
            if(_basicArchetypes == null)
                _basicArchetypes = new string[12]
                {
                    "Joker",
                    "Lover",
                    "Friend",
                    "Keeper",
                    "King",
                    "Creator",
                    "Optimist",
                    "Scientist", 
                    "Travaler",
                    "Hero",
                    "Inspirer",
                    "Rebel" 
                };
            return _basicArchetypes;
        }
    } 
    public int[] Dependency;
    public PersonalityValue[] PersonalityValues;
    public LocationReference[] Habitats;

    public MixedArchetype()
    {
        Dependency = new int[BasicArchetypes.Length];
        for (int i = 0; i < _basicArchetypes.Length; i++)
        {
            Dependency[i] = 50;
        }
    }

    public string[] GetHabitatNames()
    {
        var output = new string[Habitats.Length];
        for (int i = 0; i < Habitats.Length; i++)
        {
            if (Habitats[i])
                output[i] = Habitats[i].name;
            else
                output[i] = "NULL";
        }
        return output;
    }
} 
