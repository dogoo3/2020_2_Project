using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    private Animator _animator;

    private RaycastHit2D _rayPlayer, _rayGround;
    private Vector3 _vectordir;

    private bool _isdetect; // 플레이어를 감지했을 때
    private bool _isSuicide; // 플레이어와 붙어 자살할 때

    private float _changedirTime; // 시점 변환 시간을 계산하는 변수

    [Header("플레이어에게 입힐 데미지")]
    [SerializeField] private int damage = default;

    [Header("좀비의 기본 이동속도 및 플레이어 감지시 이동속도")]
    [SerializeField] private float moveSpeed = default;
    [SerializeField] private float detectPlayerSpeed = default;

    [Header("patrolTime 초마다 진행 방향을 바꿈.")]
    [SerializeField] private float patrolTime = default;

    [Header("detectTime 초 안에 플레이어를 감지하지 못하면 다시 원래 이동속도로 돌아간다.")]
    [SerializeField] private float detectTime = default;

    [Header("rayLength 안의 플레이어를 감지함.")]
    [SerializeField] private float rayLength = default;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _vectordir = Vector2.right;
        _animator.SetFloat("direction", _vectordir.x);
    }

    private void Update()
    {
        if (!_isSuicide) // 플레이어에게 붙어 자살할 때는 움직이게 하지 않는다.
        {
            _changedirTime += Time.deltaTime;

            if (!_isdetect) // 플레이어를 감지하는 상태이다.
            {
                transform.Translate(_vectordir * moveSpeed * Time.deltaTime);
                if (_changedirTime > patrolTime) // 일정 시간이 지나면 시점을 바꿔준다.
                {
                    _vectordir.x *= -1;
                    _animator.SetFloat("direction", _vectordir.x);
                    _changedirTime = 0;
                }

                if (Vector2.Distance(Player.instance.transform.position, transform.position) < rayLength) // 매 프레임마다 Raycast는 Performance를 낭비할 수 있으므로, 일정 거리 안으로 들어오면 Ray를 쏜다.
                {
                    // Player를 RayCast로 찾는다.
                    _rayPlayer = Physics2D.Raycast(transform.position, _vectordir, rayLength, 1 << LayerMask.NameToLayer("Player"));

                    if (_rayPlayer.collider != null) // 플레이어가 감지되면 인페테란처럼 빠르게 플레이어가 있는 방향으로 달려간다.
                    {
                        _isdetect = true;
                        _animator.SetBool("Run", _isdetect);
                        _changedirTime = 0;
                    }
                }
            }
            else // 플레이어를 감지해서 플레이어의 방향으로 빠르게 달려가는 상태이다.
            {
                transform.Translate(_vectordir * detectPlayerSpeed * Time.deltaTime);
                if (_changedirTime > detectTime) // 플레이어를 만나 자살하지 못할 경우
                {
                    _isdetect = false; // 다시 탐색 상태로 돌아간다.
                    _animator.SetBool("Run", _isdetect);
                    _vectordir.x *= -1; // 방향 전환
                    _animator.SetFloat("direction", _vectordir.x);
                    _changedirTime = 0; 
                }
            }
        }
        DetectGround();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            _animator.SetTrigger("suicide");
            _isSuicide = true;
        }
    }

    public void Delete() // Animation Func
    {
        gameObject.SetActive(false);
    }

    public void StopObject() // Animation Func
    {
        _isSuicide = true;
    }

    public void Attacked() // Animation Func, 폭발 프레임 쯤에 심어놓을 예정.
    {
        SpawnMonstersManager.instance.CatchMonster();
        Player.instance.Attacked(damage);
    }

    private void DetectGround() // 좀비가 일정 땅 안에서만 이동할 수 있도록 
    {
        _rayPlayer = Physics2D.Raycast(transform.position + _vectordir, Vector2.down, 1.5f, 1 << LayerMask.NameToLayer("Ground"));

        if(_rayPlayer.collider == null) // 시선 앞에 땅이 없으면
        {
            _vectordir.x *= -1; // 시점변경
            _animator.SetFloat("direction", _vectordir.x);
            _changedirTime = 0;
        }
    }
}
