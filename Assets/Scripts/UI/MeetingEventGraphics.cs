using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MeetingEventGraphics : RoadEventGraphics
{
    [SerializeField] private RectTransform[] _playerSideOrigins;
    [SerializeField] private RectTransform[] _npcSideOrigins;

    private Character _selectedCharacter;
    public Character SelectedCharacter
    {
        get { return _selectedCharacter; }
        set
        {
            _selectedCharacter = value;
            if (value != null && _characterSelectedCallback != null)
            {
                if (value.Stats.IsDead == _searchAmongAliveTargets) return;

                switch (_sideToSelectCharacter)
                {
                    case MeetingEventSide.PlayerSquad:
                        if(_playerSideCharacters.Contains(value))
                        {
                            _characterSelectedCallback(value);
                            _characterSelectedCallback = null;
                        }
                        break;
                    case MeetingEventSide.EnemiesSquad:
                        if (_npcSideCharacters.Contains(value))
                        {
                            _characterSelectedCallback(value);
                            _characterSelectedCallback = null;
                        }
                        break;
                }
            }
        }
    }
    private MeetingEventSide _sideToSelectCharacter;
    private bool _searchAmongAliveTargets;
    private Action<Character> _characterSelectedCallback;

    private HashSet<Character> _playerSideCharacters = new HashSet<Character>();
    private HashSet<Character> _npcSideCharacters = new HashSet<Character>();
    public HashSet<Character> SpawnNPCs(Character[] npcPrefabs)
    {
        int size = Mathf.Min(npcPrefabs.Length, _npcSideOrigins.Length);
        Character spawnedCharacter;

        for (int i = 0; i < size; i++)
        {
            spawnedCharacter = Instantiate(npcPrefabs[i], _npcSideOrigins[i].position, Quaternion.identity, _npcSideOrigins[i]);
            spawnedCharacter.transform.localScale = Vector3.left*2 + Vector3.one;
            (spawnedCharacter as ISelectable).OnSelected += (iselectable) => { SelectedCharacter = iselectable as Character; };
            _npcSideCharacters.Add(spawnedCharacter);
        }
        return _npcSideCharacters;
    }
    public HashSet<Character> SpawnPlayerSquad(Character[] playerSquadMembers)
    {
        int size = Mathf.Min(playerSquadMembers.Length, _playerSideOrigins.Length);
        Character spawnedCharacter;
        for (int i = 0; i < size; i++)
        {
            spawnedCharacter = Instantiate(playerSquadMembers[i], _playerSideOrigins[i].position, Quaternion.identity, _playerSideOrigins[i]);
            spawnedCharacter.transform.localScale = Vector3.one;
            (spawnedCharacter as ISelectable).OnSelected += (iselectable) => { SelectedCharacter = iselectable as Character; };
            _playerSideCharacters.Add(spawnedCharacter);
        }
        return _playerSideCharacters;
    }
    public void StartTargetCharacterSelection(MeetingEventSide whichSideToSearchForTarget, bool searchAmongAliveTargets, Action<Character> onTargetSelected)
    {
        _sideToSelectCharacter = whichSideToSearchForTarget;
        _searchAmongAliveTargets = searchAmongAliveTargets;
        _characterSelectedCallback = onTargetSelected;
    }
    public void EndTargetCharacterSelection()
    {
        _characterSelectedCallback = null;
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
