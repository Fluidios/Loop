using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSystem : GameSystem
{
    [SerializeField] CinemachineVirtualCamera _vCamera;
    [SerializeField] Camera _mainCamera;
    private Transform _mapCenter;

    public Camera MainCamera
    {
        get { return _mainCamera; }
    }
    public override void Initialize(Action initializationEndedCallback)
    {
        Vector3 center = SystemsManager.GetSystemOfType<Map>().MapCenter;
        _mapCenter = new GameObject("MapCenter").transform;
        _mapCenter.position = center;
        _mapCenter.SetParent(transform);
        SetTarget(_mapCenter);
    }
    public void SetTarget(Transform target)
    {
        _vCamera.LookAt = target;
        _vCamera.Follow = target;
    }
    public override bool AsyncInitialization => false;
}
