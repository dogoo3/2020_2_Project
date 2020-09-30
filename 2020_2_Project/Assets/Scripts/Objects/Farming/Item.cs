using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    protected int itemId;
    protected int itemValue;

    public virtual void Supply() { }
}

public class F_Pistol : Item
{
    public F_Pistol(int _itemId, int _itemValue)
    {
        itemId = _itemId;
        itemValue = _itemValue;
    }
    public override void Supply()
    {
        WeaponManager.instance.Supply(itemId, itemValue);
    }
}
public class F_SMG : Item
{
    public F_SMG(int _itemId, int _itemValue)
    {
        itemId = _itemId;
        itemValue = _itemValue;
    }
    public override void Supply()
    {
        WeaponManager.instance.Supply(itemId, itemValue);
    }
}
public class F_Sniper : Item
{
    public F_Sniper(int _itemId, int _itemValue)
    {
        itemId = _itemId;
        itemValue = _itemValue;
    }
    public override void Supply()
    {
        WeaponManager.instance.Supply(itemId, itemValue);
    }
}
public class F_AR : Item
{
    public F_AR(int _itemId, int _itemValue)
    {
        itemId = _itemId;
        itemValue = _itemValue;
    }
    public override void Supply()
    {
        WeaponManager.instance.Supply(itemId, itemValue);
    }
}
public class F_SG : Item
{
    public F_SG(int _itemId, int _itemValue)
    {
        itemId = _itemId;
        itemValue = _itemValue;
    }
    public override void Supply()
    {
        WeaponManager.instance.Supply(itemId, itemValue);
    }
}
public class F_Grenade : Item
{
    public F_Grenade(int _itemId, int _itemValue)
    {
        itemId = _itemId;
        itemValue = _itemValue;
    }
    public override void Supply()
    {
        WeaponManager.instance.Supply(itemId, itemValue);
    }
}
public class F_HP : Item
{
    public F_HP(int _itemValue)
    {
        itemValue = _itemValue;
    }
    public override void Supply()
    {
        Player.instance.HealHP(itemValue);
    }
}
public class F_Shield : Item
{
    public F_Shield(int _itemValue)
    {
        itemValue = _itemValue;
    }
    public override void Supply()
    {
        Player.instance.HealShield(itemValue);
    }
}

