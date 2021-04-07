using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : MonoBehaviour
{
    private Rigidbody2D _rigidbody2d;
    private Animator _animator;
    private Enemy _enemy;

    private RaycastHit2D _rayPlayer;
    private Transform _playerPos;
    
    private bool _isDetectStart; // 땅을 밟아 플레이어 감지를 시작함
    private bool _isIdle; // 가만히 서 있을 때
    private float _changeatkedTime, _elapsedDetectTime; // 피격 후 경과 시간

    [Header("플레이어에게 입힐 데미지")]
    [SerializeField] private int damage = default;

    [Header("기본 이동속도, 플레이어 감지 이동속도")]
    [SerializeField] private float moveSpeed = default;
    [SerializeField] private float runSpeed = default;
    
    [Header("rayLength 안의 플레이어를 감지함.")]
    [SerializeField] private float rayLength = default;

    [Header("피격 후 방어력 증가 시간")]
    [SerializeField] private float _attackedTime = default;

    [Header("감지 시간 & 감지 해제후 쿨타임")]
    [SerializeField] private float _detectTime = default;
    [SerializeField] private float _detectCoolTime = default;

    private void Awake()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _enemy = GetComponent<Enemy>();
    }

    private void Start()
    {
        _playerPos = Player.instance.transform;
    }

    private void OnEnable()
    {
        // Initialize
        _enemy._direction = Vector2.right;
        _animator.SetFloat("direction", _enemy._direction.x);
        _isDetectStart = false;
        _enemy.isattack = false;
        _enemy.isjump = false;
        _enemy.isdetect = false;
        _elapsedDetectTime = 0;
    }

    private void Update()
    {
        if (_isDetectStart)
        {
            // 플레이어 감지 여부에 따라 이동속도 구분 및 행동 전환
            if (_enemy.isdetect)
            {
                transform.Translate(_enemy._direction * runSpeed * Time.deltaTime);
                // 점프중엔 쿨타임 연산을 하지 않는다.
                if (!_enemy.isjump)
                {
                    _elapsedDetectTime += Time.deltaTime;
                    _rayPlayer = Physics2D.Raycast(transform.position + Vector3.down, _enemy._direction, 1.0f, 1 << LayerMask.NameToLayer("Player"));
                    if(_rayPlayer.collider != null)
                    {
                        if (Player.instance.Attacked(damage)) // 플레이어가 무적상태이면 공격모션 자체를 실행하지 않음.
                        {
                            _enemy.isattack = true;
                            _animator.SetTrigger("attack");
                        }

                        _elapsedDetectTime = 0;
                        _enemy.isdetect = false;
                        _animator.SetFloat("run", 1.0f);
                    }
                    if (_elapsedDetectTime > _detectTime)
                    {
                        _elapsedDetectTime = 0;
                        _enemy.isdetect = false;
                        _animator.SetFloat("run", 1.0f);
                    }
                }
            }
            else
            {
                if (!_enemy.isattack) // 공격중이 아닐때는 이동
                    transform.Translate(_enemy._direction * moveSpeed * Time.deltaTime);

                // 감지 쿨타임 연산
                if (_elapsedDetectTime <= _detectCoolTime)
                    _elapsedDetectTime += Time.deltaTime;
                else
                {
                    // rayLength 안에 있을 때만 Raycast 발사
                    if (Vector2.Distance(transform.position, _playerPos.position) <= rayLength)
                    {
                        _rayPlayer = Physics2D.Raycast(transform.position + Vector3.down, _enemy._direction, rayLength, 1 << LayerMask.NameToLayer("Player"));
                        if (_rayPlayer.collider != null)
                        {
                            _enemy.isdetect = true;
                            _elapsedDetectTime = 0f;
                            _animator.SetFloat("run", 1.5f);
                        }
                    }
                }
            }

            // 피격시 방어력 증가 시간 연산을 진행
            if(_enemy.isattacked)
            {
                _changeatkedTime += Time.deltaTime;
                if (_changeatkedTime > _attackedTime)
                {
                    _enemy.isattacked = false;
                    _enemy.SetDef(false);
                    _animator.SetBool("bool_attacked", false);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("bullet")) // 총알에 맞았을 때
        {
            _enemy.SetDef(true); // 총알에 맞으면 N초동안 방어력 증가
            _changeatkedTime = 0;
            _animator.SetBool("bool_attacked", true);
        }
        if (!_isDetectStart) // 생성 후 첫 감지
        {
            if (collision.CompareTag("ground"))
                _isDetectStart = true;
        }
    }
    public void ActiveDeteteStart() // Animation Func, 모든 행동을 정지할 때 사용.
    {
        _isDetectStart = false;
    }

    public void AttackChange() // Animation Func
    {
        _enemy.isattack = false;
    }

    public void Delete() // Animation Func
    {
        gameObject.SetActive(false);
    }
}
