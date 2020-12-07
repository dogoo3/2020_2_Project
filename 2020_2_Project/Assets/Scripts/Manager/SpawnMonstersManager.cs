using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMonstersManager : MonoBehaviour
{
    public static SpawnMonstersManager instance;
    
    public List<SpawnMonstersPoint> spawnMonstersPoint; // 등록 및 연산에 적용될 몬스터 소환 포인트

    private List<SpawnMonstersPoint> _copypoint = new List<SpawnMonstersPoint>(); // 백업용 몬스터 소환 포인트

    private List<Enemy> _copyEnemy = new List<Enemy>(); // 연산에 적용될 적 오브젝트 List
    [SerializeField] private List<Enemy> enemy = default; // 백업용 및 등록을 위한 적 오브젝트 List
    [Header("몬스터 스폰 간격")]
    [SerializeField] private float elapseSpawnTime = default;

    private bool _isEndSpawn; // 몬스터를 소환할 때 소환 포인트의 소환 횟수가 0인지를 판단.

    private int _spawnEnemyIndex; // enemy List에서 소환할 적 인덱스 번호 랜덤값
    private int _spawnPointIndex; // 소환 포인트 인덱스 번호 랜덤값 

    private int _killNumber; // 죽여야 하는 몬스터의 마릿수
    private int _nowkillNumber; // 현재 죽인 몬스터의 마릿수

    private void Awake()
    {
        instance = this;
        CopyData(spawnMonstersPoint, _copypoint); // 라운드 재시작 시 스폰 위치 데이터를 복구하기 위한 백업 변수
        _killNumber = enemy.Count; // 적 배열의 길이 = 죽여야 할 몬스터의 마릿수
        ResetEnemyList();
    }

    // _copyVar에 _copyedVar을 Deep Copy한다.
    public void CopyData(List<SpawnMonstersPoint> _copyVar, List<SpawnMonstersPoint> _copyedVar)
    {
        SpawnMonstersPoint temp;
        _copyedVar.Clear(); // 복사될 List의 모든 원소 삭제 
        for (int i = 0; i < _copyVar.Count; i++) // 깊은 복사 진행
        {
            temp = _copyVar[i];
            _copyedVar.Add(temp);
        }
    }

    public void GameStart() // Animator Func
    {
        if (_copyEnemy.Count != 0)
            InvokeRepeating("SpawnRandomMonster", 0f, elapseSpawnTime);
    }

    private void SpawnRandomMonster() // Invoke Func
    {
        _spawnEnemyIndex = Random.Range(0, _copyEnemy.Count); // 0 ~ 남은 적 개수의 범위에서 랜덤값 산출.
        _spawnPointIndex = Random.Range(0, spawnMonstersPoint.Count); // 0 ~ 스폰 포인트의 범위에서 랜덤값 산출.

        _isEndSpawn = spawnMonstersPoint[_spawnPointIndex].Spawn(_copyEnemy[_spawnEnemyIndex]); // 랜덤으로 정해진 스폰포인트 인덱스의 위치에서 랜덤으로 정해진 적을 소환. true반환시 더이상 스폰 못 함을 알려줌.
        _copyEnemy.RemoveAt(_spawnEnemyIndex); // 소환한 적은 리스트에서 탈출
        if (_copyEnemy.Count == 0)
            CancelInvoke("SpawnRandomMonster");
        if (_isEndSpawn)
            spawnMonstersPoint.RemoveAt(_spawnPointIndex);
    }

    public void SpawnMonster(Enemy _enemy) // 외부에서 몬스터를 소환할 일이 있는 경우
    {
        _spawnPointIndex = Random.Range(0, _copypoint.Count); // 0 ~ 스폰 포인트의 범위에서 랜덤값 산출.
        _copypoint[_spawnPointIndex].Spawn(_enemy); // 랜덤으로 정해진 스폰포인트 인덱스의 위치에서 랜덤으로 정해진 적을 소환.
        _killNumber++; // 죽여야 하는 몬스터의마릿수를 1 증가시킴
    }

    public void CatchMonster()
    {
        _nowkillNumber++; // 처치 수 ++
        if (_killNumber == _nowkillNumber) // 라운드의 모든 몬스터 처치 시
        {
            Invoke("ShowClearWindow", 3.0f); // 클리어 윈도우를 띄운다.
            TimeManager.instance.SetTimer(false);
        }
    }

    public void ResetEnemyList()
    {
        for (int i = 0; i < enemy.Count; i++)
            enemy[i].gameObject.SetActive(false);
        _nowkillNumber = 0;
        _copyEnemy.Clear();
        _copyEnemy.AddRange(enemy);
        _killNumber = enemy.Count; // 적 배열의 길이 = 죽여야 할 몬스터의 마릿수
        CopyData(_copypoint, spawnMonstersPoint);
        for (int i = 0; i < spawnMonstersPoint.Count; i++)
            spawnMonstersPoint[i].ResetCount();
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
