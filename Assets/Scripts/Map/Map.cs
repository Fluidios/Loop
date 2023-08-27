using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : GameSystem
{
    [SerializeField] private int _mapSize = 10;
    [SerializeField] private Node _nodePrefab;
    public int Size { get { return _mapSize; } }

    public override bool AsyncInitialization => false;

    private Node[,] _map;
    private List<Road> _road;
    private Vector3 _mapCenter;

    public List<Road> Road
    {
        get { return _road; }
        set
        {
            _road = value;
            RecalculateMapCenter();
        }
    }
    public Vector3 MapCenter
    {
        get => _mapCenter;
    }

    public override void Initialize(System.Action initializationEndedCallback)
    {
        CreateMap();
    }
    private void CreateMap()
    {
        _map = new Node[_mapSize, _mapSize];
        for (int i = 0; i < _mapSize; i++)
        {
            for (int j = 0; j < _mapSize; j++)
            {
                _map[i, j] = Instantiate(_nodePrefab, transform);
                _map[i,j].Setup(new Vector3Int(i,0,j));
            }
        }
    }
    private void RecalculateMapCenter()
    {
        Vector3 center = Vector3.zero;
        foreach (var item in _road)
        {
            center += item.transform.position;
        }
        center /= _road.Count;
        _mapCenter = center;
    }
    public void UpdateRoad(int atIndex, Road newRoadLocation)
    {
        _road[atIndex] = newRoadLocation;
    }
    public Node this[int x, int y]
    {
        get { return _map[x, y]; }
    }
}
