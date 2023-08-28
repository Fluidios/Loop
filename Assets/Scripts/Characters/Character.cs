using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A container for ai logic
/// </summary>
public class Character : MonoBehaviour, IVisuallyObservable 
{
    [SerializeField, Tooltip("All sensors wich provides ai with data about environment")] private Sensor[] _sensorsList;
    [SerializeField, Tooltip("Abstract class which provides a plan of actions based on sensors data")] private DecisionMaker _decisionMaker;
    [SerializeField] private CharacterStats _characterStats;
    /// <summary>
    /// runs an actions from the _plan
    /// </summary>
    private ActionsRunner _actionsRunner = new ActionsRunner();
    /// <summary>
    /// List of action, produced by _decisionMaker
    /// </summary>
    private CharacterPlan _plan;
    /// <summary>
    /// All knowledges of a character are stored here, sensors should put their data here
    /// </summary>
    private MemoryBlackboard _personalMemory;
    /// <summary>
    /// All knowledges of a character are stored here, sensors should put their data here
    /// </summary>
    public MemoryBlackboard Memory
    {
        get { return _personalMemory;}
    }

    private void Start()
    {
        Init(); 
    }

    private void Init()
    {
        _personalMemory = SystemsManager.GetSystemOfType<MemoryBank>().GetPersonalMemoryBlackboard(this);
        foreach (var sensor in _sensorsList)
        {
            sensor.Init(this);
        }
        _decisionMaker.Init(this);
        _actionsRunner.Init(this);
        _characterStats.Reset();
    }
    /// <summary>
    /// General method to update a logic of character
    /// </summary>
    /// <param name="updateEnds">callback which would be called when all done, since actions could go async</param>
    /// <param name="customWorld">if we want character to run in limited environment - provide a list of all entities</param>
    public void UpdateLogic(Action updateEnds, MonoBehaviour[] customWorld = null)
    {
        _personalMemory.Clear();
        foreach (var sensor in _sensorsList)
        {
            sensor.Scan(this, customWorld);
        }
        _plan = _decisionMaker.DecideBehaviour(this);
        _actionsRunner.Execute(_plan, updateEnds);
    }

    public bool TryScan<T>(out T value) where T:class
    {
        value = this as T;
        if (value != null)
        {
            return true;
        }
        return false;
    }
    public object Scan()
    {
        return this;
    }
}

