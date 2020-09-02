using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempStageOne : MonoBehaviour
{
    public string bgmname;

    private void Awake()
    {
        SoundManager.instance.PlayBGM(bgmname);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Application.platform == RuntimePlatform.Android)
                Application.Quit();
        }
    }
}
