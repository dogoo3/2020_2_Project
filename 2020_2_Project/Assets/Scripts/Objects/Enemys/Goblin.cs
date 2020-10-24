using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : MonoBehaviour
{
    private Rigidbody2D _rigidbody2d;
    private Animator _animator;

    private RaycastHit2D _rayPlayer;
    private Vector2 _direction;

    private Enemy _enemy;

    private bool _isdetect; // 플레이어를 감지했을 때
    private bool _isDetectStart; // 땅을 밟아 플레이어 감지를 시작함
    private bool _isJump; // 점프할 때
    private bool _isIdle; // 가만히 서 있을 때
    private float _changedirTime; // 시점 변환 시간을 계산하는 변수
    private float _changeatkedTime; // 피격 후 경과 시간
    private bool _isattacked; // 공격 당했을 때
    private bool _isattack; // 공격했을 때

    [Header("플레이어에게 입힐 데미지")]
    [SerializeField] private int damage = default;

    [Header("기본 이동속도, 플레이어 감지 이동속도 및 점프력")]
    [SerializeField] private float moveSpeed = default;
    [SerializeField] private float runSpeed = default;
    [SerializeField] private float jumpforce = default;

    [Header("minpatrolTime ~ maxpatrolTime초 사이마다 진행 방향을 바꿈.")]
    [SerializeField] private float minpatrolTime = default;
    [SerializeField] private float maxpatrolTime = default;
    private float _patrolTime;
    [Header("rayLength 안의 플레이어를 감지함.")]
    [SerializeField] private float rayLength = default;
    private int _randAction;

    private void Awake()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _enemy = GetComponent<Enemy>();
    }

    private void OnEnable()
    {
        // Initialize
        _changedirTime = 0;
        _direction = Vector2.right;
        _animator.SetFloat("direction", _direction.x);
        SetPatrolTime();
    }

    private void Update()
    {
        if (_isDetectStart)
        {
            _changedirTime += Time.deltaTime;
            if (_isattacked)  // 피격 상태일 때
            {
                _changeatkedTime += Time.deltaTime;
                if (_changeatkedTime > 2.0f) // 피격 후 2초 경과 시 평상시 walk로 돌아간다.
                {
                    _isattacked = false; 
                    _animator.SetBool("attacked", _isattacked);
                    _enemy.SetDef(_isattacked);
                }
            }

            if (!_isdetect) // 플레이어 탐색 상태
            {
                if(!_isIdle && !_isattack)
                    transform.Translate(_direction * moveSpeed * Time.deltaTime);
                if(_changedirTime > _patrolTime)
                    ChangeMotion();
                if(!_isJump && !_isIdle)
                    DetectGround();

                if (Player.instance != null)
                {
                    if (!Player.instance.CheckAttacked()) // 플레이어가 무적상태인지 체크 
                    {
                        if (!_isattack) // 고블린이 공격이 가능한 상태여야 감지 가능!
                        {
                            if (Vector2.Distance(Player.instance.transform.position, transform.position) < rayLength) // 플레이어가 Ray의 거리 내에 있는지
                            {
                                _rayPlayer = Physics2D.Raycast(transform.position + Vector3.down * 1.4f, _direction, rayLength, 1 << LayerMask.NameToLayer("Player"));
                                if (_rayPlayer.collider != null)
                                    ChangeDetectMode(true, 1.5f); // 플레이어 감지 모드로 변경
                            }
                        }
                    }
                }
            }
            else // 플레이어를 감지할 때
            {
                transform.Translate(_direction * runSpeed * Time.deltaTime);

                if (_changedirTime > _patrolTime)
                    ChangeDetectMode(false, 1.0f);

                if (!_isJump)
                    DetectGround();

                if (!Player.instance.CheckAttacked()) // 플레이어가 피격후 무적상태면 감지하지 않는다. (무적상태 : true)
                {
                    if (Vector2.Distance(Player.instance.transform.position, transform.position) < 1.5f) // 플레이어가 특정범위 내로 들어왔을경우 고블린이 공격을 한다
                    {
                        Player.instance.Attacked(damage);
                        _animator.SetTrigger("attack");
                        _isdetect = false; // 플레이어 감지 해제
                        _isattack = true; // 공격모션을 취하는 동안 이동할 수 없음.
                    }
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("ground")) // 점프 후 땅에 닿았을 때
        {
            _rayPlayer = Physics2D.Raycast(transform.position, Vector2.down, 3f, 1 << LayerMask.NameToLayer("Ground")); // 벽 옆을 부딪쳤는지 체크
            if(_rayPlayer.collider != null) // 감지됐다 = 발바닥이 벽에 닿았다.
                _isJump = false;
        }
        if(collision.CompareTag("bullet")) // 총알에 맞았을 때
        {
            _isattacked = true;
            _animator.SetBool("attacked", _isattacked);
            _enemy.SetDef(_isattacked);
            _changeatkedTime = 0;
        }
        if (!_isDetectStart) // 생성 후 첫 감지
        {
            if (collision.CompareTag("ground"))
                _isDetectStart = true;
        }
    }

    private void DetectGround()
    {
        _rayPlayer = Physics2D.Raycast(transform.position, _direction, 3.0f, 1 << LayerMask.NameToLayer("Wall")); // 벽을 감지해서
        Debug.DrawRay(transform.position, _direction * 3.0f, Color.blue, 0.1f);
        if (_rayPlayer.collider != null) // 벽을 감지하면 무조건 시점전환.
        {
            ChangeDir();
        }

        _rayPlayer = Physics2D.Raycast(transform.position + (Vector3)_direction + (Vector3.down * 2.2f), Vector2.down, 0.5f, 1 << LayerMask.NameToLayer("Ground"));
        Debug.DrawRay(transform.position + (Vector3)_direction + (Vector3.down * 2.2f), Vector2.down * 0.5f, Color.red, 0.1f);
        if (_rayPlayer.collider == null) // 시선 앞에 땅이 없으면
        {
            _rayPlayer = Physics2D.Raycast(transform.position + Vector3.down * 1.6f, _direction, 2f, 1 << LayerMask.NameToLayer("Ground"));
            Debug.DrawRay(transform.position + Vector3.down * 1.6f, _direction * 2f, Color.green, 1.0f);
            if (_rayPlayer.collider != null) // 점프할 땅이 있으면
            {
                _randAction = Random.Range(0, 2); // 50%의 확률로
                if (_randAction == 0) // 점프한다
                {
                    _isJump = true;
                    _rigidbody2d.velocity = new Vector2(0, jumpforce);
                }
                else // 방향전환한다
                    ChangeDir();
                return;
            }
        }
        _rayPlayer = Physics2D.Raycast(transform.position + Vector3.down * 0.15f, _direction, 2.0f, 1 << LayerMask.NameToLayer("Ground"));
        Debug.DrawRay(transform.position + Vector3.down*0.15f, _direction * 2.0f, Color.yellow, 0.1f);
        if(_rayPlayer.collider != null)
        {
            _isJump = true;
            _rigidbody2d.velocity = new Vector2(0, jumpforce * 1.5f);
        }
    }

    private void ChangeMotion()
    {
        _randAction = Random.Range(0, 3);
        Debug.Log(_randAction);
        switch (_randAction)
        {
            case 0:
                ChangeDir();
                SetIdle(false);
                break;
            case 1:
                _isJump = true;
                SetIdle(false);
                _rigidbody2d.velocity = new Vector2(0, jumpforce);
                break;
            case 2:
                SetIdle(true);
                break;
        }
        SetPatrolTime();
        _changedirTime = 0;
    }

    private void ChangeDir()
    {
        _direction *= -1;
        _animator.SetFloat("direction", _direction.x);
    }

    private void ChangeDetectMode(bool _is, float _speed)
    {
        _isdetect = _is;
        _animator.SetFloat("run", _speed);
        _changedirTime = 0;
        SetPatrolTime();
    }

    private void SetIdle(bool _is)
    {
        _isIdle = _is;
        _animator.SetBool("idle", _isIdle);
    }

    private void SetPatrolTime()
    {
        _patrolTime = Random.Range(minpatrolTime, maxpatrolTime);
    }

    public void ActiveDeteteStart() // Animation Func, 모든 행동을 정지할 때 사용.
    {
        _isDetectStart = true;
    }

    public void AttackChange() // Animation Func
    {
        _isattack = false;
    }

    public void Delete() // Animation Func
    {
        gameObject.SetActive(false);
    }
}
