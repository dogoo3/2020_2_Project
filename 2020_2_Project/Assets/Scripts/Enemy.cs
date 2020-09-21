using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D _rigidbody2d;
    private Animator _animator;

    [Header("적 능력치 설정")]
    [SerializeField] private float HP = default;
    [SerializeField] private float speed = default;
    [SerializeField] private float runSpeed = default;
    [SerializeField] private float damage = default;
    [SerializeField] private float attackCooltime = default;
    [SerializeField] private int score = default;
    [SerializeField] private int dropGold = default;

    private Player _tempPlayer;

    private void Awake()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "Character")
        {
            if (_tempPlayer == null)
                _tempPlayer = collision.GetComponent<Player>();
            _tempPlayer.Attacked(damage);
        }
    }

    public void MinusHP(float _hp)
    {
        HP -= _hp;
        if (HP <= 0)
        {
            if (gameObject.activeSelf) // Translate 연산을 해서 한 프레임에 여러 발의 샷건 총알이 맞을 수 있기 때문에 한 발만 적용되도록 코드 수정.
            {
                ScoreManager.instance.UpdateScore(score);
                SpawnMonstersManager.instance.CatchMonster();
                gameObject.SetActive(false);
            }
        }
    }

    public void Spawn(Vector2 _direction, float _shotspeed)
    {
        gameObject.SetActive(true);
        _rigidbody2d.velocity = _direction * _shotspeed;
    }

    // 스코어 매니저 및 골드 매니저 만들어야 함
}
