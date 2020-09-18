using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountdownSound : MonoBehaviour
{
    public void PlayBeepSound()
    {
        SoundManager.instance.PlaySFX("beepsound");
    }
}
