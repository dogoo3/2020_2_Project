using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : MonoBehaviour
{
    private Rigidbody2D _rigidbody2d;
    private BoxCollider2D _collider2d;
    private Animator _animator;
    private Enemy _enemy;

    private Transform _playerPos;
    private Collider2D _changemotionpoint;
    
    [SerializeField] private Transform _muzzleGunPos = default;
    [SerializeField] private float _detectRange = default, _moveSpeed = default;
    [SerializeField] private float _attackCooltime = default;
    [SerializeField] private float _detectTime = default;
    private bool _isDetectStart, _isJump, _isattackCool, _isonConveyorBelt, _israge;

    private float _maxHP, _elapsedtime, _elapsedChangetime, _elapsedAttackCooltime;

    private void Awake()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _collider2d = GetComponent<BoxCollider2D>();
        _animator = GetComponent<Animator>();
        _enemy = GetComponent<Enemy>();
    }

    private void OnEnable()
    {
        _isDetectStart = false;
        _isattackCool = true;
        _animator.SetFloat("direction", 1.0f);
        _animator.SetFloat("israge", 0.0f);
        // _isRage true시 능력치 원상복구
    }

    private void Start()
    {
        _maxHP = _enemy.GetHP();
        _playerPos = Player.instance.transform;
    }

    private void Update()
    {
        if(_isDetectStart)
        {
            if(!_enemy.isattack && !_enemy.isattacked)
                transform.Translate(_enemy._direction * _moveSpeed * Time.deltaTime);
            else // 피격중이라면
            {
                if (_enemy.isjump) // 점프중이라면 이동하고, 점프중이 아니라면 이동하지않음.
                    transform.Translate(_enemy._direction * _moveSpeed * Time.deltaTime);
            }
        }

        if (Vector2.Distance(_playerPos.position, transform.position) <= _detectRange)
        {
            if (!_isJump)
            {
                if (!_isattackCool)
                {
                    // 진행방향에 플레이어가 존재해야만 사격하고, 판정시점에 플레이어가 없으면 쿨타임만 자동으로 돈다.
                    if (_enemy._direction == Vector2.right && transform.position.x < _playerPos.position.x ||
                        _enemy._direction == Vector2.left && transform.position.x > _playerPos.position.x)
                    {
                        _animator.SetTrigger("attack");
                        SoundManager.instance.PlaySFX("alien_attack");
                        _enemy.isattack = true;
                        _isattackCool = true;
                    }
                    else
                        _isattackCool = true;
                }
                else
                {
                    _elapsedAttackCooltime += Time.deltaTime;
                    if (_elapsedAttackCooltime >= _attackCooltime)
                    {
                        _isattackCool = false;
                        _elapsedAttackCooltime = 0;
                    }
                }
            }
        }

        if (_enemy.isdetect)
        {
            _elapsedtime += Time.deltaTime;
            _elapsedChangetime += Time.deltaTime;
            if (_elapsedtime > _detectTime)
            {
                _elapsedtime = 0f;
                _enemy.isdetect = false;
            }

            if (_elapsedChangetime > 1.0f)
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
                if(!_israge)
                {
                    if (_enemy.GetRemainHPRate() <= 0.5f) // Hp가 절반 아래로 내려가면
                    {
                        _israge = true;
                        _animator.SetFloat("israge", 1.0f);
                        // 능력치 변경사항 작성
                    }
                }
                ChangeDir();

                if (!_isJump)
                {
                    _animator.SetTrigger("attack");
                    SoundManager.instance.PlaySFX("alien_Attack");
                    _enemy.isattack = true;
                    _isattackCool = true;
                }
            }
        }

        if(collision.CompareTag("ground"))
        {
            if (!_isDetectStart)
                _isDetectStart = true;
            if (_isJump)
            {
                _isJump = false;
                CheckChangeMotionPoint();
            }
        }
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

            CheckChangeMotionPoint();
        }
    }

    public void CheckChangeMotionPoint()
    {
        _changemotionpoint = Physics2D.OverlapBox((Vector2)transform.position + _collider2d.offset,
       _collider2d.size, 0, 1 << LayerMask.NameToLayer("ChangePoint")); // (groundcollider box offset, size, angle)
        if (_changemotionpoint != null)
        {
            ChangeActionPoint _point = _collider2d.GetComponent<ChangeActionPoint>();
            if (_point != null)
            {
                if (_enemy.isdetect)
                    _point.CheckDetectMotion(transform);
                else
                    _point.CheckMotion(transform);
            }
        }
    }

    public void SetFalseIsAttacked() // Animator Func
    {
        _enemy.isattacked = false;

        // 공격당했으므로 위치연산 진행
        _enemy.isdetect = true;
        _elapsedtime = 0; // 피격 경과 시간 초기화
        _elapsedChangetime = 0f; // 일정주기마다 플레이어 방향으로 시선 변경하는 시간 초기화

        ChangeDir();
    }
    public void SetFalseIsAttack() // Animator Func
    {
        _enemy.isattack = false;
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
        SoundManager.instance.PlaySFX("deadalien");
    }
    public void ShootMotion() // Animation Func
    {
        ObjectPoolingManager.instance.GetQueue_alienBullet(_muzzleGunPos.position, _enemy._direction);
    }
}
