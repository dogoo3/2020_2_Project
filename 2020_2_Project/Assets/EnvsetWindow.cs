using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnvsetWindow : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                if (gameObject.activeSelf)
                    HideEnvsetWindow();
                else
                    ShowEnvsetWindow();
            }
        }
    }

    public void ShowEnvsetWindow()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    public void HideEnvsetWindow()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void HideEnvsetWindow_Notime()
    {
        gameObject.SetActive(false);
    }

    public void ShowEnvsetWindow_Notime()
    {
        gameObject.SetActive(true);
    }

    public void ExitStage()
    {
        SoundManager.instance.PlayBGM("TalesWeaver_Title");

        // 할당 해제
        WeaponManager.instance = null;
        ScoreManager.instance = null;
        RoundManager.instance = null;
        FarmingManager.instance = null;
        GoldManager.instance = null;
        Player.instance = null;
        GaugeManager.instance = null;
        ObjectPoolingManager.instance = null;
        SpawnMonstersManager.instance = null;
        WindowManager.instance = null;

        SceneManager.LoadScene("Title");
        Time.timeScale = 1;
    }
}
