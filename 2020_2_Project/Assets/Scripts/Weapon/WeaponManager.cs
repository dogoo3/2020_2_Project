using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public enum WeaponName
{
    pistol,
    smg,
    sniper,
    ar,
    sg,
    grenade
}

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager instance;
    public Weapon pistol, smg, sniper, ar, sg, grenade;

    public Weapon[] weapons;

    private WeaponSelector[] _weaponSelectors;
    private WeaponName selectWeapon;

    private int i;// for문 돌림용

    private void Awake()
    {
        instance = this;
        _weaponSelectors = GetComponentsInChildren<WeaponSelector>();
        selectWeapon = WeaponName.pistol;
        SetCommand();
    }

    private void SetCommand()
    {
        weapons = new Weapon[6];

        weapons[0] = new Pistol();
        Copy_Paste(weapons[0], pistol);

        weapons[1] = new SMG();
        Copy_Paste(weapons[1], smg);

        weapons[2] = new Sniper();
        Copy_Paste(weapons[2], sniper);

        weapons[3] = new AR();
        Copy_Paste(weapons[3], ar);

        weapons[4] = new SG();
        Copy_Paste(weapons[4], sg);

        weapons[5] = new Grenade();
        Copy_Paste(weapons[5], grenade);

        for (i = 0; i < weapons.Length; i++)
            weapons[i].Init();
    }

    private void Copy_Paste(Weapon _paste, Weapon _copy)
    {
        _paste.text_bulletcount = _copy.text_bulletcount;
        _paste.weaponNum = _copy.weaponNum;
        _paste.bulletCount = _copy.bulletCount;
        _paste.coolTime = _copy.coolTime;
    }

    public void ChangeSelectWeapon(WeaponName _weaponname)
    {
        selectWeapon = _weaponname;
    }

    public void Shoot(Vector2 _origin, Vector2 _direction)
    {
        weapons[(int)selectWeapon].Shoot(_origin, _direction);
        if (weapons[(int)selectWeapon].bulletCount == 0)
            _weaponSelectors[(int)selectWeapon].Exhaust();
    }

    private void Update()
    {
        // 무기들의 쿨타임 계산
        for (i = 0; i < weapons.Length; i++)
            weapons[i].LoadingCooltime();
    }
}
