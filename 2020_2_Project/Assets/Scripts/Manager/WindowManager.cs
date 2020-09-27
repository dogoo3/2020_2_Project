using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoBehaviour
{
    public static WindowManager instance;

    [SerializeField] private GameOverWindow clearWindow;
    [SerializeField] private GameOverWindow failWindow;
    [SerializeField] private GameOverWindow allclearWindow;

    private void Awake()
    {
        instance = this;
    }

    public void ShowClearWindow()
    {
        if (RoundManager.instance.nowRound == 2)
            allclearWindow.Init();
        else
            clearWindow.Init();
        FarmingManager.instance.GameEnd();
    }

    public void ShowFailWindow()
    {
        failWindow.Init();
        FarmingManager.instance.GameEnd();
    }
}