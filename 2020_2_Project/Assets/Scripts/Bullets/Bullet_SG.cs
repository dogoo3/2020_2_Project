using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_SG : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    private Bullet _sg;

    private Color _color;
    private bool _isCrash;

    [SerializeField] private Sprite[] _bulletSprites = default;

    public float damage;
    public float shotSpeed;
    public float surviveTime;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _sg = new B_SG();
        _sg.damage = FileManager.weaponInfo["sg_deal"];
        _sg.shotSpeed = shotSpeed;
        _sg.surviveTime = surviveTime;
        _color = Color.white;
        _bulletSprites[2] = _spriteRenderer.sprite;
    }

    private void Update()
    {
        if (_sg.CheckElapsedTime())
            ObjectPoolingManager.instance.InsertQueue(this, ObjectPoolingManager.instance.queue_sg);
        if (_isCrash)
        {
            _color.a = Mathf.Clamp(_color.a - 0.031372f, 0f, 1f);  // 0.5초에 사라지게 함.
            _spriteRenderer.color = _color;
            if (_color.a <= 0f)
                ObjectPoolingManager.instance.InsertQueue(this, ObjectPoolingManager.instance.queue_sg);
        }
        else
            transform.Translate(_sg.Move());
    }

    private void OnDisable()
    {
        _color.a = 1.0f;
        _spriteRenderer.color = _color;
        _spriteRenderer.sprite = _bulletSprites[2];
        _isCrash = false;
        _sg.ResetElapsedTime();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "ground":
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
                _sg.tempEnemy = collision.gameObject.GetComponent<Enemy>();
                _sg.tempEnemy.MinusHP(_sg.damage);
                // if (!_sg.tempEnemy.CheckBoss())
                // _sg.tempEnemy.Knockback(_sg._direction);
                _isCrash = true;
                _spriteRenderer.sprite = _bulletSprites[Random.Range(0, 2)];
                SoundManager.instance.PlaySFX("bulletToenemy");
                break;
        }
    }

    public void Direction(Vector2 _direction)
    {
        _sg.Direction(_direction);
    }
}
