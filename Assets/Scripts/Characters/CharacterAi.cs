using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class CharacterAi : MonoBehaviour
{
    [SerializeField, Tooltip("All sensors wich provides ai with data about environment")] private Sensor[] _sensorsList;
    [SerializeField, Tooltip("Abstract class which provides a plan of actions based on sensors data")] private DecisionMaker _decisionMaker;
    [SerializeField] private string AttachedPersonality;

    /// <summary>
    /// runs an actions from the _plan
    /// </summary>
    private ActionsRunner _actionsRunner = new ActionsRunner();
    /// <summary>
    /// List of action, produced by _decisionMaker
    /// </summary>
    //private CharacterPlan _plan;
    /// <summary>
    /// All knowledges of a character are stored here, sensors should put their data here
    /// </summary>
    private MemoryBlackboard _personalMemory;
    private Action _unitLogicUpdateEndsCallback;
    /// <summary>
    /// All knowledges of a character are stored here, sensors should put their data here
    /// </summary>
    public MemoryBlackboard Memory
    {
        get { return _personalMemory; }
    }
    private Character _character;
    public void Init(Character character)
    {
        _character = character;
        _personalMemory = SystemsManager.GetSystemOfType<MemoryBank>().GetPersonalMemoryBlackboard(this);
        if(AttachedPersonality.Length > 0 )
            _personalMemory.Set("Personality", SystemsManager.GetSystemOfType<Society>().GetFirstPersonality());
        else
            _personalMemory.Set("Personality", SystemsManager.GetSystemOfType<Society>().GetFreePersonality());
        foreach (var sensor in _sensorsList)
        {
            sensor.Init(this);
        }
        _decisionMaker.Init(character);
        _actionsRunner.Init(character);
    }
    public void Disable()
    {
        if (_personalMemory.TryGetGeneric("Personality", out CharacterPersonality personality, null))
        {
            SystemsManager.GetSystemOfType<Society>().ReleasePersonality(personality);
        }
    }
    /// <summary>
    /// General method to update a logic of character
    /// </summary>
    /// <param name="updateEnds">callback which would be called when all done, since actions could go async</param>
    /// <param name="customWorld">if we want character to run in limited environment - provide a list of all entities</param>
    public void UpdateLogic(Action updateEnds, MonoBehaviour[] customWorld = null)
    {
        if (_character.Stats.IsDead) { Debug.Log(gameObject.name + " is dead."); updateEnds(); return; }

        _unitLogicUpdateEndsCallback = updateEnds;
        _personalMemory.Clear();
        foreach (var sensor in _sensorsList)
        {
            sensor.Scan(this, customWorld);
        }
        _decisionMaker.DecideBehaviour(this, RunDecidedPlan);
    }

    private void RunDecidedPlan(CharacterPlan plan)
    {
        if (plan != null)
            _actionsRunner.Execute(plan, _unitLogicUpdateEndsCallback);
        else
        {
            Debug.Log(gameObject.name + " - failed to produce a plan of actions to do.");
            _unitLogicUpdateEndsCallback();
        }
    }
}
