using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameSystem : MonoBehaviour
{
    public abstract bool AsyncInitialization { get; }
    public virtual void Initialize(System.Action initializationEndedCallback) { initializationEndedCallback?.Invoke(); }
    public virtual void OnUpdate() { }
}
