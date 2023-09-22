using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Only observing custom world, provided by character
/// </summary>
public class VisualSensor : Sensor
{
    public override void Init(CharacterAi character) { }

    public override void Scan(CharacterAi character, MonoBehaviour[] customWorld = null)
    {
        if (customWorld == null) return;

        int entitiesObserved = 0;
        foreach (var m in customWorld)
        {
            if (m.gameObject == this.gameObject) continue;
            //here we determine what we actually seeing
            var observable = m as IVisuallyObservable;
            if(observable != null)
            {
                if (observable.TryScan<Character>(out var observedCharacter))
                {
                    character.Memory.Set(observedCharacter.name, observedCharacter);
                    entitiesObserved++;
                }
                else 
                {
                    object unknownObservable = observable.Scan();
                    character.Memory.Set(unknownObservable.GetType().Name, unknownObservable);
                }
            }

        }
    }
}

public interface IVisuallyObservable
{
    public bool TryScan<T>(out T value) where T : class;
    public object Scan();
}
