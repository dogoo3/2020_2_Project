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
    private float _changeatkTime;
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
        _changeatkTime = 0;
        _direction = Vector2.right;
        _animator.SetFloat("direction", _direction.x);
        SetPatrolTime();
    }

    private void Update()
    {
        if(_isDetectStart)
        {
            _changedirTime += Time.deltaTime;

            if(!_isdetect) // 플레이어 탐색 상태
            {
                if(!_isIdle) // 무브상태일때(아이들이 아닐 경우)
                    transform.Translate(_direction * moveSpeed * Time.deltaTime);
                if (_changedirTime > _patrolTime)
                    ChangeMotion();
                if(!_isJump)
                    DetectGround();
                if(_isattack)
                {
                    _changeatkTime += Time.deltaTime;
                    if (_changeatkTime > 3.0f)
                        _isattack = false;
                }

                _rayPlayer = Physics2D.Raycast(transform.position + (Vector3.down * 1f), _direction, 1.0f, 1 << LayerMask.NameToLayer("Ground"));
                if (_rayPlayer.collider != null) // 앞에 벽이 있으면
                    ChangeDir(); // 방향전환
                _rayPlayer = Physics2D.Raycast(transform.position + (Vector3.down * 0.5f), _direction, 1.0f, 1 << LayerMask.NameToLayer("Ground"));
                if (_rayPlayer.collider != null) // 앞에 벽이 있으면
                    ChangeDir(); // 방향전환

                if (Player.instance != null)
                {
                    if (!_isattack) // 플레이어가 공격이 가능한 상태여야 공격할수있음!
                    {
                        if (Vector2.Distance(Player.instance.transform.position, transform.position) < rayLength)
                        {
                            _rayPlayer = Physics2D.Raycast(transform.position, _direction, rayLength, 1 << LayerMask.NameToLayer("Player"));

                            if (_rayPlayer.collider != null)
                            {
                                _isdetect = true;
                                _animator.SetBool("run", _isdetect);
                                _changedirTime = 0;
                                SetPatrolTime();
                            }
                        }
                    }
                }
            }
            else // 플레이어를 감지할 때
            {
                transform.Translate(_direction * runSpeed * Time.deltaTime);

                if(_changedirTime > _patrolTime)
                {
                    _isdetect = false;
                    _changedirTime = 0;
                    SetPatrolTime();
                }
                _rayPlayer = Physics2D.Raycast(transform.position + (Vector3.down * 0.5f), _direction, 4.0f, 1 << LayerMask.NameToLayer("Ground"));
                if (_rayPlayer.collider == null) // 앞이 안 막혀있고
                {
                    _rayPlayer = Physics2D.Raycast(transform.position + (Vector3)_direction + (Vector3.down * 1.5f), Vector2.down, 2f, 1 << LayerMask.NameToLayer("Ground"));
                    if(_rayPlayer.collider == null) // 진행방향에 브릿지가 없으면
                    {
                        // 점프한다
                        if(!_isJump)
                            _rigidbody2d.velocity = new Vector2(0, jumpforce);
                    }
                }

                _rayPlayer = Physics2D.Raycast(transform.position + (Vector3.down * 1f), _direction, 1.0f, 1 << LayerMask.NameToLayer("Ground"));
                if(_rayPlayer.collider != null) // 앞에 벽이 있으면
                    ChangeDir(); // 방향전환
                _rayPlayer = Physics2D.Raycast(transform.position + (Vector3.down * 0.5f), _direction, 1.0f, 1 << LayerMask.NameToLayer("Ground"));
                if (_rayPlayer.collider != null) // 앞에 벽이 있으면
                    ChangeDir(); // 방향전환

                if (Vector2.Distance(Player.instance.transform.position, transform.position) < 1.5f) // 플레이어가 특정범위 내로 들어왔을경우!
                {
                    Player.instance.Attacked(damage);
                    _animator.SetTrigger("attack");
                    _isdetect = false; // 플레이어 감지 해제
                    _isattack = true; // 고블린이 공격모션을 취함. 이 이후로 n초동안 공격 및 감지 불가능.
                    _changeatkTime = 0;
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
        }
        if (!_isDetectStart)
        {
            if (collision.CompareTag("ground"))
                _isDetectStart = true;
        }
    }

    private void DetectGround()
    {
        _rayPlayer = Physics2D.Raycast(transform.position + (Vector3)_direction + (Vector3.down * 1.5f), Vector2.down, 2f, 1 << LayerMask.NameToLayer("Ground"));
        
        if(_rayPlayer.collider == null) // 시선 앞에 땅이 없으면
        {
            _randAction = Random.Range(0, 3);
            if (_randAction == 0) // 방향변경
            {
                ChangeDir();
            }
            else if (_randAction == 1) // 점프
            {
                _isJump = true;
                _rigidbody2d.velocity = new Vector2(0, jumpforce);
            }
            else { } 
        }
    }

    private void ChangeMotion()
    {
        _randAction = Random.Range(0, 3);
        if (_randAction == 0) // 방향 변경
        {
            Debug.Log("change");
            ChangeDir();
            _isIdle = false;
        }
        else if(_randAction == 1) // 정지
        {
            Debug.Log("Idle");
            _isIdle = true;
        }
        else // 그대로 진행
            _isIdle = false;
        SetPatrolTime();
        _changedirTime = 0;
    }

    private void ChangeDir()
    {
        _direction.x *= -1;
        _animator.SetFloat("direction", _direction.x);
    }

    private void SetPatrolTime()
    {
        _patrolTime = Random.Range(minpatrolTime, maxpatrolTime);
    }

    public void Dead() // Animation Func
    {
        _isDetectStart = true;
    }

    public void Delete() // Animation Func
    {
        gameObject.SetActive(false);
    }
}
