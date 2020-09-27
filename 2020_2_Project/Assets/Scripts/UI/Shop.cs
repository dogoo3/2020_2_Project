using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public Text skillPoint, haveMoney, playerHP, playerSpeed, playerShield;

    [SerializeField] private int upgradeValue_playerHP, upgradeValue_playerSpeed, upgradeValue_playerShield;

    private void OnEnable()
    {
        skillPoint.text = FileManager.playerInfo["skillpoint"].ToString();
        haveMoney.text = FileManager.playerInfo["gold"].ToString();
        playerHP.text = FileManager.playerInfo["hp"].ToString();
        playerShield.text = FileManager.playerInfo["shield"].ToString();
        playerSpeed.text = FileManager.playerInfo["speed"].ToString();
    }

    public void UpgradePlayerHP()
    {
        if(FileManager.playerInfo["skillpoint"] > 0)
        {
            FileManager.playerInfo["hp"] += upgradeValue_playerHP;
            FileManager.playerInfo["skillpoint"]--;
            FileManager.WriteData("DB_int_player.csv", FileManager.playerInfo);
            playerHP.text = FileManager.playerInfo["hp"].ToString();
            skillPoint.text = FileManager.playerInfo["skillpoint"].ToString();
        }
    }

    public void UpgradePlayerShield()
    {
        if (FileManager.playerInfo["skillpoint"] > 0)
        {
            FileManager.playerInfo["shield"] += upgradeValue_playerShield;
            FileManager.playerInfo["skillpoint"]--;
            FileManager.WriteData("DB_int_player.csv", FileManager.playerInfo);
            playerShield.text = FileManager.playerInfo["shield"].ToString();
            skillPoint.text = FileManager.playerInfo["skillpoint"].ToString();
        }
    }

    public void UpgradePlayerSpeed()
    {
        if (FileManager.playerInfo["skillpoint"] > 0)
        {
            FileManager.playerInfo["shield"] += upgradeValue_playerSpeed;
            FileManager.playerInfo["skillpoint"]--;
            FileManager.WriteData("DB_int_player.csv", FileManager.playerInfo);
            playerSpeed.text = FileManager.playerInfo["speed"].ToString();
            skillPoint.text = FileManager.playerInfo["skillpoint"].ToString();
        }
    }
}
