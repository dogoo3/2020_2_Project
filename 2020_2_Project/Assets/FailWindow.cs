using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FailWindow : MonoBehaviour
{
    [SerializeField] private Text text_score;
    [SerializeField] private Player _player;

    public void Init()
    {
        ScoreManager.instance.ShowResultScore(text_score);
        gameObject.SetActive(true);
    }

    public void TouchRestartButton()
    {
        // 몬스터 풀 초기화
        SpawnMonstersManager.instance.ResetEnemyList();
    }

    public void TouchExitButton()
    {
        SceneManager.LoadScene("Title");
    }
}
