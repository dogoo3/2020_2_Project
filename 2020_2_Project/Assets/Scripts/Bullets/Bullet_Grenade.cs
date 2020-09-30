using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Grenade : MonoBehaviour
{
    private Rigidbody2D _rigidbody2d;

    private Bullet _grenade;
    private Vector2 _direction;
    private Enemy _tempEnemy;
    private Collider2D[] _tempColliders;

    private int i;

    public float damage;
    public float shotSpeed;
    public float surviveTime;

    public float bombRadius; // 폭발 반경
    private float _bombRadius;

    private void Awake()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _direction = Vector2.up;

        _grenade = new B_Grenade();
        _grenade.damage = FileManager.weaponInfo["grenade_deal"];
        _grenade.shotSpeed = shotSpeed;
        _grenade.surviveTime = surviveTime;

        _bombRadius = 1 / bombRadius;
    }

    public void Throw(Vector2 _direction)
    {
        _rigidbody2d.velocity =_grenade.Throw(_direction).normalized * _grenade.shotSpeed;
    }

    private void Update()
    {
        if (_grenade.CheckElapsedTime())
        {
            _tempColliders = Physics2D.OverlapCircleAll(transform.position, bombRadius, 1 << LayerMask.NameToLayer("Enemy"));
            for (i = 0; i < _tempColliders.Length; i++)
            {
                _tempEnemy = _tempColliders[i].GetComponent<Enemy>();
                _tempEnemy.MinusHP(Mathf.Abs(Vector2.Distance(transform.position, _tempEnemy.transform.position) * _bombRadius - 1) * _grenade.damage);
            }
            ObjectPoolingManager.instance.InsertQueue(this, ObjectPoolingManager.instance.queue_grenade);
        }
        _grenade.LoadElapsedTime();
    }

    private void OnDisable()
    {
        _grenade.ResetElapsedTime();
    }
}
