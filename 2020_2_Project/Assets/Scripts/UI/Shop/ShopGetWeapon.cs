using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopGetWeapon : MonoBehaviour
{
    public Image image;
    public Text text;

    public Text shopGoldText;

    public Text descText; 

    private ShopWeaponIcon _weaponInfo;

    public void Init(ShopWeaponIcon _info)
    {
        _weaponInfo = _info;
        image.sprite = _info._image.sprite;
        text.text = _info.cost.ToString();
        descText.text = _info.descText;
        gameObject.SetActive(true);
    }

    public void GetButton()
    {
        if(_weaponInfo.cost <= FileManager.playerInfo["gold"])
        {
            FileManager.playerInfo["gold"] -= _weaponInfo.cost;
            _weaponInfo.isembargo = true;
            FileManager.weaponembargo[_weaponInfo.weaponName] = _weaponInfo.isembargo;
            _weaponInfo.UnLock();
            FileManager.WriteData("DB_int_player.csv", FileManager.playerInfo);
            FileManager.WriteData("DB_bool_weaponembargo.csv", FileManager.weaponembargo);
            shopGoldText.text = FileManager.playerInfo["gold"].ToString();
            SoundManager.instance.PlaySFX("selectui");
            ExitButton();
        }
    }

    public void ExitButton()
    {
        gameObject.SetActive(false);
        SoundManager.instance.PlaySFX("selectui");
    }
}
