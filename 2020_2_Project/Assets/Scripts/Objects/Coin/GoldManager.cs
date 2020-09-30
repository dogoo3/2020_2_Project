using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldManager : MonoBehaviour
{
    public static GoldManager instance;

    private Text _goldText;

    private int _roundGetMoney; // 라운드당 유효한 골드 획득
    private int _stageGetMoney; // 스테이지 전체에서 유효한 골드 획득

    private void Awake()
    {
        instance = this;
        _goldText = GetComponentInChildren<Text>();
        _roundGetMoney = 0;
        _stageGetMoney = 0;
    }

    public void GetGold(int _value) // 코인을 얻었을 때
    {
        _roundGetMoney += _value;
        WriteGoldText(_roundGetMoney + _stageGetMoney);
    }

    public void FailResetGold() // 라운드 중 죽었을 때
    {
        _roundGetMoney = 0;
        WriteGoldText(_stageGetMoney);
    }

    public void RoundClearGold() // 라운드를 클리어했을 때
    {
        _stageGetMoney += _roundGetMoney;
    }

    public void AllClearGold()
    {
        FileManager.playerInfo["gold"] += _stageGetMoney + _roundGetMoney;
        FileManager.WriteData("DB_int_player.csv", FileManager.playerInfo);
    }

    private void WriteGoldText(int _value)
    {
        _goldText.text = _value.ToString();
    }
}
