using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LootTable<T>
{
    [SerializeField] private List<LootElement<T>> _table;
    private System.Random _random;

    public T GetFirst() => _table[0].LootReference;

    private int RandomInt(int max)
    {
        if(_random == null)
            _random = new System.Random();
        return _random.Next(0, max);
    }

    public LootRollResult GetLoot(out T loot)
    {
        float rValue = RandomInt(100);
        List<LootElement<T>> possibleValues = new List<LootElement<T>>();
        foreach (var item in _table)
        {
            if (rValue <= item.DropChance)
                possibleValues.Add(item);
        }
        if (possibleValues.Count == 0)
        {
            loot = default(T);
            return LootRollResult.DroppedLessThanRequested;
        }
        else
        {
            int r = RandomInt(possibleValues.Count);
            loot = possibleValues[r].LootReference;
            if (possibleValues[r].CanBeDroppedOnlyOnce) _table.Remove(possibleValues[r]);
            return LootRollResult.Success;
        }
    }

    public LootRollResult GetLoot(int amount, out List<T> loot)
    {
        LootRollResult rollResult = LootRollResult.Success;
        loot = new List<T>();
        T singleLootElement;
        for (int i = 0; i < amount; i++)
        {
            if (GetLoot(out singleLootElement) == LootRollResult.DroppedLessThanRequested)
            {
                rollResult = LootRollResult.DroppedLessThanRequested;
            }
            else
            {
                loot.Add(singleLootElement);
            }
        }

        return rollResult;
    }

    public void AddPossibleLoot(T loot, float dropChance)
    {
        if (dropChance < 0 || dropChance > 100)
        {
            Debug.LogError("Strange loot drop chances found...");
            return;
        }
        if(_table == null) _table = new List<LootElement<T>>();
       _table.Add(new LootElement<T> { LootReference = loot, DropChance = dropChance });
    }
    public int Length
    {
        get { return _table.Count; }
    }
    public void ProvideSpecificRandomnessSeed(int seed)
    {
        _random = new System.Random(seed);
    }
    public enum LootRollResult
    {
        Success, DroppedLessThanRequested
    }
}

[System.Serializable]
public class LootElement<T>
{
    public T LootReference;
    public bool CanBeDroppedOnlyOnce = false;
    [Range(0.1f, 100f)]public float DropChance = 0.1f;
}
