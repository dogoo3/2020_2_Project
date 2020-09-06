using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D _rigidbody2d;
    private Animator _animator;

    private Vector2 _movePos;
    private float _speed = 4;
    
    private void Awake()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    public void Move(int _direction)
    {
        _animator.SetInteger("direction", _direction);
        _animator.SetBool("move", true);
        _movePos.x = _direction;
    }

    public void Idle()
    {
        _animator.SetBool("move", false);
        _movePos = Vector2.zero;
    }

    public void Jump()
    {
        _animator.SetTrigger("jump");
        _rigidbody2d.velocity = new Vector2(0, 10);
    }

    private void Update()
    {
        _rigidbody2d.transform.Translate(_movePos.normalized * Time.deltaTime * _speed);
    }
}
