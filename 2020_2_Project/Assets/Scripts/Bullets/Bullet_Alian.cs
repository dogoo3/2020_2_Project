using UnityEngine;
using System.Collections;

public class Bullet_Alian : MonoBehaviour
{
    private Bullet _alian;

    public float damage;
    public float shotSpeed;
    public float surviveTime;

    private void Awake()
    {
        _alian = new B_Ailan();
        _alian.damage = damage;
        _alian.shotSpeed = shotSpeed;
        _alian.surviveTime = surviveTime;
    }

    private void Update()
    {
        if (_alian.CheckElapsedTime())
            ObjectPoolingManager.instance.InsertQueue(this, ObjectPoolingManager.instance.queue_alianBullet);
        transform.Translate(_alian.Move());
    }

    private void OnDisable()
    {
        _alian.ResetElapsedTime();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "ground":
            case "bullet":
            case "wall":
                ObjectPoolingManager.instance.InsertQueue(this, ObjectPoolingManager.instance.queue_alianBullet);
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
                ObjectPoolingManager.instance.InsertQueue(this, ObjectPoolingManager.instance.queue_alianBullet);
                // SoundManager.instance.PlaySFX("bulletToenemy");
                break;
        }
    }

    public void Direction(Vector2 _direction)
    {
        _alian.Direction(_direction);
    }
}
