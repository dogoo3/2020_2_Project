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
    public Weapon pistol, smg, sniper, ar, sg, grenade; // 초기 설정을 잡아주기 위한 변수들(무기번호, 재장전시간, UI 등)

    // 실제 연산에 사용될 무기 배열
    private Weapon[] _weapons;

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
        _weapons = new Weapon[6];

        _weapons[0] = new Pistol();
        Copy_Paste(_weapons[0], pistol);

        _weapons[1] = new SMG();
        Copy_Paste(_weapons[1], smg);

        _weapons[2] = new Sniper();
        Copy_Paste(_weapons[2], sniper);

        _weapons[3] = new AR();
        Copy_Paste(_weapons[3], ar);

        _weapons[4] = new SG();
        Copy_Paste(_weapons[4], sg);

        _weapons[5] = new Grenade();
        Copy_Paste(_weapons[5], grenade);

        for (i = 0; i < _weapons.Length; i++)
            _weapons[i].Init();
    }

    public void ClearBullets() // 라운드가 바뀔 때 총알 갯수를 갱신시켜줌.
    {
        for (i = 0; i < _weaponSelectors.Length; i++)
        {
            _weapons[i].bulletCount = 0;
            _weapons[i].Init();
            _weaponSelectors[i].Exhaust();
        }
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
        _weapons[(int)selectWeapon].Shoot(_origin, _direction);
        if (_weapons[(int)selectWeapon].bulletCount == 0)
            _weaponSelectors[(int)selectWeapon].Exhaust();
    }

    public void Supply(int _weaponIndex, int _count)
    {
        if (_weaponSelectors[_weaponIndex].nonUseWeaponSignal.activeSelf) // 무기 사용 불가 사인이 활성화되어있으면
            _weaponSelectors[_weaponIndex].Exhaust(false); // 사인 없애주고
        _weapons[_weaponIndex].Supply(_count); // 총알 보급
    }

    private void Update()
    {
        // 무기들의 쿨타임 계산
        for (i = 0; i < _weapons.Length; i++)
            _weapons[i].LoadingCooltime();
    }
}
