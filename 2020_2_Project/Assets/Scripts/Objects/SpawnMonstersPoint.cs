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
    [Header("최대 스폰 몬스터 수")]
    public int maxSpawnCount; // 등록 및 백업
    private int _maxSpawnCount; // 연산

    private void Awake()
    {
        // 몬스터가 소환될 방향벡터 설정
        _shotDirection.x = Mathf.Cos(shotAngle * Mathf.Deg2Rad);
        _shotDirection.y = Mathf.Sin(shotAngle * Mathf.Deg2Rad);
        _shotDirection.Normalize();
        _maxSpawnCount = maxSpawnCount; // 백업 
    }

    public bool Spawn(Enemy _enemy)
    {
        _enemy.transform.position = transform.position;
        _enemy.Spawn(_shotDirection, shotPower);
        _maxSpawnCount--;
        if (_maxSpawnCount == 0)
            return true;
        else
            return false;
    }

    public void ResetCount()
    {
        _maxSpawnCount = maxSpawnCount;
    }
}

