using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlEscape : MonoBehaviour
{
    public GameObject selectStageWindow; // 게임시작 윈도우
    public GameObject envSettingWindow; // 환경설정 윈도우
    public GameObject shopWindow; // 상점 윈도우
    public GameObject shop_UnLockWeaponWindow; // 상점 윈도우의 무기 획득 윈도우
    public GameObject shop_UpgradeStatsWindow; // 상점 윈도우의 능력치 업그레이드 윈도우
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                if (shop_UpgradeStatsWindow.activeSelf) // 업글 윈도우 창이 열려있는 경우
                    shop_UpgradeStatsWindow.SetActive(false);
                else if (shop_UnLockWeaponWindow.activeSelf) // 무기 해금 윈도우 창이 열려있는 경우 
                    shop_UnLockWeaponWindow.SetActive(false);
                else
                {
                    if (envSettingWindow.activeSelf) // 환경설정 창이 열려있는 경우
                        envSettingWindow.SetActive(false);
                    else if (shopWindow.activeSelf) // 상점 창이 열려있는 경우
                    {
                        SoundManager.instance.PlayBGM("title");
                        shopWindow.SetActive(false);
                    }
                    else if (selectStageWindow.activeSelf) // 스테이지 선택 창이 열려있는 경우
                    {
                        SoundManager.instance.PlayBGM("title");
                        selectStageWindow.SetActive(false);
                    }
                    else
                        Application.Quit();
                }
            }
        }
    }

    public void ActiveSelectStageWindow()
    {
        SoundManager.instance.PlayBGM("stageselect");
        selectStageWindow.SetActive(true);
    }

    public void ActiveEnvSettingWindow(bool _isActive = true)
    {
        envSettingWindow.SetActive(_isActive);
    }

    public void ActiveShopWindow(bool _isActive)
    {
        if(_isActive)
            SoundManager.instance.PlayBGM("shop");
        else
            SoundManager.instance.PlayBGM("title");
        shopWindow.SetActive(_isActive);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
