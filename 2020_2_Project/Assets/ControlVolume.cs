using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlVolume : MonoBehaviour
{
    private Slider _volumeController;

    private void Awake()
    {
        _volumeController = GetComponent<Slider>();
    }

    public void ControlSoundVolume()
    {
        SoundManager.instance.bgmPlayer.volume = _volumeController.value;
        for (int i = 0; i < SoundManager.instance.sfxPlayer.Length; i++)
             SoundManager.instance.sfxPlayer[i].volume = _volumeController.value;
    }
}
