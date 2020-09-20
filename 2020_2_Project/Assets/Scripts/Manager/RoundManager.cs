using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public static RoundManager instance;

    public string bgmname;

    private Animator _animator;

    public int nowRound;

    private void Awake()
    {
        instance = this;

        _animator = GetComponent<Animator>();
        StopBGM();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Application.platform == RuntimePlatform.Android)
                Application.Quit();
        }
    }

    public void StopBGM() // Animation Func
    {
        SoundManager.instance.StopBGM();
    }

    public void GameStart() // Animator Func
    {
        SoundManager.instance.PlayBGM(bgmname);
        SpawnMonstersManager.instance.GameStart();
    }

    public void PlayBeepSound() // Animator Func
    {
        SoundManager.instance.PlaySFX("beepsound");
    }

    public void EnableAnimator() // Animation Func
    {
        _animator.Play("countdown", -1, 0);
    }
}
