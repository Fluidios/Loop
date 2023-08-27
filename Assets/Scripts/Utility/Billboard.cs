using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform _cameraTransform;
    private Vector3 _camPos;
    private void Start()
    {
        _cameraTransform = SystemsManager.GetSystemOfType<CameraSystem>().MainCamera.transform;
    }

    private void LateUpdate()
    {
        _camPos = _cameraTransform.position;
        _camPos.y = 0;

        transform.LookAt(_camPos);
    }
}
