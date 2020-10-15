using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMonsterSpawn : MonoBehaviour
{
    private SpawnMonstersManager _spawnMonstersManager;

    [Header("몬스터 프리팹 삽입 및 생성 몬스터 수 입력")]
    [SerializeField] private Enemy[] _enemies = default;
    [SerializeField] private int _monsterCount = default;

    private Enemy _tempEnemy;

    private void Awake()
    {
        _spawnMonstersManager = GetComponent<SpawnMonstersManager>();
        for(int i=0;i<_monsterCount;i++)
        {
            _tempEnemy = Instantiate(_enemies[Random.Range(0, _enemies.Length)], Vector2.zero, Quaternion.identity);
            _tempEnemy.transform.parent = gameObject.transform;
            _spawnMonstersManager.AddMonster(_tempEnemy);
        }
    }
}
