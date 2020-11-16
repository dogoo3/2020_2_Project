﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Action
{
    Jump,
    ChangeDir,
    Random
}

enum Arrow
{
    Left,
    right
}

public class ChangeActionPoint : MonoBehaviour
{
    [Header("이 방향에서 접근했다.")]
    [SerializeField] private Arrow _arrow = default;
    [Header("액션의 종류")]
    [SerializeField] private Action _action = default;

    private Enemy _enemy;
    [Header("점프 강도")]
    [SerializeField] private Vector2 _force = default;

    private Vector2 _cal = Vector2.left + Vector2.up;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 부딪친 애가 적인지를 판별.
        if (collision.CompareTag("enemy"))
            _enemy = collision.GetComponent<Enemy>();
        else
            return;

        if (_enemy.GetVelocityForce() == Vector2.zero) // 점프 상태 및 낙하중일 때는 적용되지 않는다.
        {
            // 플레이어 감지 상태일때 액션 변환
            if(_enemy._isdetect)
            {
                if(transform.position.x > collision.transform.position.x) // 객체가 왼쪽에서 접근했다.
                {
                    if (_arrow == Arrow.right)
                        return;

                    DetectMotion(true);
                }
                else // 객체가 오른쪽에서 접근했다.
                {
                    if (_arrow == Arrow.Left)
                        return;

                    DetectMotion(false);
                }
                return;
            }

            // 플레이어 미감지 상태일 때 액션 변환
            if (transform.position.x > collision.transform.position.x) // 객체가 왼쪽에서 접근했다.
            {
                if (_arrow == Arrow.right)
                    return;

                Motion(true);
            }
            else // 객체가 오른쪽에서 접근했다.
            {
                if (_arrow == Arrow.Left)
                    return;

                Motion(false);
            }
        }
    }

    private void Motion(bool _is)
    {
        switch (_action)
        {
            case Action.Jump:
                if (_is)
                    _enemy.Jump(_force);
                else
                    _enemy.Jump(_force * _cal);
                break;
            case Action.ChangeDir:
                _enemy.ChangeDir();
                break;
            case Action.Random:
                if (Random.Range(0, 2) == 0)
                {
                    if (_is)
                        _enemy.Jump(_force);
                    else
                        _enemy.Jump(_force * _cal);
                }
                else
                    _enemy.ChangeDir();
                break;
        }
    }

    private void DetectMotion(bool _is)
    {
        switch (_action)
        {
            case Action.Jump:
            case Action.Random:
                if (_is)
                    _enemy.Jump(_force);
                else
                    _enemy.Jump(_force * _cal);
                break;
            case Action.ChangeDir:
                _enemy.ChangeDir();
                break;
        }
    }

}
