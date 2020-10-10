using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoBehaviour
{
    public static WindowManager instance;

    [SerializeField] private GameOverWindow clearWindow = default;
    [SerializeField] private GameOverWindow failWindow = default;
    [SerializeField] private GameOverWindow allclearWindow = default;
    [SerializeField] private EnvsetWindow envsetWindow = default;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                if (envsetWindow.gameObject.activeSelf)
                    envsetWindow.HideEnvsetWindow();
                else
                    envsetWindow.ShowEnvsetWindow();
            }
        }
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