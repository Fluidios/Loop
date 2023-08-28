using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryBank : GameSystem
{
    private Dictionary<MonoBehaviour, MemoryBlackboard> _personalMemoryBlackBoards = new Dictionary<MonoBehaviour, MemoryBlackboard>();
    private Dictionary<string, MemoryBlackboard> _sharedMemoryBlackBoards = new Dictionary<string, MemoryBlackboard>();
    public override bool AsyncInitialization => false;

    public MemoryBlackboard GetPersonalMemoryBlackboard(MonoBehaviour requester)
    {
        if(!_personalMemoryBlackBoards.ContainsKey(requester))
            _personalMemoryBlackBoards[requester] = new MemoryBlackboard();

        return _personalMemoryBlackBoards[requester];
    }
    public MemoryBlackboard GetSharedMemoryBlackboard(string blackboardKey)
    {
        if (!_sharedMemoryBlackBoards.ContainsKey(blackboardKey))
            _sharedMemoryBlackBoards[blackboardKey] = new MemoryBlackboard();

        return _sharedMemoryBlackBoards[blackboardKey];
    }
}
