using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeetingEventGraphics : RoadEventGraphics
{
    [SerializeField] private RectTransform[] _playerSideOrigins;
    [SerializeField] private RectTransform[] _npcSideOrigins;
    public Character[] SpawnNPCs(Character[] npcPrefabs)
    {
        int size = Mathf.Min(npcPrefabs.Length, _npcSideOrigins.Length);
        var _npcInstances = new Character[size];
        for (int i = 0; i < size; i++)
        {
            _npcInstances[i] = Instantiate(npcPrefabs[i], _npcSideOrigins[i].position, Quaternion.identity, _npcSideOrigins[i]);
            _npcInstances[i].transform.localScale = Vector3.left*2 + Vector3.one;
        }
        return _npcInstances;
    }
    public Character[] SpawnPlayerSquad(Character[] playerSquadMembers)
    {
        int size = Mathf.Min(playerSquadMembers.Length, _playerSideOrigins.Length);
        var _playerSquadMemberInstances = new Character[size];
        for (int i = 0; i < size; i++)
        {
            _playerSquadMemberInstances[i] = Instantiate(playerSquadMembers[i], _playerSideOrigins[i].position, Quaternion.identity, _playerSideOrigins[i]);
            _playerSquadMemberInstances[i].transform.localScale = Vector3.one;
        }
        return _playerSquadMemberInstances;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        foreach (var item in _playerSideOrigins)
        {
            Gizmos.DrawWireSphere(item.position, 50);
        }
        foreach (var item in _npcSideOrigins)
        {
            Gizmos.DrawWireSphere(item.position, 50);
        }
    }
}
