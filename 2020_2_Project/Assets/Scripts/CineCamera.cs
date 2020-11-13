using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class CineCamera : MonoBehaviour
{
    public static CineCamera instance;

    private CinemachineVirtualCamera _virtualcamera;
    private CinemachineFramingTransposer _transposer;

    private Vector2 _offsetpos;
    private float _elapsedTime;

    private void Awake()
    {
        instance = this;
        _virtualcamera = GetComponent<CinemachineVirtualCamera>();
        _transposer = _virtualcamera.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    public void SetOffset(Vector2 _depPos, Vector2 _arvPos)
    {
        _elapsedTime = 0f;
        _offsetpos = _depPos - _arvPos;
        _transposer.m_TrackedObjectOffset = _offsetpos;
    }

    private float CalcT()
    {
        _elapsedTime += Time.deltaTime;
        if (_elapsedTime < 1.0f)
            return 2.0f * (_elapsedTime * _elapsedTime + _elapsedTime);
        else
            return 1.0f;
    }

    private void Update()
    {
        if (_transposer.m_TrackedObjectOffset != Vector3.zero)
            _transposer.m_TrackedObjectOffset = Vector2.Lerp(_offsetpos,Vector2.zero, CalcT());
    }
}
