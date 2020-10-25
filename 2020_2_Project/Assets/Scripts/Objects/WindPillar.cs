using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindPillar : MonoBehaviour
{
    [SerializeField] private float _force = default;

    private Vector2 _forceVector = Vector2.zero;

    private void Awake()
    {
        _forceVector.y = _force;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            Player.instance.Fly(_forceVector);
    }
}
