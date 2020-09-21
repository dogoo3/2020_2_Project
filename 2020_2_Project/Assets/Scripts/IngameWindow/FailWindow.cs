using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FailWindow : MonoBehaviour
{
    [SerializeField] private Text text_score;
    [SerializeField] private Player _player;

    [SerializeField] private Transform[] roundStartPos;

    private Vector3[] _cameraPos;

    private void Awake()
    {
        _cameraPos = new Vector3[roundStartPos.Length];
        // 카메라 포지션의 z값을 따로 지정해줘야 하기 때문에 Vector3 변수를 따로 만들어준다.

        for (int i = 0; i < roundStartPos.Length; i++)
        {
            _cameraPos[i] = roundStartPos[i].position;
            _cameraPos[i].z = -10;
        }
    }

    public void Init()
    {
        ScoreManager.instance.ShowResultScore(text_score);
        gameObject.SetActive(true);
    }

    public void TouchRestartButton()
    {
        // HP / 실드 초기화(풀로 채우기)
        _player.ResetGauge();
        // 카메라 & 플레이어 이동
        _player.transform.position = roundStartPos[RoundManager.instance.nowRound].position;
        Camera.main.transform.position = _cameraPos[RoundManager.instance.nowRound];
        // 몬스터 풀 초기화
    }

    public void TouchExitButton()
    {
        SceneManager.LoadScene("Title");
        SoundManager.instance.PlayBGM("TalesWeaver_Title");
    }
}
