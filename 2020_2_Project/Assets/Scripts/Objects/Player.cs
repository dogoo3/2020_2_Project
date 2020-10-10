using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;

    private Rigidbody2D _rigidbody2d;
    private Animator _animator;

    private Vector2 _movePos, _directionPos, _jumpvalue;
    private Vector2 _oldDirectionPos; // 위 보는 키 누를 때 이전 시점을 저장하는 변수
    private bool _isjump, _isshield, _isdead;

    private float _hp, _shield, _speed, _def;
    private float _maxhp, _maxshield;

    [SerializeField] private GameObject shieldsprite = default;
    [SerializeField] private Transform muzzleGunPos = default;

    private void Awake()
    {
        instance = this;

        _rigidbody2d = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();

        // 게임 시작 시 기본 시선 설정
        _directionPos = Vector2.right;

        // 기본 능력치 설정
        _hp = FileManager.playerInfo["hp"];
        _shield = FileManager.playerInfo["shield"];
        _speed = FileManager.playerInfo["speed"];
        _def = FileManager.playerInfo["def"];
        _jumpvalue.y = FileManager.playerInfo["jump"];

        _maxhp = _hp;
        _maxshield = _shield;
    }

    private void Start()
    {
        GaugeManager.instance.InitMaxValue(_hp, _shield);
    }

    public void Move(int _direction)
    {
        if (_isdead)
            return;
        _animator.SetFloat("direction", _direction);
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
            _oldDirectionPos = _directionPos; // 위를 바라보기 직전에 어느 시점을 보고 있었는지를 저장
            _directionPos = Vector2.up;
        }
        else // 위 그만 바라볼 때(키에서 손을 뗄 때)
        {
            _directionPos = _oldDirectionPos;
        }
    }

    public void Jump()
    {
        if(!_isdead)
        {
            if (!_isjump)
            {
                _animator.SetBool("jump", true);
                _rigidbody2d.velocity = _jumpvalue;
                _isjump = true;
            }
        }
    }

    public void Shield()
    {
        if(!_isdead)
        {
            if (_shield > 0)
            {
                _isshield = true;
                _def *= 2;
                shieldsprite.SetActive(true);
            }
        }
    }

    public void UnShield()
    {
        if(!_isdead)
        { 
            if (_isshield)
            {
                _isshield = false;
                _def *= 0.5f;
                shieldsprite.SetActive(false);
            }
        }
    }

    public void Shoot() // 버튼을 누르면 작동하는 함수
    {
        if (!_isdead)
        {
            // 탄환 유무 체크 -> 그 탄환의 쿨타임 체크
            if (WeaponManager.instance.IsShootWeapon())
            {
                switch (WeaponManager.instance.GetSelectWeapon())
                {
                    case WeaponName.grenade:
                        _animator.SetTrigger("throw");
                        break;
                    default:
                        _animator.SetTrigger("shoot");
                        break;
                }
            }
        }
    }

    public void ShootMotion() // 발사 버튼을 누르고 난 뒤 애니메이션 프레임에서 작동하는 함수
    {
        WeaponManager.instance.Shoot(muzzleGunPos.position, _directionPos);
    }

    public void ResetSetting()
    {
        _hp = _maxhp;
        _shield = _maxshield;
        _isdead = false;
        _animator.Rebind();
        GaugeManager.instance.ResetGauge();
    }

    public void Attacked(float _damage)
    {
        _hp -= _damage;
        GaugeManager.instance.SetHpGauge(_hp);
        _animator.SetTrigger("attacked");
        CheckDead();
    }

    public void HealHP(float _healValue)
    {
        _hp = Mathf.Clamp(_hp + _healValue, 0, _maxhp);
        GaugeManager.instance.SetHpGauge(_hp);
    }

    public void HealShield(float _healValue)
    {
        _shield = Mathf.Clamp(_shield + _healValue, 0, _maxshield);
        GaugeManager.instance.SetShieldGauge(_shield);
    }

    private void CheckDead()
    {
        if(_hp <= 0)
        {
            _isdead = true;
            WindowManager.instance.Invoke("ShowFailWindow",3.0f); // 3초 뒤에 실패 윈도우를 띄운다.
            _animator.SetTrigger("dead");
        }
    }

    private void Update()
    {
        if(!_isdead) // dead가 true이면 플레이어가 죽었다는 의미.
        {
            _rigidbody2d.transform.Translate(_movePos.normalized * Time.deltaTime * _speed);
            if (_isshield) // 실드 키를 누르고 있을 때.
            {
                GaugeManager.instance.SetShieldGauge(_shield -= 0.16f);
                if (_shield < 0)
                    UnShield();
            }
            else // 실드 키를 안 누르고 있을 때.
            {
                if (_shield < 100.0f)
                    GaugeManager.instance.SetShieldGauge(_shield = Mathf.Clamp(_shield + 0.08f, 0, 100.0f));
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            _animator.SetBool("jump", false);
            _isjump = false;
        }
    }
}
