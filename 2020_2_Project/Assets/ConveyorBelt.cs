﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    private Animator _animator;
    [Header("0번이 왼쪽, 1번이 오른쪽")]
    public RuntimeAnimatorController[] belt;
    public Transform[] beltWheels;

    [Header("체크되어있으면 Left, 안되어있으면 Right")]
    [SerializeField] private bool _arrow = default;

    private Transform _playerPos;

    private List<Enemy> _enemies = new List<Enemy>();

    private bool _isOnPlayer;
    private int i;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    private void Start()
    {
        _playerPos = Player.instance.transform;
        if (_arrow)
            _animator.runtimeAnimatorController = belt[1];
        else
            _animator.runtimeAnimatorController = belt[0];
    }
    private void Update()
    {
        for (int i = 0; i < 3; i++)
        {
            if (_arrow) // left
                beltWheels[i].Rotate(0, 0, -3);
            else
                beltWheels[i].Rotate(0, 0, 3);
        }

        if(_isOnPlayer)
        {
            if (_arrow) // Left
                Player.instance.OnConveyorBelt(Vector2.right * 5.0f);
            //_playerPos.Translate(Vector2.left * 3.0f * Time.deltaTime);
            else
                Player.instance.OnConveyorBelt(Vector2.left * 5.0f);
                //_playerPos.Translate(Vector2.right * 3.0f * Time.deltaTime);
        }

        if (_enemies.Count != 0)
        {
            if(_arrow)
            {
                for (i = 0; i < _enemies.Count; i++)
                    _enemies[i].OnConveyorBelt(Vector2.right * 5.0f);
            }
            else
            {
                for (i = 0; i < _enemies.Count; i++)
                    _enemies[i].OnConveyorBelt(Vector2.left * 5.0f);
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
            _isOnPlayer = true;

        if (collision.CompareTag("enemy"))
            _enemies.Add(collision.GetComponent<Enemy>());
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            _isOnPlayer = false;

        if (collision.CompareTag("enemy"))
            _enemies.Remove(collision.GetComponent<Enemy>());
    }
}
