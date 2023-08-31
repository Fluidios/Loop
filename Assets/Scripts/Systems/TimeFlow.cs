using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeFlow : GameSystem
{
    [SerializeField] private int _dayLength = 4;
    [SerializeField] private UiDependentDM _playerController;
    private int _currentDay = 1;
    private int _dayPartsAmount;
    private int _currentRoadTile;
    public override bool AsyncInitialization => false;
    private bool _gameIsPaused = false;
    public bool GameIsPaused
    {
        get { return _gameIsPaused; }
        set
        {
            _gameIsPaused = value;
        }
    }
    public int CurrentRoadTile
    {
        get { return _currentRoadTile; }
        set { _currentRoadTile = value; }
    }
    public float DeltaTime
    {
        get { return _gameIsPaused ? 0 : Time.deltaTime; }
    }
    public int CurrentDay
    {
        get { return _currentDay; }
    }
    public DayPart CurrentDayPart
    {
        get
        {
            int currentDayPartIndex = Mathf.FloorToInt((float)(_currentRoadTile % _dayLength) / _dayLength * _dayPartsAmount);

            return Enum.Parse<DayPart>(currentDayPartIndex.ToString());
        }
    }

    public override void Initialize(Action initializationEndedCallback)
    {
        _dayPartsAmount = Enum.GetValues(typeof(DayPart)).Length;
    }
    public enum DayPart
    {
        Morning = 1, Day = 2, Evening = 3, Night = 0
    }
}
