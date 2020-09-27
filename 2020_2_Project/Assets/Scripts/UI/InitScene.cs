using UnityEngine;
using UnityEngine.SceneManagement;

public class InitScene : MonoBehaviour
{
    private void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep; // 화면이 꺼지지 않도록 하는 코드.

        SceneManager.LoadScene("Title");
        SoundManager.instance.SetIsplayBGM(FileManager.soundSetting["bgmOn"]);
        SoundManager.instance.SetIsplaySFX(FileManager.soundSetting["sfxOn"]);
        SoundManager.instance.PlayBGM("TalesWeaver_Title");
    }
}
