using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D _rigidbody2d;
    private Animator _animator;

    [Header("적 능력치 설정")]
    [SerializeField] private float HP = default;
    [SerializeField] private int score = default;
    [SerializeField] private int minGold = default;
    [SerializeField] private int maxGold = default;

    private void Awake()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
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
                ObjectPoolingManager.instance.GetQueue_gold(transform.position, Random.Range(minGold,maxGold));
                // 나중에 Parameter로 goldMin, goldMax도 같이 넘겨줄 것...
                _animator.SetTrigger("dead");
            }
        }
    }

    public void Spawn(Vector2 _direction, float _shotspeed)
    {
        gameObject.SetActive(true);
        _rigidbody2d.velocity = _direction * _shotspeed;
    }
}
