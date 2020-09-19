using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public string bgmname;

    public SpawnMonsters[] spawnMonstersPoint;

    public List<Enemy> enemy;

    private bool _gameStart = false;
    private bool _gameEnd = false;
    
    private int _spawnEnemyIndex;
    private int _spawnPointIndex;

    private void Awake()
    {
        SoundManager.instance.bgmPlayer.Stop();    
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Application.platform == RuntimePlatform.Android)
                Application.Quit();
        }
    }

    public void GameStart() // Animator Func
    {
        SoundManager.instance.PlayBGM(bgmname);
        if (enemy.Count != 0)
            InvokeRepeating("SpawnRandomMonster", 0f, 6.0f);
    }

    public void PlayBeepSound() // Animator Func
    {
        SoundManager.instance.PlaySFX("beepsound");
    }

    private void SpawnRandomMonster()
    {
        _spawnEnemyIndex = Random.Range(0, enemy.Count); // 0 ~ 남은 적 개수의 범위에서 랜덤값 산출.
        _spawnPointIndex = Random.Range(0, spawnMonstersPoint.Length); // 0 ~ 스폰 포인트의 범위에서 랜덤값 산출.

        spawnMonstersPoint[_spawnPointIndex].Spawn(enemy[_spawnEnemyIndex]); // 랜덤으로 정해진 스폰포인트 인덱스의 위치에서 랜덤으로 정해진 적을 소환.
        enemy.RemoveAt(_spawnEnemyIndex); // 소환한 적은 리스트에서 탈출
        if (enemy.Count == 0)
            CancelInvoke("SpawnRandomMonster");
    }
}
