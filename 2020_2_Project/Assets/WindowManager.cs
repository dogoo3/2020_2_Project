using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoBehaviour
{
    public static WindowManager instance;

    [SerializeField] private ClearWindow clearWindow;
    [SerializeField] private FailWindow failWindow;
    [SerializeField] private AllClearWindow allclearWindow;

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
    }

    public void ShowFailWindow()
    {
        failWindow.Init();
    }
}
