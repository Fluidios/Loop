using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// A container for ai logic
/// </summary>
public class Character : MonoBehaviour, IVisuallyObservable, ISelectable
{
    [SerializeField, Tooltip("All sensors wich provides ai with data about environment")] private Sensor[] _sensorsList;
    [SerializeField, Tooltip("Abstract class which provides a plan of actions based on sensors data")] private DecisionMaker _decisionMaker;
    [SerializeField] private CharacterStats _characterStats;
    [SerializeField, Tooltip("Marker to display focus on character")] private GameObject _focusedOutline;
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
        get { return _personalMemory;}
    }

    public CharacterStats Stats { get { return _characterStats; } }

    private bool _focused;
    public bool Focused => _focused;

    public Action<ISelectable> OnSelected { get; set; }

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
        if (Stats.Health.Value <= 0) { Debug.Log(gameObject.name + " is dead."); updateEnds(); return; }

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

    #region IVisuallyObservable
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
    #endregion

    #region ISelectable
    public void OnPointerClick(PointerEventData eventData)
    {
        _focused = true;
        OnSelected(this);
        Debug.Log("Click");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _focused = true;
        _focusedOutline.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _focused = false;
        _focusedOutline.SetActive(false);
    }
    #endregion
}

