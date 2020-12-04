using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Sniper : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    private Bullet _sniper;

    private Color _color;
    private bool _isCrash;

    [SerializeField] private Sprite[] _bulletSprites = default;

    public float damage;
    public float shotSpeed;
    public float surviveTime;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _sniper = new B_Sniper();
        _sniper.damage = FileManager.weaponInfo["sniper_deal"];
        _sniper.shotSpeed = shotSpeed;
        _sniper.surviveTime = surviveTime;
        _color = Color.white;
        _bulletSprites[2] = _spriteRenderer.sprite;
    }

    private void Update()
    {
        if (_sniper.CheckElapsedTime())
            ObjectPoolingManager.instance.InsertQueue(this, ObjectPoolingManager.instance.queue_sniper);
        if (_isCrash)
        {
            _color.a = Mathf.Clamp(_color.a - 0.031372f, 0f, 1f);  // 0.5초에 사라지게 함.
            _spriteRenderer.color = _color;
            if (_color.a <= 0f)
                ObjectPoolingManager.instance.InsertQueue(this, ObjectPoolingManager.instance.queue_sniper);
        }
        else
            transform.Translate(_sniper.Move());
    }

    private void OnDisable()
    {
        _color.a = 1.0f;
        _spriteRenderer.color = _color;
        _spriteRenderer.sprite = _bulletSprites[2];
        _isCrash = false;
        _sniper.ResetElapsedTime();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "ground":
            case "bullet":
            case "wall":
                _isCrash = true;
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
                _sniper.tempEnemy = collision.gameObject.GetComponent<Enemy>();
                _sniper.tempEnemy.MinusHP(_sniper.damage);
                // if(!_sniper.tempEnemy.CheckBoss())
                // _sniper.tempEnemy.Knockback(_sniper._direction);
                _isCrash = true;
                _spriteRenderer.sprite = _bulletSprites[Random.Range(0, 2)];
                SoundManager.instance.PlaySFX("bulletToenemy");
                break;
        }
    }

    public void Direction(Vector2 _direction)
    {
        _sniper.Direction(_direction);
    }
}
