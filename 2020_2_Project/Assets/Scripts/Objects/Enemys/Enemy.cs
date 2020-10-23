﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D _rigidbody2d;
    private Animator _animator;

    private bool _isDef; // Goblin을 위한 방어력 증가 변수
    private bool _isDead; // 사망을 1회만 체크하도록 함.

    [Header("적 능력치 설정")]
    [SerializeField] private float HP = default;
    [SerializeField] private int score = default;
    [SerializeField] private int minGold = default;
    [SerializeField] private int maxGold = default;

    [Header("보스몬스터인가?")]
    [SerializeField] private bool _isboss = default;

    private void Awake()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _isDef = false;
    }

    private void OnEnable()
    {
        _isDead = false;
    }

    public void MinusHP(float _hp)
    {
        if (_isDef) // 방어율 적용
            HP -= (_hp * 0.5f);
        else
            HP -= _hp;
        if (HP <= 0)
        {
            if (!_isDead)
            {
                if (gameObject.activeSelf) // Translate 연산을 해서 한 프레임에 여러 발의 샷건 총알이 맞을 수 있기 때문에 한 발만 적용되도록 코드 수정.
                {
                    ScoreManager.instance.UpdateScore(score);
                    SpawnMonstersManager.instance.CatchMonster();
                    ObjectPoolingManager.instance.GetQueue_gold(transform.position, Random.Range(minGold, maxGold));
                    // 나중에 Parameter로 goldMin, goldMax도 같이 넘겨줄 것...
                    _animator.SetTrigger("dead");
                    _isDead = true;
                }
            }
        }
    }

    public void Spawn(Vector2 _direction, float _shotspeed)
    {
        gameObject.SetActive(true);
        _rigidbody2d.velocity = _direction * _shotspeed;
    }

    public void Knockback(Vector2 _direction)
    {
        _rigidbody2d.velocity = _direction * 10.0f;
    }

    public void SetDef(bool _is)
    {
        _isDef = _is;
    }

    public bool CheckBoss()
    {
        return _isboss;
    }
}
