using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : GameSystem
{
    [SerializeField] private DeckPreset _defaultDeck;
    [SerializeField] private LocationCard _roadCardPrefab;
    private List<string> _cardsInDeck = new List<string>();
    private Randomness _randomness;
    private Hand _hand;

    public override bool AsyncInitialization => false;
    public static int ActivePlayerDeckIndex
    {
        get { return ES3.Load<int>("ActivePlayerDeck", -1); }
        set { ES3.Save<int>("ActivePlayerDeck", value); }
    }
    public override void Initialize(Action initializationEndedCallback)
    {
        LoadDeck();
        _randomness = SystemsManager.GetSystemOfType<Randomness>();
        _hand = SystemsManager.GetSystemOfType<Hand>();
    }
    public bool Empty
    {
        get { return _cardsInDeck.Count == 0; }
    }
    public LocationCard SpawnEmptyCard(Vector3 atPosition = default)
    {
        return Instantiate(_roadCardPrefab, atPosition, Quaternion.identity, _hand.transform);
    }
    public LocationCard SpawnRandomCardFromDeck(Vector3 atPosition = default)
    {
        if (Empty)
        {
            Debug.LogError("CANT TAKE UNIT FROM EMPTY DECK");
            return null;
        }
        int rIndex = _randomness.Int(0, _cardsInDeck.Count);
        var cardName = _cardsInDeck[rIndex];
        //spawn and setup card
        LocationCard card = SpawnEmptyCard(atPosition);
        card.Associate(cardName);
        return card;
    }

    public void Add(string unit)
    {
        _cardsInDeck.Add(unit);
    }

    private void LoadDeck()
    {
        if (ActivePlayerDeckIndex < 0)
        {
            ActivePlayerDeckIndex = 0;
            SaveDeck(ActivePlayerDeckIndex, _defaultDeck.RoadTilesInDeck);
            foreach (var item in _defaultDeck.RoadTilesInDeck)
            {
                _cardsInDeck.Add(item.Name);
            }
        }
        else
        {
            LoadDeckAtIndex(ActivePlayerDeckIndex);
        }
    }
    private void SaveDeck(int atIndex, Road[] roadTilesInDeck)
    {
        DeckData dataToSave = new DeckData(roadTilesInDeck);
        ES3.Save<DeckData>(string.Format("Deck{0}", atIndex), dataToSave);
    }
    public void LoadDeckAtIndex(int atIndex)
    {
        DeckData deckData = ES3.Load<DeckData>(string.Format("Deck{0}", atIndex), defaultValue: null);

        if (deckData == null)
        {
            Debug.Log("EMPTY DECK AT REQUESTED INDEX: " + atIndex);
            return;
        }
        _cardsInDeck.Clear();
        _cardsInDeck.AddRange(deckData.UnitNames);
    }

    public IEnumerator<string> GetEnumerator()
    {
        return _cardsInDeck.GetEnumerator();
    }
}
