using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Grenade : MonoBehaviour
{
    private Rigidbody2D _rigidbody2d;

    public float damage;
    public float shotSpeed;
    public float surviveTime;

    private float _elapsedTime;
    private Vector2 _direction;

    private void Awake()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _direction = Vector2.up;
    }

    public void Throw(Vector2 _direction)
    {
        this._direction.x = _direction.x;
        _rigidbody2d.velocity = this._direction.normalized * shotSpeed;
        //_rigidbody2d.AddForce(_direction*shotSpeed, ForceMode2D.Impulse);
    }

    private void Update()
    {
        if (_elapsedTime > surviveTime)
            ObjectPoolingManager.instance.InsertQueue_grenade(this);
        _elapsedTime += Time.deltaTime;
    }

    private void OnDisable()
    {
        _elapsedTime = 0;
    }
}
