using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoBehaviour
{
    public static WindowManager instance;

    [SerializeField] private GameOverWindow clearWindow = default;
    [SerializeField] private GameOverWindow failWindow = default;
    [SerializeField] private GameOverWindow allclearWindow = default;

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