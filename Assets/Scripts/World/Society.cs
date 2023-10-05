using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Society : GameSystem
{
    public override bool AsyncInitialization => true;
    [SerializeField] private int _totalWorldPopulation = 10;
    [SerializeField] GeneratorPreset _charactersGeneratorPreset;
    private CharactePersonalitiesList _personalitiesList;
    private Dictionary<string, CharacterPersonality> _personalities = new Dictionary<string, CharacterPersonality>();

    [SerializeField, ReadOnly] private List<string> _personalitiesInUse = new List<string>();

    public override void Initialize(Action initializationEndedCallback)
    {
        StartCoroutine(LoadSociety(initializationEndedCallback));
    }

    private IEnumerator LoadSociety(Action initializationEndedCallback)
    {
        _personalitiesList = CharactePersonalitiesList.Load();
        if (_personalitiesList[0] == CharactePersonalitiesList.c_defaultPersonality)
        {
            _personalitiesList.Remove(CharactePersonalitiesList.c_defaultPersonality);
            yield return GenerateSociety();
            Debug.Log("Generate");
        }
        else
        {
            yield return LoadSocietyData();
            Debug.Log("Load");
        }
        if(initializationEndedCallback != null)
            initializationEndedCallback();
    }
    private IEnumerator GenerateSociety()
    {
        LocationsLibrary locationsLibrary = SystemsManager.GetSystemOfType<LocationsLibrary>();
        var tableByPopulationRating = GenerateLootTableOfLocations(locationsLibrary, out LocationReference mostPopulatedLocation);
        LocationReference locationReference;
        CharacterPersonality personality;
        Map map = SystemsManager.GetSystemOfType<Map>();
        for (int i = 0; i < _totalWorldPopulation; i++)
        {
            if (!TryChooseLocationReference(tableByPopulationRating, mostPopulatedLocation, out locationReference)) break;
            if (!map.HasInstancesOfLocation(locationReference)) locationReference = locationsLibrary.GetLocation(map.DefaultLocation).Reference;
            personality = GeneratePersonality(locationReference, map);
            _personalities.Add(personality.Name, personality);
            _personalitiesList.AddNewPersonality(personality, false);
            yield return null;
        }
        CharactePersonalitiesList.Save(_personalitiesList);
        if (CharactePersonalitiesList.WasUpdated != null)
            CharactePersonalitiesList.WasUpdated(this);
    }
    private LootTable<LocationReference> GenerateLootTableOfLocations(LocationsLibrary locationsLibrary, out LocationReference mostPopulatedLocation)
    {
        var table = new LootTable<LocationReference>();
        int maxPopulation = int.MinValue;
        mostPopulatedLocation = null;
        foreach (Location location in locationsLibrary)
        {
            table.AddPossibleLoot(location.Reference, (float)location.Reference.PopulationRating / locationsLibrary.TotalPopulationRating);
            if(maxPopulation < location.Reference.PopulationRating)
            {
                mostPopulatedLocation = location.Reference;
                maxPopulation = location.Reference.PopulationRating;
            }    
        }
        return table;
    }
    private bool TryChooseLocationReference(LootTable<LocationReference> tableByPopulation, LocationReference mostPopulatedLocation, out LocationReference result)
    {
        if (tableByPopulation.GetLoot(out result) == LootTable<LocationReference>.LootRollResult.DroppedLessThanRequested)
        {
            if (mostPopulatedLocation != null)
            {
                result = mostPopulatedLocation;
            }
            else
            {
                Debug.LogError("Empty table and null most populated location. Are there any location in the world?");
                return false;
            }
        }
        return true;
    }
    private CharacterPersonality GeneratePersonality(LocationReference locationReference, Map map)
    {
        if(locationReference.InhabitingArchetypes.GetLoot(out MixedArchetype archetype) == LootTable<MixedArchetype>.LootRollResult.DroppedLessThanRequested)
        {
            archetype = locationReference.InhabitingArchetypes.GetFirst();
        }
        //TODO: determine if character should be populated / anchored
        return PersonalityGenerator.GenerateNew(_charactersGeneratorPreset, archetype, _personalitiesList, locationReference.name);
    }
    private IEnumerator LoadSocietyData()
    {
        foreach (var personalityName in _personalitiesList.SavedPersonalities)
        {
            _personalities.Add(personalityName, _personalitiesList.LoadPersonality(personalityName));
            yield return null;
        }
    }

    internal CharacterPersonality GetFreePersonality()
    {
        var freePersonalities = _personalities.Keys.Except(_personalitiesInUse).ToList();
        int r = freePersonalities.Count > 0 ? UnityEngine.Random.Range(0, freePersonalities.Count) : -1;
        _personalitiesInUse.Add(freePersonalities[r]);
        return _personalities[freePersonalities[r]];
    }

    internal void ReleasePersonality(CharacterPersonality myPersonality)
    {
        _personalitiesInUse.Remove(myPersonality.Name);
        _personalitiesList.SavePersonality(myPersonality);
    }
    internal CharacterPersonality GetFirstPersonality()
    {
        string name = _personalities.First().Key;
        if (_personalitiesInUse.Contains(name))
        {
            Debug.LogError(name + " already in use by someone");
            return GetFreePersonality();
        }
        else
        {
            _personalitiesInUse.Add(name);
            return _personalities[name];
        }
    }
    internal CharacterPersonality GetPersonality(string attachedPersonality)
    {
        if (_personalitiesInUse.Contains(attachedPersonality))
        {
            Debug.LogError(attachedPersonality + " already in use by someone");
            return GetFreePersonality();
        }
        else
        {
            _personalitiesInUse.Add(attachedPersonality);
            Debug.Log(string.Format("!({0})", attachedPersonality));
            foreach (var item in _personalities)
            {
                Debug.Log(string.Format("({0})",item.Key));
            }
            return _personalities[attachedPersonality];
        }
    }
}
