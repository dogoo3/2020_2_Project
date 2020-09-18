using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public string bgmname;

    public SpawnMonsters[] spawnMonsters;

    private bool _gameStart = false;
    private bool _gameEnd = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Application.platform == RuntimePlatform.Android)
                Application.Quit();
        }
    }

    public void GameStart()
    {
        SoundManager.instance.PlayBGM(bgmname);
    }

    public void PlayBeepSound()
    {
        SoundManager.instance.PlaySFX("beepsound");
    }
}
