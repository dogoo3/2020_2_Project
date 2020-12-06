using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonsterSpawn : MonoBehaviour
{
    private Enemy _enemy;

    [SerializeField] private Enemy[] _monsters = default;
    [SerializeField] private float _ActiveTime = default;

    private void Awake()
    {
        _enemy = GetComponent<Enemy>();
    }

    private void OnEnable()
    {
        InvokeRepeating("ActiveMonster", 5.0f, _ActiveTime);
    }

    private void ActiveMonster()
    {
        for (int i = 0; i < _monsters.Length; i++)
        {
            if (!_monsters[i].gameObject.activeSelf) // 좀비 오브젝트가 비활성화되어있는경우
            {
                SpawnMonstersManager.instance.SpawnMonster(_monsters[i]);
                break;
            }
        }
    }

    public void InvalidSpawnMonster() // Animation Func
    {
        CancelInvoke("ActiveMonster");
    }

    private void OnDisable()
    {
        if (_enemy.GetHP() > 0) // 비활성화될때 체력이 0 초과면 플레이어가 사망한 것이므로, 모든 부하몬스터들을 비활성화
        {
            for (int i = 0; i < _monsters.Length; i++)
                _monsters[i].gameObject.SetActive(false);

            CancelInvoke("ActiveMonster");
        }
    }
}
