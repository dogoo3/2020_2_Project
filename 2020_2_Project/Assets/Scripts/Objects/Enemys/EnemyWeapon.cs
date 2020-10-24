using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    [Header("좀비보스, 고블린보스의 칼과 같이 무언가로 플레이어를 타격할 때 그 무언가를 따라갈 콜라이더. 부모의 적 스크립트에서 컴포넌트하여 이 스크립트에 값을 입력해줌.")]
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
