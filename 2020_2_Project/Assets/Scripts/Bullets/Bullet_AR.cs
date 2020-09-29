using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_AR : MonoBehaviour
{
    private Bullet _ar;

    public float damage;
    public float shotSpeed;
    public float surviveTime;

    private void Awake()
    {
        _ar = new B_AR();
        _ar.damage = damage;
        _ar.shotSpeed = shotSpeed;
        _ar.surviveTime = surviveTime;
    }

    private void Update()
    {
        if (_ar.CheckElapsedTime())
            ObjectPoolingManager.instance.InsertQueue(this, ObjectPoolingManager.instance.queue_ar);
        transform.Translate(_ar.Move());
    }

    private void OnDisable()
    {
        _ar.ResetElapsedTime();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "ground":
            case "bullet":
            case "wall:":
                ObjectPoolingManager.instance.InsertQueue(this, ObjectPoolingManager.instance.queue_ar);
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "enemy":
                _ar.tempEnemy = collision.gameObject.GetComponent<Enemy>();
                _ar.tempEnemy.MinusHP(_ar.damage);
                ObjectPoolingManager.instance.InsertQueue(this, ObjectPoolingManager.instance.queue_ar);
                break;
        }
    }

    public void Direction(Vector2 _direction)
    {
        _ar.Direction(_direction);
    }
}
