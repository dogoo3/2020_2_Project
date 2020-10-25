using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Action
{
    Jump,
    ChangeDir,
    Random
}

enum Arrow
{
    Left,
    right
}

public class ChangeActionPoint : MonoBehaviour
{
    [Header("이 방향에서 접근했다.")]
    [SerializeField] private Arrow _arrow = default;
    [Header("액션의 종류")]
    [SerializeField] private Action _action = default;

    private Enemy _enemy;
    [Header("점프 강도")]
    [SerializeField] private Vector2 _force = default;

    private Vector2 _cal = new Vector2(-1, 1);
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("enemy"))
            _enemy = collision.GetComponent<Enemy>();
        else
            return;
        if (_enemy.GetVelocityForce() == Vector2.zero)
        {
            if (transform.position.x - collision.transform.position.x > 0) // 객체가 왼쪽에서 접근했다.
            {
                if (_arrow == Arrow.right)
                    return;
                switch (_action)
                {
                    case Action.Jump:
                        _enemy.Jump(_force);
                        break;
                    case Action.ChangeDir:
                        _enemy.ChangeDir(_enemy.GetAnimator());
                        break;
                    case Action.Random:
                        if (Random.Range(0, 2) == 0)
                            _enemy.Jump(_force);
                        else
                            _enemy.ChangeDir(_enemy.GetAnimator());
                        break;
                }

            }
            else // 객체가 오른쪽에서 접근했다.
            {
                if (_arrow == Arrow.Left)
                    return;

                switch (_action)
                {
                    case Action.Jump:
                        _enemy.Jump(_force * _cal);
                        break;
                    case Action.ChangeDir:
                        _enemy.ChangeDir(_enemy.GetAnimator());
                        break;
                    case Action.Random:
                        if (Random.Range(0, 2) == 0)
                            _enemy.Jump(_force * _cal);
                        else
                            _enemy.ChangeDir(_enemy.GetAnimator());
                        break;
                }
            }
        }
    }
}
