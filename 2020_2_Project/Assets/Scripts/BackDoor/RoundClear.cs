using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundClear : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            WindowManager.instance.ShowClearWindow();
    }
}
