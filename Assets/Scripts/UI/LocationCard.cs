using Game.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class LocationCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private TextMeshProUGUI _locationNameText;
    [SerializeField] private CanvasGroup _canvasGroup;
    [Range(1,100)]public float MoveSpeed = 50;
    [SerializeField] private LayerMask _nodesLayer;
    private LocationsLibrary _locationsLibrary;
    private CameraSystem _cameraSystem;
    private Hand _hand;
    private Map _map;
    private TimeFlow _timeFlow;
    private string _associatedLocationTileName;

    private Ray _ray;
    private float _rayToVMPlaneLength;
    private RaycastHit _hit;

    private Vector3 _startDragPosition;
    private Vector2 _draggOffset;
    private Location _associatedLocationInstance;
    private bool _dragInitializationPassed;

    private Coroutine _moveCoroutine;
    private Plane _virtualMapPlane;

    private void Awake()
    {
        _locationsLibrary = SystemsManager.GetSystemOfType<LocationsLibrary>();
        _cameraSystem = SystemsManager.GetSystemOfType<CameraSystem>();
        _hand = SystemsManager.GetSystemOfType<Hand>();
        _map = SystemsManager.GetSystemOfType<Map>();
        _timeFlow = SystemsManager.GetSystemOfType<TimeFlow>();
        MoveSpeed = Screen.width * MoveSpeed / 100;
        _virtualMapPlane = new Plane(Vector3.up, _map.MapCenter);
    }

    public void Associate(string associatedLocationTileName)
    {
        _associatedLocationTileName = associatedLocationTileName;
        _locationNameText.text = _associatedLocationTileName;
    }
    public void ThrowTo(Vector3 target, float speed = -1, Action callback = null)
    {
        if (_moveCoroutine != null)
            StopCoroutine(_moveCoroutine);
        if (speed < 0) speed = MoveSpeed;
        _moveCoroutine = StartCoroutine(MoveTo(target, speed, callback));
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        _startDragPosition = transform.position;
        _draggOffset = (Vector2)transform.position - Mouse.current.position.ReadValue();
        _timeFlow.GameIsPaused = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Mouse.current.position.ReadValue() + _draggOffset;
        if(Vector3.Distance(_startDragPosition, transform.position) > 50)
        {
            _canvasGroup.alpha = 0;
            if (!_dragInitializationPassed)
            {
                CreateLocationInstance();
                ColorizeMapAccordingToDraggedLocation();
                _dragInitializationPassed = true;
            }
            // phantom drag
            _ray = _cameraSystem.MainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(_ray, out _hit, float.MaxValue, _nodesLayer))
            {
                _associatedLocationInstance.transform.position = _hit.transform.position;
            }
            else if (_virtualMapPlane.Raycast(_ray, out _rayToVMPlaneLength))
            {
                _associatedLocationInstance.transform.position = _ray.GetPoint(_rayToVMPlaneLength);
            }
        }
        else
        {
            _canvasGroup.alpha = 1;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _timeFlow.GameIsPaused = false;
        _dragInitializationPassed = false;
        //Handle associated location spawn
        _ray = _cameraSystem.MainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(_ray, out _hit, float.MaxValue, _nodesLayer))
        {
            if(_hit.transform.TryGetComponent(out Node node))
            {
                //only suitable nodes left since we already checked it on begin of drag
                AssignLocation(node);
                ColorizeMapAsNormal();
                return;
            }
        }
        if(_associatedLocationInstance != null) _associatedLocationInstance.gameObject.SetActive(false);
        ColorizeMapAsNormal();
        _canvasGroup.alpha = 1;
        ThrowTo(_startDragPosition);
    }

    private void CreateLocationInstance()
    {
        if (_associatedLocationInstance == null)
            _associatedLocationInstance = Instantiate(_locationsLibrary.GetLocation(_associatedLocationTileName));
        else
            _associatedLocationInstance.gameObject.SetActive(true);
    }
    private void ColorizeMapAccordingToDraggedLocation()
    {
        int mSize = _map.Size;
        for (int x = 0; x < mSize; x++)
        {
            for (int y = 0; y < mSize; y++)
            {
                _map[x, y].MarkAsBlocked(!LocationCouldBePlacedHere(_map[x, y], _associatedLocationInstance));
            }
        }
    }
    private void ColorizeMapAsNormal()
    {
        int mSize = _map.Size;
        for (int x = 0; x < mSize; x++)
        {
            for (int y = 0; y < mSize; y++)
            {
                _map[x, y].MarkAsBlocked(false);
            }
        }
    }
    private bool LocationCouldBePlacedHere(Node node, Location location)
    {
        return location.PlaceRool(node);
    }
    private void AssignLocation(Node node)
    {
        node.ReplaceLocation(_associatedLocationInstance);
        _associatedLocationInstance = null;
        _hand.DiscardCardImidiatelly(this, true);
    }

    IEnumerator MoveTo(Vector3 target, float speed = 1, Action callback = null)
    {
        _canvasGroup.blocksRaycasts = false;
        Vector3 from = transform.position;
        speed /= Vector3.Distance(from, target);
        for (float t = 0; t <= 1;)
        {
            t += Time.deltaTime*speed;
            transform.position = Vector3.Lerp(from, target, t);
            yield return null;
        }
        _moveCoroutine = null;
        _canvasGroup.blocksRaycasts=true;
        callback?.Invoke();
    }

    private void OnDestroy()
    {
        if(_associatedLocationInstance != null)
            Destroy(_associatedLocationInstance.gameObject);
    }
}
