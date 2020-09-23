using UnityEngine;
using System.Collections;

public class Gold : MonoBehaviour
{
    private Rigidbody2D _rigidbody2d;
    private Collider2D _collider2d;

    private int _goldValue;

    private void Awake()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _collider2d = GetComponent<Collider2D>();
    }

    public void SetGoldValue()
    {
        _goldValue = Random.Range(50, 201);
    }

    private void OnEnable()
    {
        _rigidbody2d.velocity = new Vector2(0, 4.0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            GoldManager.instance.PlusTempMoney(_goldValue);
            ObjectPoolingManager.instance.InsertQueue_gold(this);
        }
        else if(collision.CompareTag("bullet"))
            ObjectPoolingManager.instance.InsertQueue_gold(this);
    }
}
