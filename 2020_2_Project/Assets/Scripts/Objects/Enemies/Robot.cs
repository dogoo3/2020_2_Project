using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour
{
    private Rigidbody2D _rigidbody2d;
    private BoxCollider2D _collider2d;
    private Animator _animator;
    private Collider2D _changemotionpoint;
    private Enemy _enemy;

    private RaycastHit2D _rayPlayer;

    private Transform _playerPos;

    [SerializeField] private Transform _handpos = default;
    [SerializeField] private float _moveSpeed = default;
    [SerializeField] private float _attackCooltime = default;
    [SerializeField] private float _attackRange = default;

    private bool _isDetectStart;

    private float _elapseAttackCooltime;

    private void Awake()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _collider2d = GetComponent<BoxCollider2D>();
        _enemy = GetComponent<Enemy>();
    }

    private void Start()
    {
        _playerPos = Player.instance.transform;
    }

    private void OnEnable()
    {
        _isDetectStart = false;
        _elapseAttackCooltime = 0f;
        _enemy.isattack = false;
        _enemy.isattacked = false;
    }

    private void Update()
    {
        if (_isDetectStart) // 활성화 후 처음 땅에 닿기 전까진 어떠한 액션도 취하지 않는다.
        {
            // Move
            if (!_enemy.isattack && !_enemy.isattacked) // walk / jump 상태
                transform.Translate(_enemy._direction * _moveSpeed * Time.deltaTime);
            else // 피격중이거나 공격중일 때
            {
                if (_enemy.isjump) // 점프중이라면 이동하고, 점프중이 아니라면 이동하지않음.
                    transform.Translate(_enemy._direction * _moveSpeed * Time.deltaTime);
            }
            // Detect Player(attack)
            if (!_enemy.isjump)
                _elapseAttackCooltime += Time.deltaTime;
            if (_elapseAttackCooltime >= _attackCooltime) // 공격 쿨타임을 넘기면
            {
                _rayPlayer = Physics2D.Raycast(transform.position + Vector3.down, _enemy._direction, _attackRange, 1 << LayerMask.NameToLayer("Player"));
                if (_rayPlayer.collider != null)
                {
                    _animator.SetTrigger("attack");
                    _enemy.isattack = true;
                    _elapseAttackCooltime = 0f;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(_enemy.isattack)
        {
            if (collision.CompareTag("bullet"))
                _enemy.isattack = false;
        }
        if(!_isDetectStart)
        {
            if (collision.CompareTag("ground"))
                _isDetectStart = true;
        }
    }

    public void CheckChangeMotionPoint()
    {
        _changemotionpoint = Physics2D.OverlapBox((Vector2)transform.position + _collider2d.offset,
            _collider2d.size, 0, 0, 1 << LayerMask.NameToLayer("ChangePoint"));
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

    public void Dead() // Animator Func
    {
        gameObject.SetActive(false);
    }

    public void FalseAttack() // Animation Func
    {
        _enemy.isattack = false;
    }
    public void FalseAttacked() // Animation Func
    {
        _enemy.isattacked = false;
        if(!_enemy.isjump) // 점프중이 아니라면 플레이어 방향으로 시점을 바꾼다.
        {
            if(_enemy._direction == Vector2.left) // 왼쪽을 바라보고 있을 때
            {
                if (transform.position.x < _playerPos.position.x) // 적의 오른쪽에 플레이어가 있으면 시점변환
                    _enemy.ChangeDir();
            }
            else // 오른쪽을 바라보고 있을 때
            {
                if (transform.position.x > _playerPos.position.x) // 적의 왼쪽에 플레이어가 있으면 
                    _enemy.ChangeDir();
            }
        }
        CheckChangeMotionPoint();
    }
    public void ShootMotion()
    {
        ObjectPoolingManager.instance.GetQueue_RobotGrenade(_handpos.position, _enemy._direction);
    }
}
