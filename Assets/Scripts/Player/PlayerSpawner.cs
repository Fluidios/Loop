using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : GameSystem
{
    [SerializeField] private PlayerController _player;
    public override bool AsyncInitialization => false;

    public override void Initialize(Action initializationEndedCallback)
    {
        var map = SystemsManager.GetSystemOfType<Map>();
        
        var player = Instantiate(_player, map.Road[0].transform.position, Quaternion.identity);
    }
}
