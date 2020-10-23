using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    private float _damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player.instance.Attacked(_damage);
    }

    public void InputDamage(float _damage)
    {
        this._damage = _damage;
    }
}
