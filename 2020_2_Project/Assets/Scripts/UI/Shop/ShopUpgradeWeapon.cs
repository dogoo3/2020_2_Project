using System.Collections;
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
        dealText.text = (Mathf.Round(_weaponInfo.damage * 100f) / 100f).ToString();
        respawnText.text = (Mathf.Round(_weaponInfo.cooltime * 100f) / 100f).ToString();
        //dealCostText.text = _weaponInfo.cost_upgradeDamage.ToString();
        //respawnCostText.text = _weaponInfo.cost_upgradeCooltime.ToString();
        upgDamageImage.fillAmount = FileManager.weaponLevel[_weaponInfo.weaponName + "_deal"] * 0.25f;
        upgCooltimeImage.fillAmount = FileManager.weaponLevel[_weaponInfo.weaponName + "_respawn"] * 0.25f;
        
        gameObject.SetActive(true);
    }

    public void UpgradeDeal()
    {
        if (FileManager.weaponLevel[_weaponInfo.weaponName + "_deal"] < 4)
        {
            if (_weaponInfo.cost_upgradeDamage <= FileManager.playerInfo["gold"])
            {
                FileManager.playerInfo["gold"] -= _weaponInfo.cost_upgradeDamage;
                FileManager.WriteData("DB_int_player.csv", FileManager.playerInfo);
                _weaponInfo.damage += _weaponInfo.upgradeDamage; // 데미지는 플러스
                _weaponInfo.UpdateWeaponDealInfo();
                dealText.text = (Mathf.Round(_weaponInfo.damage * 100f) / 100f).ToString();
                shopGoldText.text = FileManager.playerInfo["gold"].ToString();
                upgDamageImage.fillAmount = ++FileManager.weaponLevel[_weaponInfo.weaponName + "_deal"] * 0.25f;
                FileManager.WriteData("DB_int_weaponlevel.csv", FileManager.weaponLevel);
                SoundManager.instance.PlaySFX("selectui");
            }
        }
    }

    public void UpgradeCooltime()
    {
        if(FileManager.weaponLevel[_weaponInfo.weaponName + "_respawn"] < 4)
        {
            if (_weaponInfo.cost_upgradeCooltime <= FileManager.playerInfo["gold"])
            {
                FileManager.playerInfo["gold"] -= _weaponInfo.cost_upgradeCooltime;
                FileManager.WriteData("DB_int_player.csv", FileManager.playerInfo);
                _weaponInfo.cooltime -= _weaponInfo.upgradeCooltime; // 쿨타임은 마이너스
                _weaponInfo.UpdateWeaponCooltimeInfo();
                respawnText.text = (Mathf.Round(_weaponInfo.cooltime * 100f) / 100f).ToString();
                shopGoldText.text = FileManager.playerInfo["gold"].ToString();
                upgCooltimeImage.fillAmount = ++FileManager.weaponLevel[_weaponInfo.weaponName + "_respawn"] * 0.25f;
                FileManager.WriteData("DB_int_weaponlevel.csv", FileManager.weaponLevel);
                SoundManager.instance.PlaySFX("selectui");
            }
        }
    }

    public void ExitButton()
    {
        gameObject.SetActive(false);
        SoundManager.instance.PlaySFX("selectui");
    }
}
