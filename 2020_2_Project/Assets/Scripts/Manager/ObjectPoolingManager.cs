using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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

    // 임시 오브젝트
    private Bullet_Pistol _pistol;
    private Bullet_SMG _smg;
    private Bullet_Sniper _sniper;
    private Bullet_AR _ar;
    private Bullet_SG _sg;
    private Bullet_Grenade _grenade;
    private Gold _gold;

    // 저장 큐
    private Queue<Bullet_Pistol> queue_pistol = new Queue<Bullet_Pistol>();
    private Queue<Bullet_SMG> queue_smg = new Queue<Bullet_SMG>();
    private Queue<Bullet_Sniper> queue_sniper = new Queue<Bullet_Sniper>();
    private Queue<Bullet_AR> queue_ar = new Queue<Bullet_AR>();
    private Queue<Bullet_SG> queue_sg = new Queue<Bullet_SG>();
    private Queue<Bullet_Grenade> queue_grenade = new Queue<Bullet_Grenade>();
    private Queue<Gold> queue_gold = new Queue<Gold>();

    private void Awake()
    {
        instance = this;

        for (int i = 0; i < 15; i++)
        {
            _pistol = Instantiate(pistol, Vector2.zero, Quaternion.identity);
            _pistol.transform.parent = gameObject.transform;
            _pistol.name = "pistol" + "(" + i.ToString() + ")";
            queue_pistol.Enqueue(_pistol);
            _pistol.gameObject.SetActive(false);

            _smg = Instantiate(smg, Vector2.zero, Quaternion.identity);
            _smg.transform.parent = gameObject.transform;
            _smg.name = "smg" + "(" + i.ToString() + ")";
            queue_smg.Enqueue(_smg);
            _smg.gameObject.SetActive(false);

            _sniper = Instantiate(sniper, Vector2.zero, Quaternion.identity);
            _sniper.transform.parent = gameObject.transform;
            _sniper.name = "sniper" + "(" + i.ToString() + ")";
            queue_sniper.Enqueue(_sniper);
            _sniper.gameObject.SetActive(false);

            _ar = Instantiate(ar, Vector2.zero, Quaternion.identity);
            _ar.transform.parent = gameObject.transform;
            _ar.name = "ar" + "(" + i.ToString() + ")";
            queue_ar.Enqueue(_ar);
            _ar.gameObject.SetActive(false);

            _sg = Instantiate(sg, Vector2.zero, Quaternion.identity);
            _sg.transform.parent = gameObject.transform;
            _sg.name = "sg" + "(" + i.ToString() + ")";
            queue_sg.Enqueue(_sg);
            _sg.gameObject.SetActive(false);

            _grenade = Instantiate(grenade, Vector2.zero, Quaternion.identity);
            _grenade.transform.parent = gameObject.transform;
            _grenade.name = "grenade" + "(" + i.ToString() + ")";
            queue_grenade.Enqueue(_grenade);
            _grenade.gameObject.SetActive(false);

            _gold = Instantiate(gold, Vector2.zero, Quaternion.identity);
            _gold.transform.parent = gameObject.transform;
            _gold.name = "gold" + "(" + i.ToString() + ")";
            queue_gold.Enqueue(_gold);
            _gold.gameObject.SetActive(false);
        }
    }

    #region pistol
    public void InsertQueue_pistol(Bullet_Pistol _object)
    {
        queue_pistol.Enqueue(_object);
        _object.gameObject.SetActive(false);
    }
    public void GetQueue_pistol(Vector2 _origin, Vector2 _direction)
    {
        if(queue_pistol.Count != 0)
        {
            _pistol = queue_pistol.Dequeue();
            _pistol.transform.position = _origin;
            _pistol.Direction(_direction);
            _pistol.gameObject.SetActive(true);
        }
    }
    #endregion

    #region smg
    public void InsertQueue_smg(Bullet_SMG _object)
    {
        queue_smg.Enqueue(_object);
        _object.gameObject.SetActive(false);
    }
    public void GetQueue_smg(Vector2 _origin, Vector2 _direction)
    {
        if (queue_smg.Count != 0)
        {
            _smg = queue_smg.Dequeue();
            _smg.transform.position = _origin;
            _smg.Direction(_direction);
            _smg.gameObject.SetActive(true);
        }
    }
    #endregion

    #region sniper
    public void InsertQueue_sniper(Bullet_Sniper _object)
    {
        queue_sniper.Enqueue(_object);
        _object.gameObject.SetActive(false);
    }
    public void GetQueue_sniper(Vector2 _origin, Vector2 _direction)
    {
        if (queue_sniper.Count != 0)
        {
            _sniper = queue_sniper.Dequeue();
            _sniper.transform.position = _origin;
            _sniper.Direction(_direction);
            _sniper.gameObject.SetActive(true);
        }
    }
    #endregion

    #region ar
    public void InsertQueue_ar(Bullet_AR _object)
    {
        queue_ar.Enqueue(_object);
        _object.gameObject.SetActive(false);
    }
    public void GetQueue_ar(Vector2 _origin, Vector2 _direction)
    {
        if (queue_ar.Count != 0)
        {
            _ar = queue_ar.Dequeue();
            _ar.transform.position = _origin;
            _ar.Direction(_direction);
            _ar.gameObject.SetActive(true);
        }
    }
    #endregion

    #region sg
    public void InsertQueue_sg(Bullet_SG _object)
    {
        queue_sg.Enqueue(_object);
        _object.gameObject.SetActive(false);
    }
    public void GetQueue_sg(Vector2 _origin, Vector2 _direction)
    {
        if (queue_sg.Count != 0)
        {
            _sg = queue_sg.Dequeue();
            _sg.transform.position = _origin;
            _sg.Direction(_direction);
            _sg.gameObject.SetActive(true);
        }
    }
    #endregion

    #region grenade
    public void InsertQueue_grenade(Bullet_Grenade _object)
    {
        queue_grenade.Enqueue(_object);
        _object.gameObject.SetActive(false);
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
    #endregion

    #region gold
    public void InsertQueue_gold(Gold _object)
    {
        queue_gold.Enqueue(_object);
        _object.gameObject.SetActive(false);
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
    #endregion
}
