using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : MonoBehaviour
{
    private Rigidbody2D _rigidbody2d;
    private Animator _animator;

    private RaycastHit2D _rayPlayer;
    private Vector2 _direction;

    [Header("플레이어에게 입힐 데미지")]
    [SerializeField] private int damage = default;

    [Header("기본 이동속도")]
    [SerializeField] private float moveSpeed = default;

    [Header("patrolTime 초마다 진행 방향을 바꿈.")]
    [SerializeField] private float patrolTime = default;

    [Header("rayLength 안의 플레이어를 감지함.")]
    [SerializeField] private float rayLength = default;

    private void Awake()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        // Initialize
    }
}
