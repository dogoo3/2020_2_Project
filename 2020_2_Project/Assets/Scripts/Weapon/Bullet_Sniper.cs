﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Sniper : MonoBehaviour
{
    private Bullet _sniper;

    public float damage;
    public float shotSpeed;
    public float surviveTime;

    private void Awake()
    {
        _sniper = new B_Sniper();
        _sniper.damage = damage;
        _sniper.shotSpeed = shotSpeed;
        _sniper.surviveTime = surviveTime;
    }

    private void Update()
    {
        if (_sniper.CheckElapsedTime())
            ObjectPoolingManager.instance.InsertQueue_sniper(this);
        transform.Translate(_sniper.Move());
    }

    private void OnDisable()
    {
        _sniper.ResetElapsedTime();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "ground":
            case "bullet":
                ObjectPoolingManager.instance.InsertQueue_sniper(this);
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "enemy":
                _sniper.tempEnemy = collision.gameObject.GetComponent<Enemy>();
                _sniper.tempEnemy.MinusHP(_sniper.damage);
                ObjectPoolingManager.instance.InsertQueue_sniper(this);
                break;
        }
    }

    public void Direction(Vector2 _direction)
    {
        _sniper.Direction(_direction);
    }
}
