using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponCooltimeWatcher : MonoBehaviour
{
    private Image _image;
    private float _elapsed;

    public WeaponName weaponName;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    private void Update()
    {
        if(WeaponManager.instance != null)
            _elapsed = WeaponManager.instance.GetElapseCooltime(weaponName);
        _image.fillAmount = _elapsed;
    }
}
