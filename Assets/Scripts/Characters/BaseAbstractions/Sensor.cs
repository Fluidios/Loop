using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Sensor : MonoBehaviour
{
    public abstract void Init(CharacterAi character);
    /// <summary>
    /// Collect the data of the world in a sensor-specific spector
    /// </summary>
    /// <param name="character"></param>
    /// <param name="customWorld">could be provided if we want character to run in a limited simulation and not a whole scene</param>
    public abstract void Scan(CharacterAi character, MonoBehaviour[] customWorld = null);
}
