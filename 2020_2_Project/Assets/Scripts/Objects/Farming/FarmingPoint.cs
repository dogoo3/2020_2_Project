﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmingPoint : MonoBehaviour
{
    [Header("오오라 파밍존인가?")]
    public bool isauraZone;

    public void Spawn(int _itemNum)
    {
        if (isauraZone)
            FarmingManager.instance.DeleteAuraPoint(this);
        else
            FarmingManager.instance.DeletePoint(this); // 내 Zone에서 Item이 Spawn되었기 때문에 중복 Spawn을 막음.
        switch(_itemNum)
        {
            case 0: // Pistol
                ObjectPoolingManager.instance.GetQueue_Item_pistol(transform.position, this);
                break;
            case 1: // SMG
                ObjectPoolingManager.instance.GetQueue_Item_smg(transform.position, this);
                break;
            case 2: // Sniper
                ObjectPoolingManager.instance.GetQueue_Item_sniper(transform.position, this);
                break;
            case 3: // AR
                ObjectPoolingManager.instance.GetQueue_Item_ar(transform.position, this);
                break;
            case 4: // SG
                ObjectPoolingManager.instance.GetQueue_Item_sg(transform.position, this);
                break;
            case 5: // Grenade
                ObjectPoolingManager.instance.GetQueue_Item_grenade(transform.position, this);
                break;
            case 6: // HP 
                ObjectPoolingManager.instance.GetQueue_Item_hp(transform.position, this);
                break;
            case 7: // Shield
                ObjectPoolingManager.instance.GetQueue_Item_shield(transform.position, this);
                break;
        }
    }

    public void GetItem() // 아이템 획득 시 아이템을 소환해도 된다는 신호를 주는 함수.
    {
        if (isauraZone)
            FarmingManager.instance.AddAuraPoint(this);
        else
            FarmingManager.instance.AddPoint(this);
    }
}
