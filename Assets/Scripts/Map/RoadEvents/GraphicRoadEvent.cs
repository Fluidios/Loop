using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GraphicRoadEvent<T> : RoadEvent where T : RoadEventGraphics
{
    [SerializeField] private T _eventGraphics;

    private EventsVisualizer _visualizer;
    private T _spawnedGraphics;

    private void Start()
    {
        _visualizer = SystemsManager.GetSystemOfType<EventsVisualizer>();
        _spawnedGraphics = _visualizer.ShowEventVisual(_eventGraphics);
        InitializeGraphics(_spawnedGraphics);
    }
    protected virtual void InitializeGraphics(T graphics)
    {

    }

    protected void MarkEventAsPassed()
    {
        _visualizer.HideEventVisual(_spawnedGraphics, () => Passed = true);
    }
}
