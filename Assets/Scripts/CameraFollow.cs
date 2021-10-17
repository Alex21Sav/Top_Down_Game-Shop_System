using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _target;

    private Transform _cameraPosition;
    private Vector3 _offSet;

    private void Start()
    {
        _cameraPosition = Camera.main.transform;
        _offSet = _cameraPosition.position - _target.position;
    }

    private void LateUpdate()
    {
        _cameraPosition.position = _target.position + _offSet;
    }
}
