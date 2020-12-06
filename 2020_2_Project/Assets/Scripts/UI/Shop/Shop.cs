using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public Text skillPoint, haveMoney, playerHP, playerSpeed, playerShield;

    [SerializeField] private int upgradeValue_playerHP = default,
        upgradeValue_playerSpeed = default,
        upgradeValue_playerShield = default;

    [SerializeField] private ShopWeaponIcon[] _shopWeaponIcons = default;
    [SerializeField] private ShopGetWeapon _shopGetWeaponWindow = default;
    [SerializeField] private ShopUpgradeWeapon _shopUpgradeWeaponWindow = default;

    private void OnEnable()
    {
        skillPoint.text = FileManager.playerInfo["skillpoint"].ToString();
        haveMoney.text = FileManager.playerInfo["gold"].ToString();
        playerHP.text = FileManager.playerInfo["hp"].ToString();
        playerShield.text = FileManager.playerInfo["shield"].ToString();
        playerSpeed.text = FileManager.playerInfo["speed"].ToString();
    }

    private void Upgrade()
    {
        FileManager.playerInfo["skillpoint"]--;
        FileManager.WriteData("DB_int_player.csv", FileManager.playerInfo);
        skillPoint.text = FileManager.playerInfo["skillpoint"].ToString();
        SoundManager.instance.PlaySFX("selectui");
    }

    public void UpgradePlayerHP()
    {
        if(FileManager.playerInfo["skillpoint"] > 0)
        {
            FileManager.playerInfo["hp"] += upgradeValue_playerHP;
            playerHP.text = FileManager.playerInfo["hp"].ToString();
            Upgrade();
        }
    }

    public void UpgradePlayerShield()
    {
        if (FileManager.playerInfo["skillpoint"] > 0)
        {
            FileManager.playerInfo["shield"] += upgradeValue_playerShield;
            playerShield.text = FileManager.playerInfo["shield"].ToString();
            Upgrade();
        }
    }

    public void UpgradePlayerSpeed()
    {
        if (FileManager.playerInfo["skillpoint"] > 0)
        {
            FileManager.playerInfo["speed"] += upgradeValue_playerSpeed;
            playerSpeed.text = FileManager.playerInfo["speed"].ToString();
            Upgrade();
        }
    }

    public void TouchWeaponIcon(int _index)
    {
        if(_shopWeaponIcons[_index].isembargo) // 무기가 해금되어 있는 상태라면
            _shopUpgradeWeaponWindow.Init(_shopWeaponIcons[_index]); // 업그레이드를 위한 윈도우를 띄워준다.
        else
            _shopGetWeaponWindow.Init(_shopWeaponIcons[_index]); // 해금을 위한 윈도우를 띄워준다.
        SoundManager.instance.PlaySFX("selectui");
    }
}
