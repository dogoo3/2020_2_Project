using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alian : MonoBehaviour
{
    private Rigidbody2D _rigidbody2d;
    private Animator _animator;
    private Enemy _enemy;

    private Transform _playerPos;

    [SerializeField] private Transform muzzleGunPos = default;
    [SerializeField] private float _detectRange = default, _moveSpeed = default;
    [SerializeField] private float _attackCooltime = default;
    private bool _isDetectStart, _isattacked, _isattack, _isJump, _isattackCool;

    private float _maxHP;

    private void Awake()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _enemy = GetComponent<Enemy>();
    }

    private void OnEnable()
    {
        _isDetectStart = false;
        _enemy._direction = Vector2.right;
    }

    private void Start()
    {
        _maxHP = _enemy.GetHP();
        _playerPos = Player.instance.transform;
    }

    private void Update()
    {
        if (!_isattacked && !_isattack)
            transform.Translate(_enemy._direction * _moveSpeed * Time.deltaTime);

        if(Vector2.Distance(_playerPos.position,transform.position) <= _detectRange)
        {
            if (!_isJump)
            {
                if (!_isattackCool)
                {
                    // 진행방향에 플레이어가 존재해야만 사격하고, 판정시점에 플레이어가 없으면 쿨타임만 자동으로 돈다.
                    if(_enemy._direction == Vector2.right && transform.position.x < _playerPos.position.x ||
                        _enemy._direction == Vector2.left && transform.position.x > _playerPos.position.x)
                    {
                        _animator.SetTrigger("attack");
                        _isattack = true;
                        _isattackCool = true;
                        Invoke("SetAttackCooltime", _attackCooltime);
                    }
                    else
                    {
                        _isattackCool = true;
                        Invoke("SetAttackCooltime", _attackCooltime);
                    }
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("bullet"))
        {
            _isattacked = true;
            _animator.SetTrigger("attacked");
        }
        if(!_isDetectStart)
        {
            if (collision.CompareTag("ground"))
                _isDetectStart = true;
        }
    }

    public void SetFalseIsAttacked() // Animator Func
    {
        _isattacked = false;
    }
    public void SetFalseIsAttack() // Animator Func
    {
        _isattack = false;
    }
    public void SetFalseIsJump() // Animator Func
    {
        _isJump = false;
    }
    public void Dead() // Animator Func
    {
        gameObject.SetActive(false);
    }
    private void SetAttackCooltime() // Invoke Func
    {
        _isattackCool = false;
    }

    public void ShootMotion() // Animation Func
    {
        ObjectPoolingManager.instance.GetQueue_alienBullet(muzzleGunPos.position, _enemy._direction);
    }
}
