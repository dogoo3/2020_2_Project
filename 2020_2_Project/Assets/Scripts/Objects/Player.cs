using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;

    private Rigidbody2D _rigidbody2d;
    [SerializeField] private Collider2D _collider2d = default;
    private Collider2D _detectRope;

    private Animator _animator;
    private SpriteRenderer _spriterenderer;
    
    private Vector2 _movePos, _directionPos, _jumpvalue;
    private Vector2 _oldDirectionPos; // 위 보는 키 누를 때 이전 시점을 저장하는 변수
    private Vector2 _catchRopeHeight;
    private Vector2 _upShootVector = new Vector2(0.40159f, 0.91581f);
    private Rope _rope;

    private bool _isjump, _isshield, _isdead, _isattacked, _isLookup, _isRope, _isBlink;
    private bool _isRopeUp, _isRopeDown;

    private float _hp, _shield, _speed, _def;
    private float _maxhp, _maxshield;
    private float _invincibleTime = 2.0f, _lookupTime;

    [SerializeField] private GameObject shieldsprite = default;
    [SerializeField] private Transform muzzleGunPos = default;

    private RaycastHit2D _rayPlayer;

    private void Awake()
    {
        instance = this;
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriterenderer = GetComponent<SpriteRenderer>();

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
        if (_isLookup) // 위를 바라볼 때(키를 누르고 있을 때)
        {
            if(!_isRope) // 로프를 타고 있는 상태가 아니면
            {
                _detectRope = Physics2D.OverlapBox(transform.position, Vector2.one * 0.5f, 0, 1 << LayerMask.NameToLayer("Rope")); // 로프 탐색
                if(_detectRope == null) // 로프가 없으면 시점 위로
                {
                    if (WeaponManager.instance.IsHaveWeapon())
                    {
                        this._isLookup = true;
                        _animator.SetBool("Lookup", this._isLookup);
                        _oldDirectionPos = _directionPos; // 위를 바라보기 직전에 어느 시점을 보고 있었는지를 저장
                        _directionPos = _upShootVector; // 위로 쏠 때 벡터 정규화값 저장
                        _directionPos.x *= _oldDirectionPos.x; // 방향 변경
                        _animator.SetFloat("float_Lookup", 1.0f); // Blend Tree 내 분기를 위한 Lookup 파라미터
                        _animator.Play("Lookup", 0, _lookupTime);
                        
                    }
                }
                else // 로프가 감지되면
                {
                    DetectRope();
                    _isRopeUp = true;
                }
            }
            else // 로프를 타고 있는 상태면
                _isRopeUp = true;
        }
        else // 위 그만 바라볼 때(키에서 손을 뗄 때)
        {
            if(!_isRope)
            {
                if (WeaponManager.instance.IsHaveWeapon())
                {
                    this._isLookup = false;
                    _animator.SetBool("Lookup", this._isLookup);
                    _animator.SetFloat("float_Lookup", 0f);
                    _animator.Play("LookupReverse", 0, 1 - _lookupTime);
                    _directionPos = _oldDirectionPos;
                }
            }
            else
                _isRopeUp = false;
        }
    }

    public void Jump()
    {
        if(!_isdead)
        {
            if(!_isRope) // 로프를 타고 있지 않을 때에는 일반 점프동작을 수행
            {
                _rayPlayer = Physics2D.Raycast(transform.position + (Vector3.down * 1.3f), Vector2.down, 0.5f, 1 << LayerMask.NameToLayer("Ground"));
                if (_rayPlayer.collider != null)
                {
                    if (!_isjump)
                        JumpSet(_jumpvalue);
                }
            }
            else // 로프를 타고 있을 때 다른 방향키를 누르고 있으면 로프를 탈출하면서 점프함.
            {
                if(_movePos != Vector2.zero)
                {
                    JumpSet(_jumpvalue * 0.5f);
                    EscapeRope();
                }
            }
        }
    }

    private void JumpSet(Vector2 _jumpforce)
    {
        _animator.SetBool("jump", true);
        _rigidbody2d.velocity += _jumpforce;
        _isjump = true;
    }

    public void Shield()
    {
        if(!_isdead)
        {
            if(!_isRope)
            {
                _detectRope = Physics2D.OverlapBox(transform.position + Vector3.down*1.25f, Vector2.one * 0.5f, 0, 1 << LayerMask.NameToLayer("Rope")); // 로프 탐색
                if (_detectRope == null) // 로프가 없으면 실드 사용
                {
                    if (_shield > 30.0f)
                    {
                        _isshield = true;
                        shieldsprite.SetActive(true);
                    }
                }
                else // 로프 감지 시
                {
                    DetectRope();
                    _isRopeDown = true;
                }
            }
            else
                _isRopeDown = true;
        }
    }

    public void UnShield()
    {
        if(!_isdead)
        {
            if (!_isRope)
            {
                if (_isshield)
                {
                    _isshield = false;
                    shieldsprite.SetActive(false);
                }
            }
            else
                _isRopeDown = false;
        }
    }

    public void Shoot() // 버튼을 누르면 작동하는 함수
    {
        if (!_isdead)
        {
            if (_lookupTime == 1.0f || _lookupTime == 0) // 시점을 변경하는 동안(위를 보기 위해 고개를 드는 동안)에는 공격할 수 없다.
            {
                // 탄환 유무 체크 -> 그 탄환의 쿨타임 체크
                if (WeaponManager.instance.IsShootWeapon())
                    _animator.SetTrigger("shoot");
            }
        }
    }

    public void ShootMotion() // 발사 버튼을 누르고 난 뒤 애니메이션 프레임에서 작동하는 함수
    {
        WeaponManager.instance.Shoot(muzzleGunPos.position, _directionPos);
    }

    public void CheckShootAfterLookup()
    {
        if (_isLookup)
            _animator.Play("Lookup", 0, 1f);
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
        if(_isshield)
        {
            if (_shield >= 50.0f)
                _shield -= 50.0f;
            else
                _shield = 0f;

            SoundManager.instance.PlaySFX("shielddef");
            _isattacked = true;
            Invoke("DEFShield", 1.0f);
            return;
        }
        _hp -= _damage;
        GaugeManager.instance.SetHpGauge(_hp);
        _isattacked = true;
        _animator.SetTrigger("attacked");
        SoundManager.instance.PlaySFX("playerattacked");
        if (!_isBlink) // 처음 피격받는다면 깜빡임을 실행해준다.
        {
            Blink();
            Invoke("CancelBlink", _invincibleTime);
        }
        CheckDead();
    }

    private void DEFShield()
    {
        _isattacked = false;
    }

    public bool CheckAttacked()
    {
        return _isattacked;
    }
    
    public bool GetisDead() // 클리어를 띄울 때 플레이어 사망 유무를 반환하는 함수.
    {
        return _isdead;
    }

    public void KnockBack(int _damage, Vector2 _knockback)
    {
        Attacked(_damage);
        _rigidbody2d.velocity = _knockback;
    }

    public void Fly(Vector2 _force)
    {
        _isjump = true;
        _animator.SetBool("jump", _isjump);
        _rigidbody2d.velocity = _force;
    }

    public void SetWeaponAnimation(WeaponName _weaponNum)
    {
        _animator.SetFloat("weaponNum", (float)_weaponNum);
    }

    #region AttackBlinkFunc
    private void Blink()
    {
        _isBlink = true;
        InvokeRepeating("BlinkAtt", 0f, 0.2f);
        InvokeRepeating("BlinkOri", 0.1f, 0.2f);
    }

    public void CancelBlink() // Animation Func
    {
        _isBlink = false;
        _isattacked = false;
        CancelInvoke("BlinkAtt");
        CancelInvoke("BlinkOri");
        BlinkOri();
    }

    private void BlinkAtt()
    {
        _spriterenderer.color = Color.gray;
    }

    private void BlinkOri()
    {
        _spriterenderer.color = Color.white;
    }
    #endregion
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
    
    private void DetectRope()
    {
        _rope = _detectRope.GetComponent<Rope>();
        _rigidbody2d.gravityScale = 0;
        _collider2d.isTrigger = true;
        _catchRopeHeight = transform.position;
        _catchRopeHeight.x = _detectRope.transform.position.x;
        transform.position = _catchRopeHeight;
        _isRope = true;
    }

    private void EscapeRope()
    {
        _animator.SetBool("jump", false);
        _isjump = false;
        _isRope = false;
        _rigidbody2d.gravityScale = 3.3f;
        _collider2d.isTrigger = false;
    }

    private void CheckDead()
    {
        if(_hp <= 0 && !_isdead)
        {
            _isdead = true;
            CancelBlink();
            SoundManager.instance.PlaySFX("playerdead");
            WindowManager.instance.Invoke("ShowFailWindow",3.0f); // 3초 뒤에 실패 윈도우를 띄운다.
            _animator.SetTrigger("dead");
            _movePos = Vector2.zero;
        }
    }

    private void Update()
    {
        if (!_isdead) // dead가 true이면 플레이어가 죽었다는 의미.
        {
            if(!_isRope) // 로프를 타고 있을때는 키를 눌러도 이동하지 않음.
                transform.Translate(_movePos.normalized * Time.deltaTime * _speed);
            if (_isshield) // 실드 키를 누르고 있을 때.
            {
                GaugeManager.instance.SetShieldGauge(_shield -= 0.16f);
                if (_shield <= 0)
                    UnShield();
            }
            else // 실드 키를 안 누르고 있을 때.
            {
                if (_shield < 100.0f)
                    GaugeManager.instance.SetShieldGauge(_shield = Mathf.Clamp(_shield + 0.02f, 0, 100.0f));
            }

            if (_isLookup)
                _lookupTime = Mathf.Clamp(_lookupTime + 0.33333333f, 0, 1f);
            else
                _lookupTime = Mathf.Clamp(_lookupTime - 0.33333333f, 0, 1f);

            if(_isRopeDown)
            {
                transform.Translate(Vector3.down * 2.0f * Time.deltaTime);
                if (_rope.CarculateDownPos(transform))
                {
                    EscapeRope();
                    _isRopeDown = false;
                }
            }
            if(_isRopeUp)
            {
                transform.Translate(Vector3.up * 2.0f * Time.deltaTime);
                if (_rope.CarculateUpPos(transform))
                {
                    EscapeRope();
                    _isRopeUp = false;
                }
            }
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                    Move(-1);
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                    Move(1);
                else if (Input.GetKeyDown(KeyCode.UpArrow))
                    LookUp(true);
                else if (Input.GetKeyDown(KeyCode.DownArrow))
                    Shield();
                else if (Input.GetKeyDown(KeyCode.Z)) // Shoot
                    Shoot();
                else if (Input.GetKeyDown(KeyCode.Space))
                    Jump();
                else { }

                if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
                    Idle();
                else if (Input.GetKeyUp(KeyCode.UpArrow))
                    LookUp(false);
                else if (Input.GetKeyUp(KeyCode.DownArrow))
                    UnShield();
                else { }
            }
        }
    }

    public void OnConveyorBelt(Vector2 vector2)
    {
        _rigidbody2d.velocity = vector2;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            _rayPlayer = Physics2D.Raycast(transform.position, Vector3.up, 1.12f, 1 << LayerMask.NameToLayer("Ground"));
            if (_rayPlayer.collider == null)
            {
                _animator.SetBool("jump", false);
                _isjump = false;
            }
        }
    }
}
