using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField] private Collider _collider;
    [SerializeField] private SpriteRenderer _selfGraphics;
    [SerializeField] private Color _blockingColor = Color.red;
    private Color _normalColor;
    public Vector3Int GridPosition
    {
        get; private set;
    }
    private Location _location;
    public Action<Location> OnLocationUpdated;
    public Location Location
    {
        get { return _location; }
    }
    public void Setup(Vector3Int gridPosition)
    {
        this.GridPosition = gridPosition;
        transform.position = this.GridPosition;
    }

    private void Awake()
    {
        _normalColor = _selfGraphics.color;
    }

    public void MarkAsBlocked(bool value)
    {
        if(_location == null)
            _selfGraphics.color = value ? _blockingColor : _normalColor;
        else
            _location.GroundGraphics.color = value ? _blockingColor : _normalColor;
        _collider.enabled = !value;
    }
    public void SetSelfGraphicsActive(bool value)
    {
        _selfGraphics.gameObject.SetActive(value);
    }
    public void ReplaceLocation(Location location, bool showSpawnAnimation = true)
    {
        if (this._location != null)
        {
            Destroy(this._location.gameObject);
        }
        
        _location = location;

        if(location != null) 
        {
            location.transform.SetParent(transform);
            location.transform.position = transform.position;
            if (showSpawnAnimation)
            {
                _selfGraphics.gameObject.SetActive(false);
                location.ShowSpawnAnimation();
            }
            if (OnLocationUpdated != null)
                OnLocationUpdated(location);
        }
        else
        {
            _selfGraphics.gameObject.SetActive(true);
        }
        
    }
}
