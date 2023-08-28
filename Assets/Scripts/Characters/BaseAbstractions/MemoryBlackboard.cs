using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Storage of all data from scanners and personal character memories
/// </summary>
public class MemoryBlackboard
{
    private Dictionary<string, int> _intValues = new Dictionary<string, int>();
    private Dictionary<string, float> _floatValues = new Dictionary<string, float>();
    private Dictionary<string, bool> _boolValues = new Dictionary<string, bool>();
    private Dictionary<string, string> _stringValues = new Dictionary<string, string>();
    private Dictionary<string, Character> _characterValues = new Dictionary<string, Character>();
    private Dictionary<string, object> _genericValues = new Dictionary<string, object>();

    public void Set(string key, int value)
    {
        _intValues[key] = value;
    }

    public void Set(string key, float value)
    {
        _floatValues[key] = value;
    }

    public void Set(string key, bool value)
    {
        _boolValues[key] = value;
    }

    public void Set(string key, string value)
    {
        _stringValues[key] = value;
    }

    public void Set(string key, Character value)
    {
        _characterValues[key] = value;
    }

    public void Set(string key, object value)
    {
        _genericValues[key] = value;
    }

    public bool TryGetInt(string key, out int value)
    {
        if (!_intValues.ContainsKey(key))
        {
            value = 0;
            return false;
        }

        value = _intValues[key];
        return true;
    }

    public bool TryGetFloat(string key, out float value)
    {
        if (!_floatValues.ContainsKey(key))
        {
            value = 0f;
            return false;
        }

        value = _floatValues[key];
        return true;
    }

    public bool TryGetBool(string key, out bool value)
    {
        if (!_boolValues.ContainsKey(key))
        {
            value = false;
            return false;
        }

        value = _boolValues[key];
        return true;
    }

    public bool TryGetString(string key, out string value)
    {
        if (!_stringValues.ContainsKey(key))
        {
            value = string.Empty;
            return false;
        }

        value = _stringValues[key];
        return true;
    }

    public bool TryGetCharacter(string key, out Character value)
    {
        if (!_characterValues.ContainsKey(key))
        {
            value = null;
            return false;
        }

        value = _characterValues[key];
        return true;
    }
    public bool TryGetGeneric<T>(string key, out T value, T defaultValue) where T : class
    {
        if (!_genericValues.ContainsKey(key))
        {
            value = defaultValue;
            return false;
        }

        value = _genericValues[key] as T;
        return true;
    }

    public IEnumerable<Character> GetIEnumerableOfCharacters()
    {
        return _characterValues.Values;
    }
    public void Clear()
    {
        _intValues.Clear();
        _floatValues.Clear();
        _boolValues.Clear();
        _stringValues.Clear();
        _characterValues.Clear();
    }
}
