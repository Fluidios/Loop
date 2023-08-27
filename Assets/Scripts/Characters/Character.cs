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
    private readonly CharacterMemory _memory = new CharacterMemory();
    /// <summary>
    /// All knowledges of a character are stored here, sensors should put their data here
    /// </summary>
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
    /// <summary>
    /// General method to update a logic of character
    /// </summary>
    /// <param name="updateEnds">callback which would be called when all done, since actions could go async</param>
    /// <param name="customWorld">if we want character to run in limited environment - provide a list of all entities</param>
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

