using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlEscape : MonoBehaviour
{
    public GameObject selectStageWindow; // 게임시작 윈도우
    public GameObject envSettingWindow; // 환경설정 윈도우
    public GameObject shopWindow; // 상점 윈도우

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                if (envSettingWindow.activeSelf) // 환경설정 창이 열려있는 경우
                    envSettingWindow.SetActive(false);
                else if (shopWindow.activeSelf) // 상점 창이 열려있는 경우
                    shopWindow.SetActive(false);
                else if (selectStageWindow.activeSelf) // 스테이지 선택 창이 열려있는 경우
                    selectStageWindow.SetActive(false);
                else
                    Application.Quit();
            }
        }
    }

    public void ActiveSelectStageWindow()
    {
        selectStageWindow.SetActive(true);
    }

    public void ActiveEnvSettingWindow(bool _isActive = true)
    {
        envSettingWindow.SetActive(_isActive);
    }

    public void ActiveShopWindow(bool _isActive)
    {
        shopWindow.SetActive(_isActive);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
