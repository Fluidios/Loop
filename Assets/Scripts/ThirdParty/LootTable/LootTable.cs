using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LootTable<T>
{
    [SerializeField] private List<LootElement<T>> _table;
    private System.Random _random;

    private int RandomInt(int max)
    {
        if(_random == null)
            _random = new System.Random();
        return _random.Next(0, max);
    }

    public LootRollResult GetLoot(out T loot)
    {
        float rValue = RandomInt(100);
        List<T> possibleValues = new List<T>();
        foreach (var item in _table)
        {
            if (rValue <= item.DropChance)
                possibleValues.Add(item.LootReference);
        }
        if (possibleValues.Count == 0)
        {
            loot = default(T);
            return LootRollResult.DroppedLessThanRequested;
        }
        else
        {
            loot = possibleValues[RandomInt(possibleValues.Count)];
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
    [Range(0.1f, 100f)]public float DropChance = 0.1f;
}
