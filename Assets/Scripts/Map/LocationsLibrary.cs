using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationsLibrary : GameSystem
{
    [SerializeField] private Location[] _library;
    Dictionary<string, Location> _dictionary;
    public override bool AsyncInitialization => false;
    public override void Initialize(Action initializationEndedCallback)
    {
        _dictionary = new Dictionary<string, Location>();

        foreach (var item in _library)
        {
            _dictionary.Add(item.Name, item);
        }
    }


    public Location GetLocation(string name)
    {
        if (_dictionary.TryGetValue(name, out var data))
            return data;

        Debug.LogError("CANT FIND LOCATION BY NAME: " + name);
        return null;
    }
}