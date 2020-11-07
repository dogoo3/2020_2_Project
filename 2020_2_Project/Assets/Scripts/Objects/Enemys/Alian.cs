using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alian : MonoBehaviour
{
    private Rigidbody2D _rigidbody2d;
    private Animator _animator;
    private Enemy _enemy;

    private Transform _playerPos;

    [SerializeField] private float _detectRange = default, _moveSpeed = default;
    
    private bool _isDetectStart, _isattacked, _isattack, _isJump, _isattackCool;

    private float _maxHP, _attackCooltime;

    private void Awake()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _enemy = GetComponent<Enemy>();
        _playerPos = Player.instance.transform;
    }

    private void OnEnable()
    {
        _isDetectStart = false;
    }

    private void Start()
    {
        _maxHP = _enemy.GetHP();
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
                    _animator.SetTrigger("attack");
                    _isattack = true;
                    _isattackCool = true;
                    Invoke("SetAttackCooltime", _attackCooltime);
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
    
    private void SetAttackCooltime() // Invoke Func
    {
        _isattackCool = false;
    }
}
