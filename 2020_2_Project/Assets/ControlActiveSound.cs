using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlActiveSound : MonoBehaviour
{
    public Toggle bgmOn;
    public Toggle bgmOff;
    public Toggle sfxOn;
    public Toggle sfxOff;

    private void Awake()
    {
        if(FileManager.soundSetting["bgmOn"])
        {
            bgmOn.isOn = true;
            bgmOff.isOn = false;
        }
        else
        {
            bgmOn.isOn = false;
            bgmOff.isOn = true;
        }

        if(FileManager.soundSetting["sfxOn"])
        {
            sfxOn.isOn = true;
            sfxOff.isOn = false;
        }
        else
        {
            sfxOn.isOn = false;
            sfxOff.isOn = true;
        }
    }

    private void SwitchBGM(bool _is)
    {
        FileManager.soundSetting["bgmOn"] = _is;
        SoundManager.instance.SetIsplayBGM(_is);
        FileManager.WriteData("DB_bool.csv", FileManager.soundSetting); // 불필요한 File 쓰기 방지
    }
    private void SwitchSFX(bool _is)
    {
        FileManager.soundSetting["sfxOn"] = _is;
        SoundManager.instance.SetIsplaySFX(_is);
        FileManager.WriteData("DB_bool.csv", FileManager.soundSetting);
    }

    public void SwitchingBGM(bool _is)
    {
        if(!_is) // BGM ON Toggle을 터치했을 때
            if (bgmOn.isOn) // 중복 터치를 방지 
                SwitchBGM(true);
        else // BGM OFF Toggle을 터치했을 때
            if (bgmOff.isOn) // 중복 터치를 방지
                SwitchBGM(false);
    }

    public void SwitchingSFX(bool _is)
    {
        if(!_is) // SFX ON Toggle을 눌렀을 때
            if (sfxOn.isOn) SwitchSFX(true);
        else // SFX OFF Toggle을 눌렀을 때
            if (sfxOff.isOn) SwitchSFX(false);
    }
}
