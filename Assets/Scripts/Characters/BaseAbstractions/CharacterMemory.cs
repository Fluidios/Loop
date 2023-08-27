using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Storage of all data from scanners and personal character memories
/// </summary>
public class CharacterMemory
{
    /// <summary>
    /// comes from sensors just now, would be cleaned before every logic update
    /// </summary>
    private Dictionary<Type, Dictionary<string, MemoryNote>> _hotMemoryData = new();
    /// <summary>
    /// comes from sensors just now, would be cleaned before every logic update
    /// </summary>
    public Dictionary<Type, Dictionary<string, MemoryNote>> HotMemoryData => _hotMemoryData;

    public void ClearHotMemories()
    {
        _hotMemoryData.Clear();
    }
}
/// <summary>
/// One memory data block
/// </summary>
public class MemoryNote
{
    public Type Type;
    private object _note;
    public object Note => _note;
    public MemoryNote(Type type, object note)
    {
        Type = type;
        _note = note;
    }
}
