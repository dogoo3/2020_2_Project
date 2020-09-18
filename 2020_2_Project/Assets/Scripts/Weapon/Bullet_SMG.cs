using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_SMG : MonoBehaviour
{
    private Bullet _smg;

    public float damage;
    public float shotSpeed;
    public float surviveTime;

    private void Awake()
    {
        _smg = new B_SMG();
        _smg.damage = damage;
        _smg.shotSpeed = shotSpeed;
        _smg.surviveTime = surviveTime;
    }

    private void Update()
    {
        if (_smg.CheckElapsedTime())
            ObjectPoolingManager.instance.InsertQueue_smg(this);
        transform.Translate(_smg.Move());
    }

    private void OnDisable()
    {
        _smg.ResetElapsedTime();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "ground":
            case "bullet":
                ObjectPoolingManager.instance.InsertQueue_smg(this);
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "enemy":
                _smg.tempEnemy = collision.gameObject.GetComponent<Enemy>();
                _smg.tempEnemy.MinusHP(_smg.damage);
                ObjectPoolingManager.instance.InsertQueue_smg(this);
                break;
        }
    }

    public void Direction(Vector2 _direction)
    {
        _smg.Direction(_direction);
    }
}
