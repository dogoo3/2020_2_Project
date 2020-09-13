using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_SG : MonoBehaviour
{
    public float damage;
    public float shotSpeed;
    public float surviveTime;

    private float _elapsedTime;
    private Vector2 _direction;

    public void Direction(Vector2 _direction)
    {
        this._direction = _direction;
    }

    private void Update()
    {
        if (_elapsedTime > surviveTime)
            ObjectPoolingManager.instance.InsertQueue_sg(this);
        transform.Translate(_direction * shotSpeed * Time.deltaTime);
        _elapsedTime += Time.deltaTime;
    }

    private void OnDisable()
    {
        _elapsedTime = 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "ground":
            case "bullet":
                ObjectPoolingManager.instance.InsertQueue_sg(this);
                break;
        }
    }
}
