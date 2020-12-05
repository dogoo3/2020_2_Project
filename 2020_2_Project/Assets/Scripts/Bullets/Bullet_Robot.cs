using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Robot : MonoBehaviour
{
    private Rigidbody2D _rigidbody2d;
    private Animator _animator;

    private Bullet _grenade;
    private Vector2 _direction;
    private Enemy _tempEnemy;
    private Collider2D _tempCollider;

    private bool _isbomb;

    private int i;

    public float damage;
    public float shotSpeed;
    public float surviveTime;

    public float bombRadius; // 폭발 반경
    private float _bombRadius;

    private void Awake()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();

        _direction = Vector2.up;

        _grenade = new B_Robot();
        _grenade.damage = damage;
        _grenade.shotSpeed = shotSpeed;
        _grenade.surviveTime = surviveTime;

        _bombRadius = 1 / bombRadius;
    }

    public void Throw(Vector2 _direction)
    {
        _rigidbody2d.angularVelocity = 300.0f * -_direction.x;
        _rigidbody2d.velocity = _grenade.Throw(_direction).normalized * _grenade.shotSpeed;
    }

    private void Update()
    {
        if (!_isbomb)
        {
            if (_grenade.CheckElapsedTime())
            {
                _isbomb = true;
                _rigidbody2d.freezeRotation = true;
                transform.rotation = Quaternion.identity;
                _tempCollider = Physics2D.OverlapCircle(transform.position, bombRadius, 1 << LayerMask.NameToLayer("Player"));
                if (_tempCollider != null)
                    Player.instance.Attacked(Mathf.Abs(Vector2.Distance(transform.position, _tempCollider.transform.position) * _bombRadius - 1) * _grenade.damage);
                _animator.SetTrigger("bomb");
                SoundManager.instance.PlaySFX("bombgre_deadrobot");
            }
            _grenade.LoadElapsedTime();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
            _rigidbody2d.velocity *= 0.2f;
    }

    private void OnDisable()
    {
        _grenade.ResetElapsedTime();
        _isbomb = false;
        _rigidbody2d.freezeRotation = false;
        _animator.Rebind();
    }

    public void InsertQueue() // Animation Func
    {
        ObjectPoolingManager.instance.InsertQueue(this, ObjectPoolingManager.instance.queue_robotgrenade);
    }
}
