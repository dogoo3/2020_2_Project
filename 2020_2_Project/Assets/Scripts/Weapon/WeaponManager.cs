using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum WeaponNumber
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

    [Header("0:pistol, 1:smg 2:sniper, 3:ar, 4:sg, 5:grenade")]

    private Weapon[] _weapons;
    private int _selectWeapon; // 현재 선택되어있는 무기 번호

    private void Awake()
    {
        instance = this;

        _weapons = GetComponentsInChildren<Weapon>();
        _selectWeapon = 0; // 기본 0번
    }

    public void SelectWeapon(int _num)
    {
        _selectWeapon = _num;
    }

    public void Shoot(Vector2 _origin, Vector2 _direction)
    {
        if(_weapons[_selectWeapon].IsShotWeapon())
        {
            switch (_selectWeapon)
            {
                case (int)WeaponNumber.pistol:
                    ObjectPoolingManager.instance.GetQueue_pistol(_origin, _direction); // 권총
                    break;
                case (int)WeaponNumber.smg:
                    ObjectPoolingManager.instance.GetQueue_smg(_origin, _direction); // 소총
                    break;
                case (int)WeaponNumber.sniper:
                    ObjectPoolingManager.instance.GetQueue_sniper(_origin, _direction); // 저격총
                    break;
                case (int)WeaponNumber.ar:
                    ObjectPoolingManager.instance.GetQueue_ar(_origin, _direction); // 기관단총
                    break;
                case (int)WeaponNumber.sg:
                    ObjectPoolingManager.instance.GetQueue_sg(_origin, _direction); // 샷건
                    break;
                case (int)WeaponNumber.grenade:
                    ObjectPoolingManager.instance.GetQueue_grenade(_origin, _direction); // 수류탄
                    break;
            }
            _weapons[_selectWeapon].MinusBulletCount(); // 하나 깎고   
        }
    }
}
