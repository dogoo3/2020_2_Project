using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_SG : MonoBehaviour
{
    private Bullet _sg;

    public float damage;
    public float shotSpeed;
    public float surviveTime;

    private void Awake()
    {
        _sg = new B_SG();
        _sg.damage = FileManager.weaponInfo["sg_deal"];
        _sg.shotSpeed = shotSpeed;
        _sg.surviveTime = surviveTime;
    }

    private void Update()
    {
        if (_sg.CheckElapsedTime())
            ObjectPoolingManager.instance.InsertQueue(this, ObjectPoolingManager.instance.queue_sg);
        transform.Translate(_sg.Move());
    }

    private void OnDisable()
    {
        _sg.ResetElapsedTime();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "ground":
            case "wall":
                ObjectPoolingManager.instance.InsertQueue(this, ObjectPoolingManager.instance.queue_sg);
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "enemy":
                _sg.tempEnemy = collision.gameObject.GetComponent<Enemy>();
                _sg.tempEnemy.MinusHP(_sg.damage);
                _sg.tempEnemy.Knockback(_sg._direction);
                ObjectPoolingManager.instance.InsertQueue(this, ObjectPoolingManager.instance.queue_sg);
                break;
        }
    }

    public void Direction(Vector2 _direction)
    {
        _sg.Direction(_direction);
    }
}
