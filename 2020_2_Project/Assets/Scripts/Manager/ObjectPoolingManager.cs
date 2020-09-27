﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ObjectPoolingManager : MonoBehaviour
{
    public static ObjectPoolingManager instance;

    [Header("프리팹 등록")]
    public Bullet_Pistol pistol;
    public Bullet_SMG smg;
    public Bullet_Sniper sniper;
    public Bullet_AR ar;
    public Bullet_SG sg;
    public Bullet_Grenade grenade;
    public Gold gold;

    public FarmingItem farm_pistol;
    public FarmingItem farm_smg;
    public FarmingItem farm_sniper;
    public FarmingItem farm_ar;
    public FarmingItem farm_sg;
    public FarmingItem farm_grenade;
    public FarmingItem farm_hp;
    public FarmingItem farm_shield;

    // 임시 오브젝트
    private Bullet_Pistol _pistol;
    private Bullet_SMG _smg;
    private Bullet_Sniper _sniper;
    private Bullet_AR _ar;
    private Bullet_SG _sg;
    private Bullet_Grenade _grenade;
    private Gold _gold;

    private FarmingItem _farmItem;

    // 저장 큐
    public Queue<Bullet_Pistol> queue_pistol = new Queue<Bullet_Pistol>();
    public Queue<Bullet_SMG> queue_smg = new Queue<Bullet_SMG>();
    public Queue<Bullet_Sniper> queue_sniper = new Queue<Bullet_Sniper>();
    public Queue<Bullet_AR> queue_ar = new Queue<Bullet_AR>();
    public Queue<Bullet_SG> queue_sg = new Queue<Bullet_SG>();
    public Queue<Bullet_Grenade> queue_grenade = new Queue<Bullet_Grenade>();
    public Queue<Gold> queue_gold = new Queue<Gold>();

    public Queue<FarmingItem> queue_f_pistol = new Queue<FarmingItem>();
    public Queue<FarmingItem> queue_f_smg = new Queue<FarmingItem>();
    public Queue<FarmingItem> queue_f_sniper = new Queue<FarmingItem>();
    public Queue<FarmingItem> queue_f_ar = new Queue<FarmingItem>();
    public Queue<FarmingItem> queue_f_sg = new Queue<FarmingItem>();
    public Queue<FarmingItem> queue_f_grenade = new Queue<FarmingItem>();
    public Queue<FarmingItem> queue_f_hp = new Queue<FarmingItem>();
    public Queue<FarmingItem> queue_f_shield = new Queue<FarmingItem>();
    
    // for 반복용
    private int i;

    private void Init<T>(T _prefab, Queue<T> _inputQueue, string _objName, int turnNum) where T : MonoBehaviour
    {
        T temp = Instantiate(_prefab, Vector2.zero, Quaternion.identity);
        temp.transform.parent = gameObject.transform;
        temp.name = _objName + "(" + turnNum.ToString() + ")";
        _inputQueue.Enqueue(temp);
    }

    private void Awake()
    {
        instance = this;

        if (FileManager.weaponembago["pistol"])
        {
            for (i = 0; i < 5; i++)
            {
                Init(pistol, queue_pistol, "pistol", i);
                Init(farm_pistol, queue_f_pistol, "item_pistol", i);
            }
        }

        if (FileManager.weaponembago["smg"])
        {
            for (i = 0; i < 5; i++)
            {
                Init(smg, queue_smg, "smg", i);
                Init(farm_smg, queue_f_smg, "item_smg", i);
            }
        }

        if (FileManager.weaponembago["sniper"])
        {
            for (i = 0; i < 5; i++)
            {
                Init(sniper, queue_sniper, "sniper", i);
                Init(farm_sniper, queue_f_sniper, "item_sniper", i);
            }
        }

        if (FileManager.weaponembago["ar"])
        {
            for (i = 0; i < 5; i++)
            {
                Init(ar, queue_ar, "ar", i);
                Init(farm_ar, queue_f_ar, "item_ar", i);
            }
        }

        if (FileManager.weaponembago["sg"])
        {
            for (i = 0; i < 15; i++)
            {
                Init(sg, queue_sg, "sg", i);
                if (i < 5)
                    Init(farm_sg, queue_f_sg, "item_sg", i);
            }
        }

        if (FileManager.weaponembago["grenade"])
        {
            for (i = 0; i < 5; i++)
            {
                Init(grenade, queue_grenade, "grenade", i);
                Init(farm_grenade, queue_f_grenade, "item_grenade", i);
            }
        }

        for (i = 0; i < 5; i++)
        {
            Init(gold, queue_gold, "gold", i);
            Init(farm_hp, queue_f_hp, "hp", i);
            Init(farm_shield, queue_f_shield, "shield", i);
        }
    }

    public void InsertQueue<T>(T _object, Queue<T> _queue) where T : MonoBehaviour
    {
        _queue.Enqueue(_object);
        _object.gameObject.SetActive(false);
    }

    public void GetQueue_pistol(Vector2 _origin, Vector2 _direction)
    {
        if(queue_pistol.Count != 0)
        {
            _pistol = queue_pistol.Dequeue();
            _pistol.transform.position = _origin;
            _pistol.gameObject.SetActive(true);
            _pistol.Direction(_direction);
        }
    }

    public void GetQueue_smg(Vector2 _origin, Vector2 _direction)
    {
        if (queue_smg.Count != 0)
        {
            _smg = queue_smg.Dequeue();
            _smg.transform.position = _origin;
            _smg.gameObject.SetActive(true);
            _smg.Direction(_direction);
        }
    }

    public void GetQueue_sniper(Vector2 _origin, Vector2 _direction)
    {
        if (queue_sniper.Count != 0)
        {
            _sniper = queue_sniper.Dequeue();
            _sniper.transform.position = _origin;
            _sniper.gameObject.SetActive(true);
            _sniper.Direction(_direction);
        }
    }

    public void GetQueue_ar(Vector2 _origin, Vector2 _direction)
    {
        if (queue_ar.Count != 0)
        {
            _ar = queue_ar.Dequeue();
            _ar.transform.position = _origin;
            _ar.gameObject.SetActive(true);
            _ar.Direction(_direction);
        }
    }

    public void GetQueue_sg(Vector2 _origin, Vector2 _direction)
    {
        if (queue_sg.Count != 0)
        {
            _sg = queue_sg.Dequeue();
            _sg.transform.position = _origin;
            _sg.gameObject.SetActive(true);
            _sg.Direction(_direction);
        }
    }

    public void GetQueue_grenade(Vector2 _origin, Vector2 _direction)
    {
        if (queue_grenade.Count != 0)
        {
            _grenade = queue_grenade.Dequeue();
            _grenade.transform.position = _origin;
            _grenade.gameObject.SetActive(true);
            _grenade.Throw(_direction);
        }
    }

    public void GetQueue_gold(Vector2 _position)
    {
        if(queue_gold.Count != 0)
        {
            _gold = queue_gold.Dequeue();
            _gold.transform.position = _position;
            _gold.SetGoldValue(); // 골드의 범위는 Gold 스크립트에!
            _gold.gameObject.SetActive(true);
        }
    }

    private void GetQueue_ItemSpawn(Vector2 _position, FarmingPoint _point) // 함수 중복 처리
    {
        _farmItem.transform.position = _position;
        _farmItem.PutMySpawnPoint(_point);
        _farmItem.gameObject.SetActive(true);
        FarmingManager.instance.InputItem(_farmItem);
    }

    public void GetQueue_Item_pistol(Vector2 _position, FarmingPoint _point)
    {
        if (queue_f_pistol.Count != 0)
        {
            _farmItem = queue_f_pistol.Dequeue();
            GetQueue_ItemSpawn(_position, _point);
        }
    }

    public void GetQueue_Item_smg(Vector2 _position, FarmingPoint _point)
    {
        if (queue_f_smg.Count != 0)
        {
            _farmItem = queue_f_smg.Dequeue();
            GetQueue_ItemSpawn(_position, _point);
        }
    }

    public void GetQueue_Item_sniper(Vector2 _position, FarmingPoint _point)
    {
        if (queue_f_sniper.Count != 0)
        {
            _farmItem = queue_f_sniper.Dequeue();
            GetQueue_ItemSpawn(_position, _point);
        }
    }

    public void GetQueue_Item_ar(Vector2 _position, FarmingPoint _point)
    {
        if (queue_f_ar.Count != 0)
        {
            _farmItem = queue_f_ar.Dequeue();
            GetQueue_ItemSpawn(_position, _point);
        }
    }

    public void GetQueue_Item_sg(Vector2 _position, FarmingPoint _point)
    {
        if (queue_f_sg.Count != 0)
        {
            _farmItem = queue_f_sg.Dequeue();
            GetQueue_ItemSpawn(_position, _point);
        }
    }

    public void GetQueue_Item_grenade(Vector2 _position, FarmingPoint _point)
    {
        if (queue_f_grenade.Count != 0)
        {
            _farmItem = queue_f_grenade.Dequeue();
            GetQueue_ItemSpawn(_position, _point);
        }
    }

    public void GetQueue_Item_hp(Vector2 _position, FarmingPoint _point)
    {
        if (queue_f_hp.Count != 0)
        {
            _farmItem = queue_f_hp.Dequeue();
            GetQueue_ItemSpawn(_position, _point);
        }
    }

    public void GetQueue_Item_shield(Vector2 _position, FarmingPoint _point)
    {
        if (queue_f_shield.Count != 0)
        {
            _farmItem = queue_f_shield.Dequeue();
            GetQueue_ItemSpawn(_position, _point);
        }
    }
}