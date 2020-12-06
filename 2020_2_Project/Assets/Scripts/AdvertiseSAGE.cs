using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdvertiseSAGE : MonoBehaviour
{
    public string nextscene;
    public void Advertise_SAGE()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep; // 화면이 꺼지지 않도록 하는 코드.

        SoundManager.instance.SetIsplayBGM(FileManager.soundSetting["bgmOn"]);
        SoundManager.instance.SetIsplaySFX(FileManager.soundSetting["sfxOn"]);
        SoundManager.instance.PlayBGM("title");
        SceneManager.LoadScene(nextscene);
    }
}
