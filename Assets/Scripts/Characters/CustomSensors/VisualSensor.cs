using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

/// <summary>
/// Only observing custom world, provided by character
/// </summary>
public class VisualSensor : Sensor
{
    [SerializeField] private VisualRecognizer[] _recognizers;
    public override void Init(CharacterAi character) { } 

    public override void Scan(CharacterAi character, MonoBehaviour[] customWorld = null)
    {
        if (customWorld == null)
        {
            Debug.LogWarning("VisualSensor can only handle customWorld for now");
            return;
        }

        bool observableRecognized;
        foreach (var m in customWorld)
        {
            if (m.gameObject == this.gameObject) continue;
            //here we determine what we actually seeing
            var observable = m as IVisuallyObservable;
            if(observable != null)
            {
                observableRecognized = false;
                foreach(VisualRecognizer recognizer in _recognizers)
                {
                    observableRecognized = recognizer.TryRecognizeAndSaveToMemory(observable, character.Memory);
                    if (observableRecognized) break;
                }

                if (!observableRecognized)
                {
                    //give the character a chance to learn to recognize such objects in the future.
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
