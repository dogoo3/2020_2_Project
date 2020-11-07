﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUpgradeWeapon : MonoBehaviour
{
    public Image weaponImage;
    public Text dealText, respawnText;

    public Text shopGoldText;

    // public Text dealCostText, respawnCostText;
    public Image upgDamageImage, upgCooltimeImage;

    private ShopWeaponIcon _weaponInfo;
    
    public void Init(ShopWeaponIcon _info)
    {
        _weaponInfo = _info;
        weaponImage.sprite = _weaponInfo._image.sprite;
        dealText.text = _weaponInfo.damage.ToString();
        respawnText.text = _weaponInfo.cooltime.ToString();
        //dealCostText.text = _weaponInfo.cost_upgradeDamage.ToString();
        //respawnCostText.text = _weaponInfo.cost_upgradeCooltime.ToString();
        upgDamageImage.fillAmount = FileManager.weaponLevel[_weaponInfo.weaponName + "_deal"] * 0.2f;
        upgCooltimeImage.fillAmount = FileManager.weaponLevel[_weaponInfo.weaponName + "_respawn"] * 0.2f;
        
        gameObject.SetActive(true);
    }

    public void UpgradeDeal()
    {
        if (FileManager.weaponLevel[_weaponInfo.weaponName + "_deal"] < 5)
        {
            if (_weaponInfo.cost_upgradeDamage <= FileManager.playerInfo["gold"])
            {
                FileManager.playerInfo["gold"] -= _weaponInfo.cost_upgradeDamage;
                FileManager.WriteData("DB_int_player.csv", FileManager.playerInfo);
                _weaponInfo.damage += _weaponInfo.upgradeDamage; // 데미지는 플러스
                _weaponInfo.UpdateWeaponDealInfo();
                dealText.text = _weaponInfo.damage.ToString();
                shopGoldText.text = FileManager.playerInfo["gold"].ToString();
                upgDamageImage.fillAmount = ++FileManager.weaponLevel[_weaponInfo.weaponName + "_deal"] * 0.2f;
                FileManager.WriteData("DB_int_weaponlevel.csv", FileManager.weaponLevel);
            }
        }
    }

    public void UpgradeCooltime()
    {
        if(FileManager.weaponLevel[_weaponInfo.weaponName + "_respawn"] < 5)
        {
            if (_weaponInfo.cost_upgradeCooltime <= FileManager.playerInfo["gold"])
            {
                FileManager.playerInfo["gold"] -= _weaponInfo.cost_upgradeCooltime;
                FileManager.WriteData("DB_int_player.csv", FileManager.playerInfo);
                _weaponInfo.cooltime -= _weaponInfo.upgradeCooltime; // 쿨타임은 마이너스
                _weaponInfo.UpdateWeaponCooltimeInfo();
                respawnText.text = _weaponInfo.cooltime.ToString();
                shopGoldText.text = FileManager.playerInfo["gold"].ToString();
                upgCooltimeImage.fillAmount = ++FileManager.weaponLevel[_weaponInfo.weaponName + "_respawn"] * 0.2f;
                FileManager.WriteData("DB_int_weaponlevel.csv", FileManager.weaponLevel);
            }
        }
    }

    public void ExitButton()
    {
        gameObject.SetActive(false);
    }
}
