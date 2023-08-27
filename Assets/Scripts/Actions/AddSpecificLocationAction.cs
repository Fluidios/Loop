using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AddSpecificLocationAction : ActionComponent
{
    [SerializeField] private bool _debugMode;
    [SerializeField] private Transform _cardsSpawnOrigin;
    [SerializeField] private LootTable<Location> _locationsList = new LootTable<Location>();
    private Deck _deck;
    private Hand _hand;
    private void Awake()
    {
        _deck = SystemsManager.GetSystemOfType<Deck>();
        _hand = SystemsManager.GetSystemOfType<Hand>();
    }

    public override void Execute()
    {
        if(_locationsList.Length > 1)
        {
            if(_locationsList.GetLoot(out var location) == LootTable<Location>.LootRollResult.DroppedLessThanRequested) 
            {
                if (_debugMode)
                    Debug.Log("Null drop.");
            }
            var card = _deck.SpawnEmptyCard(Camera.main.WorldToScreenPoint(_cardsSpawnOrigin.position));
            card.Associate(location.Name);

            _hand.AddToHand(card);
        }
    }
}
