using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private Transform _arrivePos = default;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            CineCamera.instance.SetOffset(collision.transform.position, _arrivePos.position);
            collision.transform.position = _arrivePos.position;
            SoundManager.instance.PlaySFX("portal");
        }
    }
}
