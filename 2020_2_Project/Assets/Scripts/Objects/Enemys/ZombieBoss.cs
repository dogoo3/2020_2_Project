using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieBoss : MonoBehaviour
{
    private Rigidbody2D _rigidbody2d;
    private Animator _animator;
    private Vector2 _direction;

    private EnemyWeapon _weapon;

    private RaycastHit2D _rayPlayer;
    private bool _isdead;

    [Header("플레이어에게 입힐 데미지")]
    [SerializeField] private int damage = default;

    [Header("좀비의 기본 이동속도 및 플레이어 감지시 이동속도")]
    [SerializeField] private float moveSpeed = default;

    [Header("패트롤 시간")]
    [SerializeField] private float _patrolTime = default;

    [Header("스폰할 좀비들")]
    [SerializeField] private Enemy[] zombies = default;

    [Header("부하 좀비 생성 간격")]
    [SerializeField] private float _zombieActivetime = default;

    private float _elapsedTime;

    private void Awake()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _weapon = GetComponentInChildren<EnemyWeapon>();
        _weapon.InputDamage(damage);
    }

    private void OnEnable()
    {
        _isdead = false;
        _direction = Vector2.right;
        _animator.SetFloat("direction", _direction.x);
        InvokeRepeating("ActiveMonster", 5.0f, _zombieActivetime);
    }

    private void Update()
    {
        if(!_isdead)
        {
            _elapsedTime += Time.deltaTime;
            transform.Translate(_direction * moveSpeed * Time.deltaTime);
            if(_elapsedTime > _patrolTime)
            {
                _direction.x *= -1;
                _animator.SetFloat("direction", _direction.x);
                _elapsedTime = 0;
            }

            if(Player.instance != null)
            {
                if (!Player.instance.CheckAttacked())
                {
                    if (Vector2.Distance(Player.instance.transform.position, transform.position) < 2.6f)
                    {
                        _rayPlayer = Physics2D.Raycast(transform.position + (Vector3.down * 1.5f), _direction, 2.6f, 1 << LayerMask.NameToLayer("Player")); // 공격 Ray 발사

                        if (_rayPlayer.collider != null)
                        {
                            _animator.SetTrigger("attack");
                            _isdead = true;
                        }
                    }
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("bullet"))
        {
            _animator.SetTrigger("attacked");
        }
    }

    private void ActiveMonster()
    {
        for(int i=0;i<zombies.Length;i++)
        {
            if(!zombies[i].gameObject.activeSelf) // 좀비 오브젝트가 비활성화되어있는경우
            {
                SpawnMonstersManager.instance.SpawnMonster(zombies[i]);
                break;
            }
        }
    }

    public void Dead()
    {
        _isdead = true;
        CancelInvoke("ActiveMonster");
    }

    public void Move()
    {
        _isdead = false;
    }

    public void DestroyObject()
    {
        gameObject.SetActive(false);
    }
}
