using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopWeaponIcon : MonoBehaviour
{
    [HideInInspector]
    public bool isembargo; // 현재 해금되어 있는 상태인가? -> True면 해금, False면 봉인.
    [HideInInspector]
    public int cost; // 해금을 하기 위한 비용
    [HideInInspector]
    public int damageLevel, cooltimeLevel; // 업그레이드 레벨
    [HideInInspector]
    public Image _image;
    [HideInInspector]
    public float damage, cooltime; // 무기 데미지, 쿨타임

    [Header("pistol, smg, sniper, ar, sg, grenade -> 대소문자구분!!!")]
    public string weaponName;

    public float upgradeDamage, upgradeCooltime; // 업그레이드 1회시 증가할 양
    public int cost_upgradeDamage, cost_upgradeCooltime; // 업그레이드를 하기 위한 비용

    public GameObject lockIcon; // 잠금 아이콘 오브젝트(하위)
    private string _fileindex_deal, _fileindex_respawn, _fileindex_cost;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _fileindex_deal = "_deal";
        _fileindex_respawn = "_respawn";
        _fileindex_cost = "_cost";
        isembargo = FileManager.weaponembargo[weaponName];
        if (isembargo)
        {
            if (lockIcon != null)
                lockIcon.SetActive(false);
        }
        else
        {
            if (lockIcon != null)
                lockIcon.SetActive(true);
        }
        if(FileManager.weaponInfo.ContainsKey(weaponName + _fileindex_cost))
            cost = (int)FileManager.weaponInfo[weaponName + _fileindex_cost];
        damage = FileManager.weaponInfo[weaponName + _fileindex_deal];
        cooltime = FileManager.weaponInfo[weaponName + _fileindex_respawn];
    }

    public void UnLock()
    {
        lockIcon.SetActive(false);
    }

    public void UpdateWeaponDealInfo()
    {
        FileManager.weaponInfo[weaponName + _fileindex_deal] += upgradeDamage;
        FileManager.WriteData("DB_float_weaponinfo.csv", FileManager.weaponInfo);
    }

    public void UpdateWeaponCooltimeInfo()
    {
        FileManager.weaponInfo[weaponName + _fileindex_respawn] += upgradeCooltime;
        FileManager.WriteData("DB_float_weaponinfo.csv", FileManager.weaponInfo);
    }
}
