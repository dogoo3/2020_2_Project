using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] private int _damage = default;
    [Header("넉백시킬 순간적인 힘의 최소값, 최대값, 최대값은 점프값보다 낮게.")]
    [SerializeField] private int _minKnockBackForce = default;
    [SerializeField] private int _maxKnockBackForce = default;
    [Header("발사각의 최소값, 최대값. 0 ~ 180 사이")]
    [SerializeField] private float _minKnockBackAngle = default;
    [SerializeField] private float _maxKnockBackAngle = default;

    private Vector2 _knockback;
    private float _angle;
    private int _force;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _angle = Random.Range(_minKnockBackAngle, _maxKnockBackAngle);
            _force = Random.Range(_minKnockBackForce, _maxKnockBackForce);
            _knockback.x = Mathf.Sin(_angle * Mathf.Deg2Rad);
            _knockback.y = Mathf.Cos(_angle * Mathf.Deg2Rad);
            Player.instance.KnockBack(_damage, _knockback * _force);
        }
    }
}
