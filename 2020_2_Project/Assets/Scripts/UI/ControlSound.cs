using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ControlSound : MonoBehaviour, IPointerDownHandler
{
    [Header("Check : BGM, NonCheck = SFX")]
    public bool isType;

    private Image _image;
    private bool _isOn;

    public Sprite onSprite, offSprite;

    private void Awake()
    {
        _image = GetComponent<Image>();
        if (isType) // BGM
            Init("bgmOn");
        else // SFX
            Init("sfxOn");
    }

    private void Init(string _string)
    {
        if (FileManager.soundSetting[_string])
        {
            _isOn = true;
            _image.sprite = onSprite;
        }
        else
        {
            _isOn = false;
            _image.sprite = offSprite;
        }
    }

    private void Set(string _key)
    {
        FileManager.soundSetting[_key] = _isOn;
        SoundManager.instance.SetIsplayBGM(_isOn);
        FileManager.WriteData("DB_bool_envset.csv", FileManager.soundSetting);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _isOn = !_isOn;
        if (_isOn)
            _image.sprite = onSprite;
        else
            _image.sprite = offSprite;
        if (isType)
            Set("bgmOn");
        else
            Set("sfxOn");
    }
}
