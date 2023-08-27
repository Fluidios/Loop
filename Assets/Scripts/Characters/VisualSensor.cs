using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Only observing custom world, provided by character
/// </summary>
public class VisualSensor : Sensor
{
    public override void Init(Character character) { }

    public override void Scan(Character character, MonoBehaviour[] customWorld = null)
    {
        if (customWorld == null) return;

        IVisuallyObservable observable;
        int entitiesObserved = 0;
        string memoryKey;
        Type sensorType = typeof(VisualSensor);
        foreach (var m in customWorld)
        {
            if (m.gameObject == this.gameObject) continue;
            observable = m as IVisuallyObservable;
            if(observable != null)
            {
                if (entitiesObserved == 0)
                    character.Memory.HotMemoryData.Add(sensorType, new Dictionary<string, MemoryNote>());
                var memoryNote = observable.Scan(); 
                memoryKey = string.Format("{0}-{1}", memoryNote.Type, entitiesObserved);
                character.Memory.HotMemoryData[sensorType].Add(memoryKey, memoryNote);
                entitiesObserved++;
            }
        }
    }
}

public interface IVisuallyObservable
{
    public MemoryNote Scan();
}
