using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_SMG : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    private Bullet _smg;

    private Color _color;
    private Sprite _bulletSprite;
    private bool _isCrash;

    [SerializeField] private Sprite _crashSprite = default;

    public float damage;
    public float shotSpeed;
    public float surviveTime;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _smg = new B_SMG();
        _smg.damage = FileManager.weaponInfo["smg_deal"];
        _smg.shotSpeed = shotSpeed;
        _smg.surviveTime = surviveTime;
        _color = Color.white;
        _bulletSprite = _spriteRenderer.sprite;
    }

    private void Update()
    {
        if (_smg.CheckElapsedTime())
            ObjectPoolingManager.instance.InsertQueue(this, ObjectPoolingManager.instance.queue_smg);
        if (_isCrash)
        {
            _spriteRenderer.sprite = _crashSprite;
            _color.a = Mathf.Clamp(_color.a - 0.031372f, 0f, 1f);  // 0.5초에 사라지게 함.
            _spriteRenderer.color = _color;
            if (_color.a <= 0f)
                ObjectPoolingManager.instance.InsertQueue(this, ObjectPoolingManager.instance.queue_smg);
        }
        else
            transform.Translate(_smg.Move());
    }

    private void OnDisable()
    {
        _color.a = 1.0f;
        _spriteRenderer.color = _color;
        _spriteRenderer.sprite = _bulletSprite;
        _isCrash = false;
        _smg.ResetElapsedTime();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "ground":
            case "bullet":
            case "wall":
                _isCrash = true;
                SoundManager.instance.PlaySFX("bulletTowall");
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "enemy":
                _smg.tempEnemy = collision.gameObject.GetComponent<Enemy>();
                _smg.tempEnemy.MinusHP(_smg.damage);
                // if (!_smg.tempEnemy.CheckBoss())
                // _smg.tempEnemy.Knockback(_smg._direction);
                _isCrash = true;
                SoundManager.instance.PlaySFX("bulletToenemy");
                break;
        }
    }

    public void Direction(Vector2 _direction)
    {
        _smg.Direction(_direction);
    }
}
