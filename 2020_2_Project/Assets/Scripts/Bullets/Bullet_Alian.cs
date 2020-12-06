using UnityEngine;
using System.Collections;

public class Bullet_Alian : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private CircleCollider2D _collider2d;

    private Bullet _alian;

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
        _alian = new B_Ailan();
        _alian.damage = damage;
        _alian.shotSpeed = shotSpeed;
        _alian.surviveTime = surviveTime;
        _color = Color.white;
        _bulletSprites[2] = _spriteRenderer.sprite;
    }

    private void Update()
    {
        if (_alian.CheckElapsedTime())
            ObjectPoolingManager.instance.InsertQueue(this, ObjectPoolingManager.instance.queue_alianBullet);
        if (_isCrash)
        {
            _color.a = Mathf.Clamp(_color.a - 0.031372f, 0f, 1f);  // 0.5초에 사라지게 함.
            _spriteRenderer.color = _color;
            if (_color.a <= 0f)
                ObjectPoolingManager.instance.InsertQueue(this, ObjectPoolingManager.instance.queue_alianBullet);
        }
        else
            transform.Translate(_alian.Move());
    }

    private void OnDisable()
    {
        _color.a = 1.0f;
        _spriteRenderer.color = _color;
        _spriteRenderer.sprite = _bulletSprites[2];
        _isCrash = false;
        _collider2d.enabled = true;
        _alian.ResetElapsedTime();
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
                _collider2d.enabled = false;
                SoundManager.instance.PlaySFX("bulletTowall");
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Player":
                Player.instance.Attacked(damage);
                _isCrash = true;
                _collider2d.enabled = false;
                _spriteRenderer.sprite = _bulletSprites[Random.Range(0, 2)];
                break;
        }
    }

    public void Direction(Vector2 _direction)
    {
        _alian.Direction(_direction);
    }
}
