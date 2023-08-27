using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Sensor : MonoBehaviour
{
    public abstract void Init(Character character);

    public abstract void Scan(Character character, MonoBehaviour[] customWorld = null);
}
