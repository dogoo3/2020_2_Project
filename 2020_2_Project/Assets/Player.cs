using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D _rigidbody2d;
    private Animator _animator;

    private Vector2 _movePos, _directionPos, _jumpvalue;
    private Vector2 _oldDirectionPos; // 위 보는 키 누를 때 이전 시점을 저장하는 변수
    private bool _isjump, _isshield;

    private float _hp, _shield, _speed, _def, _jump;
    private float _maxhp, _maxshield;

    public GameObject shieldsprite;
    
    private void Awake()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();

        // 게임 시작 시 기본 시선 설정
        _directionPos = Vector2.right;

        // 기본 능력치 설정
        _hp = 100;
        _shield = 100;
        _speed = 4;
        _def = 10;
        _jump = 10;
        _jumpvalue.y = _jump;

        _maxhp = _hp;
        _maxshield = _shield;
    }

    private void Start()
    {
        GaugeManager.instance.InitMaxValue(_hp, _shield);
    }

    public void Move(int _direction)
    {
        _animator.SetInteger("direction", _direction);
        _animator.SetBool("move", true);
        _movePos.x = _direction;
        _directionPos.x = _direction;
    }

    public void Idle()
    {
        _animator.SetBool("move", false);
        _movePos = Vector2.zero;
    }

    public void LookUp(bool _isLookup)
    {
        if(_isLookup) // 위를 바라볼 때(키를 누르고 있을 때)
        {
            _oldDirectionPos = _directionPos;
            _directionPos = Vector2.up;
        }
        else // 위 그만 바라볼 때(키에서 손을 뗄 때)
        {
            _directionPos = _oldDirectionPos;
        }
    }

    public void Jump()
    {
        if (!_isjump)
        {
            _animator.SetTrigger("jump");
            _rigidbody2d.velocity = _jumpvalue;
            _isjump = true;
        }
    }

    public void Shield()
    {
        if (_shield > 0)
        {
            _isshield = true;
            _def *= 2;
            shieldsprite.SetActive(true);
        }
    }

    public void UnShield()
    {
        if (_isshield)
        {
            _isshield = false;
            _def *= 0.5f;
            shieldsprite.SetActive(false);
        }
    }

    public void Shoot()
    {
        WeaponManager.instance.Shoot(transform.position, _directionPos);
    }

    public void ResetGauge()
    {
        _hp = _maxhp;
        _shield = _maxshield;
        GaugeManager.instance.ResetGauge();
    }

    public void Attacked(float _damage)
    {
        _hp -= _damage;
        GaugeManager.instance.SetHpGauge(_hp);
    }

    private void CheckDead()
    {
        if(_hp <= 0)
        {
            // 사망 모션 및 사망시 UI 구현해야 함.
        }
    }

    private void Update()
    {
        _rigidbody2d.transform.Translate(_movePos.normalized * Time.deltaTime * _speed);
        if(_isshield) // 실드 키를 누르고 있을 때.
        {
            GaugeManager.instance.SetShieldGauge(_shield -= 0.16f);
            if(_shield < 0 )
                UnShield();
        }
        else // 실드 키를 안 누르고 있을 때.
        {
            if(_shield < 100.0f)
                GaugeManager.instance.SetShieldGauge(_shield = Mathf.Clamp(_shield + 0.08f, 0, 100.0f));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("ground"))
            _isjump = false;
    }
}
