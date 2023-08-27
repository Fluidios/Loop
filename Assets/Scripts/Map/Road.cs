using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : Location
{
    [SerializeField] private bool _canBeReplacedByOtherRoad;
    [SerializeField] private LootTable<RoadEvent> _possibleEvents;
    private Randomness _randomness;
    private RoadEvent _currentEvent;
    public RoadEvent RoadEvent
    {
        get { return _currentEvent; }
    }
    public bool CanBeReplacedByOtherRoad
    {
        get { return _canBeReplacedByOtherRoad; }
    }

    private void Awake()
    {
        _randomness = SystemsManager.GetSystemOfType<Randomness>();
        _possibleEvents.ProvideSpecificRandomnessSeed(_randomness.UInt()); //since we dont want from all roads to provide simmilar events sequence
        OnAwake();
    }

    protected virtual void OnAwake() { }

    public void OnPlayerEnterRoadTile(PlayerController player)
    {
        if(_possibleEvents.GetLoot(out var roadEvent) == LootTable<RoadEvent>.LootRollResult.DroppedLessThanRequested)
        {
            Debug.LogError("Cant get event for road, event with 100% chance of appear should exist as default variant");
            return;
        }
        _currentEvent = Instantiate(roadEvent, transform.position, Quaternion.identity, transform);
        _currentEvent.PlayerController = player;
    }

    public void OnPlayerExitRoadTile(PlayerController player)
    {
        Destroy(_currentEvent.gameObject);
    }

    public override bool PlaceRool(Node node)
    {
        var road = (node.Location as Road);
        return road != null && road.CanBeReplacedByOtherRoad;
    }
}
