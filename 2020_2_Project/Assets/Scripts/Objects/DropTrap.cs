using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropTrap : MonoBehaviour
{
    private Rigidbody2D _rigidbody2d;

    private Vector2 _oripos, _uppos;

    private bool _islandGround, _isReset, _isTouchPlayer, _isUp;
    private float _elapsetime, _gravityScale;

    private float _displacepos; // 얼마나 위치값이 바뀌었는지를 파악함.

    [Header("지면 충돌 후 N초뒤에 복귀")]
    [SerializeField] private float _resetTime = default;
    [Header("원래 포인트 도착 후 N초 뒤에 다시 떨굼")]
    [SerializeField] private float _dropTime = default;
    [Header("데미지")]
    [SerializeField] private float _damage = default;

    private void Awake()
    {
        _rigidbody2d = GetComponentInParent<Rigidbody2D>();
    }

    private void Start()
    {
        _oripos = transform.parent.position;
        _gravityScale = _rigidbody2d.gravityScale;
    }

    private void Update()
    {
        if (_islandGround) // 떨어지고 난 뒤 실행되는 부분
        {
            _elapsetime += Time.deltaTime;
            if (_elapsetime > _resetTime)
            {
                _elapsetime = 0;
                _isReset = true;
                _islandGround = false;
                _isUp = true;
            }
        }

        if (_isReset) // 원래 위치 복귀 후에 실행되는 부분
        {
            _uppos = Vector2.Lerp(_uppos, _oripos, CalcTime());
            transform.parent.position = _uppos;
            if ((Vector2)transform.parent.position == _oripos)
            {
                _elapsetime = 0;
                _isReset = false;
                Invoke("DropObject", _dropTime);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("ground"))
        {
            _islandGround = true;
            _uppos = transform.parent.position;
            _rigidbody2d.gravityScale = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            Player.instance.Squash(true, _damage);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (_isUp)
            {
                Player.instance.Squash(false);
                _isUp = false;
            }
        }
    }

    private float CalcTime() // 일정 시간이 지나면 1을 반환하여 오브젝트를 떨어뜨린다.
    {
        _elapsetime += Time.deltaTime;
        if (_elapsetime < 2.0f)
            return (_elapsetime * _elapsetime + _elapsetime) * 0.5f;
        else
            return 1.0f;
    }

    private void DropObject()
    {
        _rigidbody2d.gravityScale = _gravityScale;
        _isUp = false;
    }
}
