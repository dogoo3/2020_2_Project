using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMonstersPoint : MonoBehaviour
{
    [Header("0도 : (0,1)방향, 시계반대방향으로 돌아감.")]
    [Range(0, 360)]
    public int shotAngle;
    [Space()]
    [Header("발사할 강도")]
    public int shotPower; 
    private Vector2 _shotDirection;

    private void Awake()
    {
        // 몬스터가 소환될 방향벡터 설정
        _shotDirection.x = Mathf.Cos(shotAngle * Mathf.Deg2Rad);
        _shotDirection.y = Mathf.Sin(shotAngle * Mathf.Deg2Rad);
        _shotDirection.Normalize();
    }

    public void Spawn(Enemy _enemy)
    {
        _enemy.transform.position = transform.position;
        _enemy.Spawn(_shotDirection, shotPower);
    }
}

