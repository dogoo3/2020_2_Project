using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverWindow : MonoBehaviour
{
    [SerializeField] private Text text_score;
    [SerializeField] private Player _player;

    [SerializeField] private SpawnMonstersManager[] rounds;
    [SerializeField] private Transform[] roundStartPos;

    private Vector3[] _cameraPos;

    public int[] round1_bulletCount;
    public int[] round2_bulletCount;
    public int[] round3_bulletCount;
    
    private void Awake()
    {
        if (roundStartPos.Length != 0)
        {
            _cameraPos = new Vector3[roundStartPos.Length];
            // 카메라 포지션의 z값을 따로 지정해줘야 하기 때문에 Vector3 변수를 따로 만들어준다.

            for (int i = 0; i < roundStartPos.Length; i++)
            {
                _cameraPos[i] = roundStartPos[i].position;
                _cameraPos[i].z = -10;
            }
        }
    }

    public void Init()
    {
        ScoreManager.instance.ShowResultScore(text_score);
        gameObject.SetActive(true);
    }

    public void TouchNextStageButton()
    {
        /* 다음 라운드 속성 설정 */
        // Clear한 SpawnMonstersManager 오브젝트 비활성화
        rounds[RoundManager.instance.nowRound].gameObject.SetActive(false);
        // 카메라 & 플레이어 이동
        _player.transform.position = roundStartPos[++RoundManager.instance.nowRound].position;
        Camera.main.transform.position = _cameraPos[RoundManager.instance.nowRound];
        // 다음 라운드의 SpawnMonstersManager 활성화
        rounds[RoundManager.instance.nowRound].gameObject.SetActive(true);

        // 공통 속성 설정
        TouchStageButton();
    }

    public void TouchRestartStageButton()
    {
        /* 현재 라운드 속성 재설정 */
        // 카메라 & 플레이어 이동
        _player.transform.position = roundStartPos[RoundManager.instance.nowRound].position;
        Camera.main.transform.position = _cameraPos[RoundManager.instance.nowRound];
        // 몬스터 풀 초기화
        SpawnMonstersManager.instance.ResetEnemyList();

        // 공통 속성 설정
        TouchStageButton();
    }

    public void TouchExitButton()
    {
        SceneManager.LoadScene("Title");
        SoundManager.instance.PlayBGM("TalesWeaver_Title");
    }

    private void TouchStageButton()
    {
        // WeaponManager에서 무기 탄알 수 초기화하기
        switch(RoundManager.instance.nowRound)
        {
            case 0:
                WeaponManager.instance.SetCommand(round1_bulletCount);
                break;
            case 1:
                WeaponManager.instance.SetCommand(round2_bulletCount);
                break;
            case 2:
                WeaponManager.instance.SetCommand(round3_bulletCount);
                break;
        }
        // HP / Shield 초기화(풀로 채우기)
        _player.ResetGauge();
        // WeaponManager에서 이전 라운드 때 다 쓴 무기 활성화시키기
        WeaponManager.instance.EnableWeapon();
        // ScoreManager의 score 0으로 초기화
        ScoreManager.instance.ResetScore();
        // 카운터 재시작 및 브금 일시중지
        RoundManager.instance.EnableAnimator();
        RoundManager.instance.StopBGM();
        // 이 UI 없앰
        gameObject.SetActive(false);
    }
}
