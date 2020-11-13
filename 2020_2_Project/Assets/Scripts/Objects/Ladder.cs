using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    private BoxCollider2D _collider;

    private float _upPos, _downPos;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        _upPos = transform.position.y+1.0f + transform.localScale.y * _collider.size.y * 0.5f; // 최고점 연산
        _downPos = transform.position.y - transform.localScale.y * _collider.size.y * 0.5f; // 최저점 연산
    }

    public bool CarculateUpPos(Transform _transform)
    {
        if (_transform.position.y >= _upPos) // 플레이어가 올라가다가 밧줄의 최고점에 다다랐을 경우
            return true;
        else
            return false;
    }

    public bool CarculateDownPos(Transform _transform)
    {
        if (_transform.position.y <= _downPos) // 플레이어가 내려가다가 밧줄의 최저점에 다다랐을 경우
            return true;
        else
            return false;
    }
}
