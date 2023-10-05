using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Location Reference")]
public class LocationReference : ScriptableObject
{
    [SerializeField] private int _populationRating;
    [SerializeField] private LootTable<MixedArchetype> _inhabitingArchetypes;
    public int PopulationRating => _populationRating;
    public LootTable<MixedArchetype> InhabitingArchetypes => _inhabitingArchetypes;
}
