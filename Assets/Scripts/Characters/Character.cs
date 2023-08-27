using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour, IVisuallyObservable 
{
    [SerializeField] private Sensor[] _sensorsList;
    [SerializeField] private DecisionMaker _decisionMaker;
    private ActionsRunner _actionsRunner = new ActionsRunner();
    private CharacterPlan _plan;
    private CharacterMemory _memory = new CharacterMemory();

    public CharacterMemory Memory
    {
        get { return _memory;}
    }

    private void Start()
    {
        Init();   
    }

    private void Init()
    {
        foreach (var sensor in _sensorsList)
        {
            sensor.Init(this);
        }
        _decisionMaker.Init(this);
        _actionsRunner.Init(this);
    }

    public void UpdateLogic(Action updateEnds, MonoBehaviour[] customWorld = null)
    {
        _memory.ClearHotMemories();
        foreach (var sensor in _sensorsList)
        {
            sensor.Scan(this, customWorld);
        }
        _plan = _decisionMaker.DecideBehaviour(this);
        _actionsRunner.Execute(_plan, updateEnds);
    }

    public MemoryNote Scan()
    {
        return new MemoryNote(typeof(Character), this);
    }
}

