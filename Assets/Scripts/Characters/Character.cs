using System;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// A container for ai logic
/// </summary>
public class Character : MonoBehaviour, IVisuallyObservable, ISelectable
{
    [SerializeField] private CharacterAi _ai;
    [SerializeField] private CharacterStats _characterStats;
    [SerializeField, Tooltip("Marker to display focus on character")] private GameObject _focusedOutline;

    public CharacterAi Ai => _ai;
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
        _ai.Init(this);
        _characterStats.Reset();
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

