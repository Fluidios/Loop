using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterStats
{
    public CappedObservableValue<int> Health;

    public void Reset()
    {
        Health.Reset();
    }
}
