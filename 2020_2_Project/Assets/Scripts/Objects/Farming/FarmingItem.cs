using UnityEngine;
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
    private SpriteRenderer _spriterenderer;
    private Item _item;
    private Color _color;

    public FarmingPoint _mySpawnPoint;

    private int _itemId;
    public int supplyValue;
    private bool _isGet;

    private void Awake()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _collider2d = GetComponent<Collider2D>();
        _spriterenderer = GetComponent<SpriteRenderer>();

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
        _rigidbody2d.gravityScale = 1;
        _rigidbody2d.velocity = new Vector2(0, 4.0f);
        _spriterenderer.color = Color.white;
        _color = Color.white;
        Invoke("EnableComponent", 1.0f); // 1초뒤 활성화
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
            _collider2d.enabled = false;
            _isGet = true;
            // 오오라존여부에 따라 어느 리스트로 들어가야 하는지를 분기한다.
            // 아이템 획득 시 활성화 아이템이 들어있는 배열에서 삭제해주는 함수임.
            if (_mySpawnPoint.isauraZone)
                FarmingManager.instance.OutputAuraItem(this);
            else
                FarmingManager.instance.OutputItem(this);
        }
    }

    private void Update()
    {
        if (_isGet)
        {
            _color.a -= 0.032f;
            _spriterenderer.color = _color;
            if (_color.a <= 0)
            {
                _isGet = false;
                InsertQueue();
            }
        }
    }

    private void EnableComponent() // Invoke Func
    {
        _collider2d.enabled = true;
    }

    public void PutMySpawnPoint(FarmingPoint _point) // 파라미터의 point에서 나온 아이템이 살아있는지 죽었는지를 판별하기 위해서 넣어줌.
    {
        _mySpawnPoint = _point;
    }

    public FarmingPoint GetSpawnPoint() // 라운드를 종료할 때 아이템들을 모두 없애주는 함수이다.
    {
        return _mySpawnPoint;
    }

    public void SetGround() // 자식 스크립트(PatrolItem)를 위한 함수
    {
        _rigidbody2d.gravityScale = 0;
    }
}
