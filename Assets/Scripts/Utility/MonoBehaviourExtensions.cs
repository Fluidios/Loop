using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public static class MonoBehaviourExtensions 
{
    public static void InvokeUpdate(this MonoBehaviour obj)
    {
        var updateMethod = obj.GetType().GetMethod("Update",
            BindingFlags.NonPublic | BindingFlags.Instance);
        updateMethod?.Invoke(obj, null);
    }
    public static void InvokeOnEnable(this MonoBehaviour obj)
    {
        var onEnableMethod = obj.GetType().GetMethod("OnEnable",
            BindingFlags.NonPublic | BindingFlags.Instance);
        onEnableMethod?.Invoke(obj, null);
    }
}
