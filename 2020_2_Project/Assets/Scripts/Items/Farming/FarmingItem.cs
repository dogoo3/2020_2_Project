﻿using UnityEngine;
using System.Collections;

public enum ItemKind
{
    pistol,
    smg,
    sniper,
    ar,
    sg,
    grenade,
    hp,
    shield
}

public class FarmingItem : MonoBehaviour
{
    [Header("아이템 속성 반드시 잡아줘야 함!")]
    public ItemKind itemKind;

    private Rigidbody2D _rigidbody2d;
    private Collider2D _collider2d;

    private Item _item;
    public FarmingPoint _mySpawnPoint;

    private int _itemId;
    public int supplyValue;

    private void Awake()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _collider2d = GetComponent<Collider2D>();
        switch (itemKind)
        {
            case ItemKind.pistol:
                _itemId = 0;
                _item = new F_Pistol(_itemId, supplyValue);
                break;
            case ItemKind.smg:
                _itemId = 1;
                _item = new F_SMG(_itemId, supplyValue);
                break;
            case ItemKind.sniper:
                _itemId = 2;
                _item = new F_Sniper(_itemId, supplyValue);
                break;
            case ItemKind.ar:
                _itemId = 3;
                _item = new F_AR(_itemId, supplyValue);
                break;
            case ItemKind.sg:
                _itemId = 4;
                _item = new F_SG(_itemId, supplyValue);
                break;
            case ItemKind.grenade:
                _itemId = 5;
                _item = new F_Grenade(_itemId, supplyValue);
                break;
            case ItemKind.hp:
                _item = new F_HP(supplyValue);
                break;
            case ItemKind.shield:
                _item = new F_Shield(supplyValue);
                break;
        }
    }

    private void OnEnable()
    {
        // Active 시 멋지게(?) 등장하기 위한 모션
        _rigidbody2d.velocity = new Vector2(0, 4.0f);

        Invoke("EnableCollider", 1.0f); // 1초뒤 활성화
        _collider2d.enabled = false; // 콜라이더 꺼 줌
    }

    public void InsertQueue()
    {
        switch(itemKind)
        {
            case ItemKind.pistol:
                ObjectPoolingManager.instance.InsertQueue(this, ObjectPoolingManager.instance.queue_f_pistol);
                break;
            case ItemKind.smg:
                ObjectPoolingManager.instance.InsertQueue(this, ObjectPoolingManager.instance.queue_f_smg);
                break;
            case ItemKind.sniper:
                ObjectPoolingManager.instance.InsertQueue(this, ObjectPoolingManager.instance.queue_f_sniper);
                break;
            case ItemKind.ar:
                ObjectPoolingManager.instance.InsertQueue(this, ObjectPoolingManager.instance.queue_f_ar);
                break;
            case ItemKind.sg:
                ObjectPoolingManager.instance.InsertQueue(this, ObjectPoolingManager.instance.queue_f_sg);
                break;
            case ItemKind.grenade:
                ObjectPoolingManager.instance.InsertQueue(this, ObjectPoolingManager.instance.queue_f_grenade);
                break;
            case ItemKind.hp:
                ObjectPoolingManager.instance.InsertQueue(this, ObjectPoolingManager.instance.queue_f_hp);
                break;
            case ItemKind.shield:
                ObjectPoolingManager.instance.InsertQueue(this, ObjectPoolingManager.instance.queue_f_shield);
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            _mySpawnPoint.GetItem();
            _item.Supply(); // 아이템 공급
            InsertQueue();
            _collider2d.enabled = false;
            FarmingManager.instance.OutputItem(this);
        }
    }

    private void EnableCollider() // Invoke Func
    {
        _collider2d.enabled = true;
    }

    public void PutMySpawnPoint(FarmingPoint _point) // 파라미터의 point에서 나온 아이템이 살아있는지 죽었는지를 판별하기 위해서 넣어줌.
    {
        _mySpawnPoint = _point;
    }

    public FarmingPoint GetSpawnPoint()
    {
        return _mySpawnPoint;
    }
}
