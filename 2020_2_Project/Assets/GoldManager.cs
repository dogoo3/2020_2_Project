using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldManager : MonoBehaviour
{
    public static GoldManager instance;

    private int _ingameGetMoney; // 인게임 중에 획득한 골드

    private void Awake()
    {
        instance = this;
        // _myMoney = FileManager.playerInfo["gold"]; // 나중에 csv파일 만들면 적용
    }

    public void PlusTempMoney(int _value) // 스테이지 클리어 전에 획득한 골드를 저장
    {
        _ingameGetMoney += _value;
        GoldWindow.instance.UpdateText(_ingameGetMoney);
    }

    public void ResetTempMoney()
    {
        _ingameGetMoney = 0;
    }

    public void GetMyMoney() // 스테이지가 완료되면 획득한 골드를 한 번에 저장
    {
        // FileManager.playerInfo["gold"] += _ingameGetMoney; // 나중에 csv파일 만들면 적용
        // FileManager.WriteData("DB_player.csv", FileManager.playerInfo);
        ResetTempMoney();
    }

    public void UseMyMoney(int _value)
    {
        // FileManager.playerInfo["gold"] -= _value; // 나중에 csv파일 만들면 적용
        // FileManager.WriteData("DB_player.csv", FileManager.playerInfo);
    }
}
