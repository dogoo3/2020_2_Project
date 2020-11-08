﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldTrap : MonoBehaviour
{
    [SerializeField] private float _recoveryTime = default, _damage = default;
    [Header("0번은 왼쪽, 1번은 오른쪽")]
    [SerializeField] Transform[] _blade;

    private bool _isdetect = true;
    private float _elapsedTime, _angle;
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
        }

        if(!_isdetect)
        {
            _elapsedTime += Time.deltaTime;
            if (_angle <= 90)
            {
                _blade[0].rotation = Quaternion.Euler(0, 0, -_angle);
                _blade[1].rotation = Quaternion.Euler(0, 0, _angle);
                _angle += 6;
            }
        }

        if(_elapsedTime > _recoveryTime)
        {
            _isdetect = true;
            if (_angle > 0)
            {
                _angle -= 3;
                _blade[0].rotation = Quaternion.Euler(0, 0, -_angle);
                _blade[1].rotation = Quaternion.Euler(0, 0, _angle);
            }
            else
                _elapsedTime = 0f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if (_isdetect)
            {
                _isdetect = false;
                // Player.instance.Attacked(_damage);
            }
        }
    }
}
