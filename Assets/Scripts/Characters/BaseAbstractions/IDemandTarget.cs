using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDemandTarget<T> where T : class
{
    public MeetingEventSide WhichSideToSearchForTarget { get; }
    public T Target { get; set; }
}

public enum MeetingEventSide
{
    PlayerSquad, EnemiesSquad,
}
