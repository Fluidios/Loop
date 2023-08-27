using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _betweenTilesMoveSpeed = 5;
    [SerializeField] private Character[] _playerDefaultSquad;
    public Action<Road> PlayerEnterRoadTile;
    private Map _map;
    private TimeFlow _timeFlow;
    private int _passedTiles = 0;
    Coroutine _moveRoutine;

    public Character[] PlayerSquadPrefabs
    {
        get
        {
            return _playerDefaultSquad;
        }
    }

    void Start()
    {
        _map = SystemsManager.GetSystemOfType<Map>();
        _timeFlow = SystemsManager.GetSystemOfType<TimeFlow>();
        TryGoNextRoadTile();
    }

    public void TryGoNextRoadTile()
    {
        if (!SystemsManager.AllSystemsInitialized) return;
        var currentRoad = _map.Road[_timeFlow.CurrentRoadTile % _map.Road.Count];
        if (_passedTiles == 0 || currentRoad.RoadEvent.Passed)
        {
            if(_passedTiles != 0)
                currentRoad.OnPlayerExitRoadTile(this);
            _timeFlow.CurrentRoadTile++;
            currentRoad = _map.Road[_timeFlow.CurrentRoadTile % _map.Road.Count];

            if (_moveRoutine != null)
                StopCoroutine(_moveRoutine);
            _moveRoutine = StartCoroutine(MoveAnimation(currentRoad.transform.position, CallPlayerEnterTile));

            _passedTiles++;
            //Debug.Log("Next");
        }
        else
        {
            //Debug.Log("Fail"); 
        }
    }
    private void CallPlayerEnterTile()
    {
        var road = _map.Road[_timeFlow.CurrentRoadTile % _map.Road.Count];
        road.OnPlayerEnterRoadTile(this);
        if(PlayerEnterRoadTile != null)
            PlayerEnterRoadTile.Invoke(road);
    }

    public void WaitAndTryGoNextRoadTile()
    {
        StartCoroutine(DoWithDelay(1, TryGoNextRoadTile));
    }
    IEnumerator DoWithDelay(float delay, Action action)
    {
        for (float t = 0; t < delay;)
        {
            t += _timeFlow.DeltaTime;
            yield return null;
        }
        action();
    }
    IEnumerator MoveAnimation(Vector3 target, Action callback = null)
    {
        float moveSpeed = _betweenTilesMoveSpeed;
        Vector3 from = transform.position;
        float distance = Vector3.Distance(from, target);
        moveSpeed = moveSpeed / distance;
        for (float t = 0; t <= 1;)
        {
            t += _timeFlow.DeltaTime * moveSpeed;
            transform.position = Vector3.Lerp(from, target, t);
            yield return null;
        }
        _moveRoutine = null;
        callback?.Invoke();
    }
}
