using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Pistol : MonoBehaviour
{
    private Bullet _pistol;

    public float damage;
    public float shotSpeed;
    public float surviveTime;

    private void Awake()
    {
        _pistol = new B_Pistol();
        _pistol.damage = damage;
        _pistol.shotSpeed = shotSpeed;
        _pistol.surviveTime = surviveTime;
    }

    private void Update()
    {
        if (_pistol.CheckElapsedTime())
            ObjectPoolingManager.instance.InsertQueue_pistol(this);
        transform.Translate(_pistol.Move());
    }

    private void OnDisable()
    {
        _pistol.ResetElapsedTime();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "ground":
            case "bullet":
                ObjectPoolingManager.instance.InsertQueue_pistol(this);
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "enemy":
                _pistol.tempEnemy = collision.gameObject.GetComponent<Enemy>();
                _pistol.tempEnemy.MinusHP(_pistol.damage);
                ObjectPoolingManager.instance.InsertQueue_pistol(this);
                break;
        }
    }

    public void Direction(Vector2 _direction)
    {
        _pistol.Direction(_direction);
    }
}
