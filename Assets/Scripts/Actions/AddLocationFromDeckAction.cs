using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AddLocationFromDeckAction : ActionComponent
{
    [SerializeField] private int _amount = 1;
    [SerializeField] private Transform _cardsSpawnOrigin;
    private Deck _deck;
    private Hand _hand;
    private CameraSystem _cameraSystem;

    private void Awake()
    {
        _deck = SystemsManager.GetSystemOfType<Deck>();
        _hand = SystemsManager.GetSystemOfType<Hand>();
        _cameraSystem = SystemsManager.GetSystemOfType<CameraSystem>();
    }
    public override void Execute()
    {
        Camera cam = _cameraSystem.MainCamera;
        for (int i = 0; i < _amount; i++)
        {
            var card = _deck.SpawnRandomCardFromDeck(cam.WorldToScreenPoint(_cardsSpawnOrigin.position));

            _hand.AddToHand(card);
        }
    }
}
