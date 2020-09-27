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
        // 라운드 획득 골드 적립(내부에서 관리)
        GoldManager.instance.RoundClearCoin();
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
        // 해당 라운드에 획득한 골드 초기화
        GoldManager.instance.FailResetCoin();
        // 아이템 스폰 포인트 초기화
        FarmingManager.instance.ResetRound();
        // 공통 속성 설정
        TouchStageButton();
    }

    public void TouchExitButton(string _stage)
    {
        SoundManager.instance.PlayBGM("TalesWeaver_Title");

        if (_stage != "") // 3스테이지까지 완료했을 경우
        {
            FileManager.stageClear[_stage] = true;
            GoldManager.instance.AllClearCoin();
            FileManager.WriteData("DB_bool_stageclear.csv", FileManager.stageClear);
        }

        // 할당 해제
        WeaponManager.instance = null;
        ScoreManager.instance = null;
        RoundManager.instance = null;
        FarmingManager.instance = null;
        GoldManager.instance = null;
        Player.instance = null;
        GaugeManager.instance = null;
        ObjectPoolingManager.instance = null;
        SpawnMonstersManager.instance = null;
        WindowManager.instance = null;

        SceneManager.LoadScene("Title");
    }

    private void TouchStageButton()
    {
        // WeaponManager에서 무기들의 탄알 수 0으로 만들기
        WeaponManager.instance.ClearBullets();
        // HP / Shield 초기화(풀로 채우기)
        _player.ResetGauge();
        // ScoreManager의 score 0으로 초기화
        ScoreManager.instance.ResetScore();
        // 카운터 재시작 및 브금 일시중지
        RoundManager.instance.EnableAnimator();
        RoundManager.instance.StopBGM();
        // 활성화된 아이템들을 모두 집어넣기
        FarmingManager.instance.ClearItem();
        // 이 UI 없앰
        gameObject.SetActive(false);
    }
}
