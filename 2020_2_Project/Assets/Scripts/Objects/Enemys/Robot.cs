using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour
{
    private Rigidbody2D _rigidbody2d;
    private Animator _animator;
    private Enemy _enemy;

    [SerializeField] private float _moveSpeed = default;

    private void Awake()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _enemy = GetComponent<Enemy>();
    }

    private void OnEnable()
    {

    }

    private void Update()
    {
        transform.Translate(_enemy._direction * _moveSpeed * Time.deltaTime);
    }


    public void Dead() // Animator Func
    {
        gameObject.SetActive(false);
    }
}
