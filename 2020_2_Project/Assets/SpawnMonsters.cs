using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMonsters : MonoBehaviour
{
    [Header("0도 : (0,1)방향, 시계반대방향으로 돌아감.")]
    [Range(0, 360)]
    public int shotAngle;

    private Vector2 _shotDirection;

    private void Start()
    {
        _shotDirection.x = Mathf.Cos(shotAngle * Mathf.Deg2Rad);
        _shotDirection.y = Mathf.Sin(shotAngle * Mathf.Deg2Rad);
        Debug.Log(_shotDirection);
    }
}

