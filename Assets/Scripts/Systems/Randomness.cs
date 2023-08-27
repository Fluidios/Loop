using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Randomness : GameSystem
{
    [SerializeField] private int _forcedSeed = -1;
    public int Seed { get; private set; }
    public System.Random Random { get; private set; }

    public override bool AsyncInitialization => false;

    public override void Initialize(System.Action initializationEndedCallback)
    {
        if (_forcedSeed < 0)
            Seed = UnityEngine.Random.Range(0, int.MaxValue);
        else
            Seed = _forcedSeed;
        Random = new System.Random(Seed);
    }
    public int UInt()
    {
        return Random.Next(0, int.MaxValue);
    }
    public int Int(int min, int max)
    {
        return Random.Next(min, max);
    }
    public bool Bool()
    {
        return Random.NextDouble() > 0.5;
    }
}
