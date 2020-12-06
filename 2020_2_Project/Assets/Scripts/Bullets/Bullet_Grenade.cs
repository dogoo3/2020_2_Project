using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Grenade : MonoBehaviour
{
    private Rigidbody2D _rigidbody2d;
    private CircleCollider2D _collider2d;
    private Animator _animator;

    private Bullet _grenade;
    private Vector2 _direction;
    private Enemy _tempEnemy;
    private Collider2D[] _tempColliders;

    private bool _isbomb;

    private int i;

    public float damage;
    public float shotSpeed;
    public float surviveTime;

    public float bombRadius; // 폭발 반경
    private float _bombRadius;
    private bool _isground;

    private void Awake()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _collider2d = GetComponent<CircleCollider2D>();
        _animator = GetComponent<Animator>();

        _direction = Vector2.up;

        _grenade = new B_Grenade();
        _grenade.damage = FileManager.weaponInfo["grenade_deal"];
        _grenade.shotSpeed = shotSpeed;
        _grenade.surviveTime = surviveTime;

        _bombRadius = 1 / bombRadius;
    }

    public void Throw(Vector2 _direction)
    {
        _rigidbody2d.angularVelocity = 300.0f * -_direction.x;
        _rigidbody2d.velocity =_grenade.Throw(_direction).normalized * _grenade.shotSpeed;
    }

    private void Update()
    {
        if(!_isbomb)
        {
            if (_grenade.CheckElapsedTime())
            {
                _isbomb = true;
                _rigidbody2d.freezeRotation = true;
                transform.rotation = Quaternion.identity;
                _tempColliders = Physics2D.OverlapCircleAll(transform.position, bombRadius, 1 << LayerMask.NameToLayer("Enemy"));
                for (i = 0; i < _tempColliders.Length; i++)
                {
                    _tempEnemy = _tempColliders[i].GetComponent<Enemy>();
                    _tempEnemy.MinusHP(Mathf.Abs(Vector2.Distance(transform.position, _tempEnemy.transform.position) * _bombRadius - 1) * _grenade.damage);
                }
                _animator.SetTrigger("bomb");
                _collider2d.enabled = false;
                SoundManager.instance.PlaySFX("bombgre_deadrobot");
            }
            else
            {
                _grenade.LoadElapsedTime();
                if (_isground)
                    _rigidbody2d.velocity -= _direction;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            _rigidbody2d.velocity *= 0.2f;
            _isground = true;
            Invoke("Stop", 1.0f);
        }
    }

    private void OnDisable()
    {
        _grenade.ResetElapsedTime();

        _isbomb = false;
        _collider2d.enabled = true;
        _rigidbody2d.freezeRotation = false;
        _isground = false;
        _animator.Rebind();
    }

    public void InsertQueue() // Animation Func
    {
        ObjectPoolingManager.instance.InsertQueue(this, ObjectPoolingManager.instance.queue_grenade);
    }
    private void Stop()
    {
        _rigidbody2d.velocity = Vector2.zero;
        _rigidbody2d.angularVelocity = 0f;
    }
}
