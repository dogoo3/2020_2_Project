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

    private float _maxHP, _elapsedtime, _elapsedChangetime;

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
        if (!_isattack)
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
                        if (!_isJump)
                        {
                            _animator.SetTrigger("attack");
                            _isattack = true;
                        }
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

        if(_enemy._isdetect)
        {
            _elapsedtime += Time.deltaTime;
            _elapsedChangetime += Time.deltaTime;
            if(_elapsedtime > _enemy._detectTime)
            {
                _elapsedtime = 0f;
                _enemy._isdetect = false;
            }

            if(_elapsedChangetime > 1.0f)
            {
                _elapsedChangetime = 0f;
                ChangeDir();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("bullet"))
        {
            if (collision.gameObject.GetComponent<Bullet_Alian>() == null) // 외계인들끼리 쏜 총알에는 당연히 반응하지 않는다.
            {
                _isattacked = true;
                _animator.SetTrigger("attacked");
            }
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

        // 공격당했으므로 위치연산 진행
        _enemy._isdetect = true;
        _elapsedtime = 0; // 피격 경과 시간 초기화
        _elapsedChangetime = 0f; // 일정주기마다 플레이어 방향으로 시선 변경하는 시간 초기화

        ChangeDir();
    }

    private void ChangeDir()
    {
        if (!_isJump) // 점프중일때는 방향전환을 하지 않는다.
        {
            if (_playerPos.position.x > transform.position.x) // 외계인의 오른쪽에 플레이어가 있을 경우
            {
                if (_enemy._direction == Vector2.left) // 왼쪽을 보고 있으면
                    _enemy.ChangeDir(); // 방향전환
            }
            else // 외계인의 왼쪽에 플레이어가 있을 경우
            {
                if (_enemy._direction == Vector2.right) // 오른쪽을 보고 있으면
                    _enemy.ChangeDir();
            }
        }
    }
    public void SetFalseIsAttack() // Animator Func
    {
        _isattack = false;
    }
    public void SetFalseIsJump() // Animator Func
    {
        _isJump = false;
    }
    public void SetTrueIsJump() // Animator Func
    {
        _isJump = true;
    }
    public void Dead() // Animator Func
    {
        gameObject.SetActive(false);
    }
    public void ShootMotion() // Animation Func
    {
        ObjectPoolingManager.instance.GetQueue_alienBullet(muzzleGunPos.position, _enemy._direction);
    }

    private void SetAttackCooltime() // Invoke Func
    {
        _isattackCool = false;
    }
}
