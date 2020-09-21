using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ClearWindow : MonoBehaviour
{
    [SerializeField] private Text text_score;
    [SerializeField] private Player _player;

    [SerializeField] private SpawnMonstersManager[] rounds;

    [SerializeField] private Transform[] roundStartPos;

    private Vector3[] _cameraPos;

    private void Awake()
    {
        _cameraPos = new Vector3[roundStartPos.Length];
        // 카메라 포지션의 z값을 따로 지정해줘야 하기 때문에 Vector3 변수를 따로 만들어준다. 
        for(int i=0;i<roundStartPos.Length;i++)
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

    public void TouchNextStageButton()
    {
        // HP / 실드 초기화(풀로 채우기)
        _player.ResetGauge();
        // 카메라 & 플레이어 이동
        _player.transform.position = roundStartPos[RoundManager.instance.nowRound].position;
        Camera.main.transform.position = _cameraPos[RoundManager.instance.nowRound];
        // SpawnMonstersManager에서 관련 내용 초기화(다음 라운드로 넘겨줌)
        rounds[RoundManager.instance.nowRound++].gameObject.SetActive(false); // 현재 라운드의 스폰매니저 비활성화 후 1라운드 증가
        rounds[RoundManager.instance.nowRound].gameObject.SetActive(true);

        // WeaponManager에서 무기 탄알 수 초기화하기
        //switch (RoundManager.instance.nowRound)
        //{
        //    case 1: // 1라운드 -> 2라운드
        //        WeaponManager.instance.SetCommand(10, 10, 10, 10, 10, 2);
        //        break;
        //    case 2: // 2라운드 -> 3라운드
        //        WeaponManager.instance.SetCommand(20, 20, 20, 20, 20, 3);
        //        break;
        //}

        // WeaponManager에서 이전 라운드 때 다 쓴 무기 활성화시키기
        WeaponManager.instance.EnableWeapon();

        // ScoreManager의 score 0으로 초기화
        ScoreManager.instance.ResetScore();

        // 카운터 재시작 및 브금 일시중지
        RoundManager.instance.EnableAnimator(); // ON하면 애니메이션이 시작됨
        RoundManager.instance.StopBGM();

        // 이 UI 없앰
        gameObject.SetActive(false);
    }

    public void TouchExitButton()
    {
        SceneManager.LoadScene("Title");
        SoundManager.instance.PlayBGM("TalesWeaver_Title");
    }
}
