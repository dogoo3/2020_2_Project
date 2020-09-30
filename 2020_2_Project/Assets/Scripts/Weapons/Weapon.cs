using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Weapon
{
    private bool isShot = true; // 발사 유무

    public Text text_bulletcount;

    public int weaponNum; // 무기번호
    public int bulletCount; // 총알 갯수

    public float coolTime; // 재사용 쿨타임
    public float divCooltime; // UI 연산을 위해 1 / coolTime 을 연산한 값
    private float elapsecooltime; // 진행 쿨타임
    public virtual void Shoot(Vector2 _origin, Vector2 _direction) { }
    public virtual void Supply(int _plusBulletCount) { }

    public void Init()
    {
        text_bulletcount.text = "(" + bulletCount.ToString() + ")";
    }

    protected void SpendBullet()
    {
        bulletCount--;
        Init();
    }

    public bool IsShootWeapon()
    {
        if (bulletCount > 0) // 총알이 남아있는가?
        {
            if (isShot) // 쿨타임이 경과했는가?
            {
                isShot = false; // 쿨타임 상태로 전환한 다음
                return true; // true를 반환하면 총을 발사한다. (자식클래스 참조. if문의 반환bool 함수이기 때문에 쿨타임 시작되자마자 총알을 발사하기 때문)
            }
            else
                return false;
        }
        else
            return false;
    }

    public void LoadingCooltime()
    {
        if (!isShot) // 쿨타임 중이면
        {
            elapsecooltime += Time.deltaTime;
            if (elapsecooltime > coolTime)
            {
                isShot = true;
                elapsecooltime = 0;
            }
        }
    }

    public float GetElapseCooltime()
    {
        return elapsecooltime * divCooltime;
    }

    public bool GetIsShot()
    {
        return isShot;
    }
}

public class Pistol : Weapon // 권총 
{
    public override void Shoot(Vector2 _origin, Vector2 _direction)
    {
        ObjectPoolingManager.instance.GetQueue_pistol(_origin, _direction);
        SpendBullet();
    }

    public override void Supply(int _plusBulletCount)
    {
        bulletCount += _plusBulletCount; // 총알 갯수 올려주고
        Init(); // UI 초기화
    }
}

public class SMG : Weapon // 소총
{
    public override void Shoot(Vector2 _origin, Vector2 _direction)
    {
        ObjectPoolingManager.instance.GetQueue_smg(_origin, _direction);
        SpendBullet();
    }

    public override void Supply(int _plusBulletCount)
    {
        bulletCount += _plusBulletCount; // 총알 갯수 올려주고
        Init(); // UI 초기화
    }
}

public class Sniper : Weapon // 저격소총 
{
    public override void Shoot(Vector2 _origin, Vector2 _direction)
    {
        ObjectPoolingManager.instance.GetQueue_sniper(_origin, _direction);
        SpendBullet();
    }

    public override void Supply(int _plusBulletCount)
    {
        bulletCount += _plusBulletCount; // 총알 갯수 올려주고
        Init(); // UI 초기화
    }
}

public class AR : Weapon // 기관단총
{
    public override void Shoot(Vector2 _origin, Vector2 _direction)
    {
        ObjectPoolingManager.instance.GetQueue_ar(_origin, _direction);
        SpendBullet();
    }

    public override void Supply(int _plusBulletCount)
    {
        bulletCount += _plusBulletCount; // 총알 갯수 올려주고
        Init(); // UI 초기화
    }
}

public class SG : Weapon // 샷건
{
    private int i; // for 돌림용
    private Vector2 dir_bullet; // 산탄 방향
    public int angle = 90;
    public int line = 7;
    public override void Shoot(Vector2 _origin, Vector2 _direction)
    {
        if (_direction.y == 0) // 좌 / 우 시점일 때
        {
            for (i = 0; i < line; i++)
            {
                dir_bullet.x = Mathf.Cos((angle * 0.5f - (angle / (line - 1)) * i) * Mathf.Deg2Rad) * _direction.x;
                dir_bullet.y = Mathf.Sin((angle * 0.5f - (angle / (line - 1)) * i) * Mathf.Deg2Rad);
                ObjectPoolingManager.instance.GetQueue_sg(_origin, dir_bullet);
            }
        }
        else // 위를 바라보고 있을 때 
        {
            for (i = 0; i < line; i++)
            {
                dir_bullet.x = Mathf.Cos((angle * 0.5f + (angle / (line - 1)) * i) * Mathf.Deg2Rad);
                dir_bullet.y = Mathf.Sin((angle * 0.5f + (angle / (line - 1)) * i) * Mathf.Deg2Rad);
                ObjectPoolingManager.instance.GetQueue_sg(_origin, dir_bullet);
            }
        }
        SpendBullet();
    }

    public override void Supply(int _plusBulletCount)
    {
        bulletCount += _plusBulletCount; // 총알 갯수 올려주고
        Init(); // UI 초기화
    }
}

public class Grenade : Weapon // 수류탄
{
    public override void Shoot(Vector2 _origin, Vector2 _direction)
    {
        ObjectPoolingManager.instance.GetQueue_grenade(_origin, _direction);
        SpendBullet();
    }

    public override void Supply(int _plusBulletCount)
    {
        bulletCount += _plusBulletCount; // 총알 갯수 올려주고
        Init(); // UI 초기화
    }
}
