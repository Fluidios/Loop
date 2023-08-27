using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootTableExample : MonoBehaviour
{
    [SerializeField] private LootTable<string> _table;

    [ContextMenu("Get Loot")]
    public void GetLoot()
    {
        var respond = _table.GetLoot(out var loot);
        
        if(respond == LootTable<string>.LootRollResult.DroppedLessThanRequested)
        {
            Debug.Log("Bad luck table drops nothing...");
        }
        else
        {
            Debug.Log(loot);
        }
    }

    [ContextMenu("Get 10 loot items at once")]
    public void Get10Loot()
    {
        var respond = _table.GetLoot(10, out var loot);

        if (respond == LootTable<string>.LootRollResult.DroppedLessThanRequested)
        {
            if(loot.Count == 0)
                Debug.Log("Bad luck table drops nothing...");
            else
            {
                Debug.Log("Table drop less than requested. Small drop chances though...");
            }
        }
        foreach (var item in loot)
        {
            Debug.Log(item);
        }
    }
}
