using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Pistol : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private CircleCollider2D _collider2d;

    private Bullet _pistol;

    private Color _color;
    private bool _isCrash;

    [SerializeField] private Sprite[] _bulletSprites = default;

    public float damage;
    public float shotSpeed;
    public float surviveTime;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider2d = GetComponent<CircleCollider2D>();
        _pistol = new B_Pistol();
        _pistol.damage = FileManager.weaponInfo["pistol_deal"];
        _pistol.shotSpeed = shotSpeed;
        _pistol.surviveTime = surviveTime;
        _color = Color.white;
        _bulletSprites[2] = _spriteRenderer.sprite;
    }

    private void Update()
    {
        if (_pistol.CheckElapsedTime()) // 총알의 유효시간이 지났을 경우
            ObjectPoolingManager.instance.InsertQueue(this, ObjectPoolingManager.instance.queue_pistol);
        if(_isCrash) // 총알이 어딘가에 충돌했을 경우
        {
            _color.a = Mathf.Clamp(_color.a - 0.031372f, 0f, 1f);  // 0.5초에 사라지게 함.
            _spriteRenderer.color = _color;
            if(_color.a <= 0f)
                ObjectPoolingManager.instance.InsertQueue(this, ObjectPoolingManager.instance.queue_pistol);
        }
        else // 지속적으로 총알 이동
            transform.Translate(_pistol.Move());
    }

    private void OnDisable()
    {
        _color.a = 1.0f;
        _spriteRenderer.color = _color;
        _spriteRenderer.sprite = _bulletSprites[2];
        _isCrash = false;
        _collider2d.enabled = true;
        _pistol.ResetElapsedTime();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "ground":
            case "bullet":
            case "wall":
                _isCrash = true;
                _collider2d.enabled = false;
                _spriteRenderer.sprite = _bulletSprites[Random.Range(0, 2)];
                SoundManager.instance.PlaySFX("bulletTowall");
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "enemy":
                _pistol.tempEnemy = collision.gameObject.GetComponent<Enemy>();
                _pistol.tempEnemy.MinusHP(_pistol.damage);
                // if (!_pistol.tempEnemy.CheckBoss())
                // _pistol.tempEnemy.Knockback(_pistol._direction);
                _isCrash = true;
                _collider2d.enabled = false;
                _spriteRenderer.sprite = _bulletSprites[Random.Range(0, 2)];
                SoundManager.instance.PlaySFX("bulletToenemy");
                break;
        }
    }

    public void Direction(Vector2 _direction)
    {
        _pistol.Direction(_direction);
    }
}
