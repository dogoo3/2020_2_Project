using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMonstersManager : MonoBehaviour
{
    public static SpawnMonstersManager instance;

    public SpawnMonstersPoint[] spawnMonstersPoint;

    private List<Enemy> _copyEnemy = new List<Enemy>();
    [SerializeField] private List<Enemy> enemy;
    [Header("몬스터 스폰 간격")]
    [SerializeField] private float elapseSpawnTime;
    
    private int _spawnEnemyIndex; // enemy List에서 소환할 적 인덱스 번호 랜덤값
    private int _spawnPointIndex; // 소환 포인트 인덱스 번호 랜덤값 

    private int _killNumber; // 죽여야 하는 몬스터의 마릿수
    private int _nowkillNumber; // 현재 죽인 몬스터의 마릿수

    private void Awake()
    {
        instance = this;
        _killNumber = enemy.Count; // 적 배열의 길이 = 죽여야 할 몬스터의 마릿수
        ResetEnemyList();
    }

    public void GameStart() // Animator Func
    {
        if (_copyEnemy.Count != 0)
            InvokeRepeating("SpawnRandomMonster", 0f, elapseSpawnTime);
    }

    private void SpawnRandomMonster()
    {
        _spawnEnemyIndex = Random.Range(0, _copyEnemy.Count); // 0 ~ 남은 적 개수의 범위에서 랜덤값 산출.
        _spawnPointIndex = Random.Range(0, spawnMonstersPoint.Length); // 0 ~ 스폰 포인트의 범위에서 랜덤값 산출.

        spawnMonstersPoint[_spawnPointIndex].Spawn(_copyEnemy[_spawnEnemyIndex]); // 랜덤으로 정해진 스폰포인트 인덱스의 위치에서 랜덤으로 정해진 적을 소환.
        _copyEnemy.RemoveAt(_spawnEnemyIndex); // 소환한 적은 리스트에서 탈출
        if (_copyEnemy.Count == 0)
            CancelInvoke("SpawnRandomMonster");
    }

    public void CatchMonster()
    {
        _nowkillNumber++; // 처치 수 ++
        if (_killNumber == _nowkillNumber) // 라운드의 모든 몬스터 처치 시
            Invoke("ShowClearWindow", 3.0f); // 클리어 윈도우를 띄운다.
    }

    public void ResetEnemyList()
    {
        for (int i = 0; i < enemy.Count; i++)
            enemy[i].gameObject.SetActive(false);
        _nowkillNumber = 0;
        _copyEnemy.Clear();
        _copyEnemy.AddRange(enemy);
        CancelInvoke("SpawnRandomMonster");
    }

    public void AddMonster(Enemy _enemy)
    {
        enemy.Add(_enemy);
    }

    private void ShowClearWindow()
    {
        if (!Player.instance.GetisDead()) // 플레이어가 죽지 않았을 경우
            WindowManager.instance.ShowClearWindow();
    }


    
}
