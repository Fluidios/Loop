using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterStats
{
    public CappedObservableValue<int> Health;
    public bool IsDead
    {
        get { return (Health.Value < 1); }
    }
    public bool BelongToPlayerTeam
    {
        get; set;
    }

    public void Reset()
    {
        Health.Reset();
    }

    public void OnDisable()
    {
    }
}
