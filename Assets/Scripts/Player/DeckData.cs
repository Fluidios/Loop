using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DeckData
{
    public string[] UnitNames;

    public DeckData() { }
    public DeckData(Road[] roadTilesInDeck)
    {
        UnitNames = new string[roadTilesInDeck.Length];
        for (int i = 0; i < roadTilesInDeck.Length; i++)
        {
            UnitNames[i] = roadTilesInDeck[i].Name;
        }
    }
}
