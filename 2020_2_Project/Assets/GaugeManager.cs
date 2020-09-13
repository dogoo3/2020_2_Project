using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GaugeManager : MonoBehaviour
{
    public static GaugeManager instance;
    public Image gauge_HP;
    public Image gauge_Shield;

    private float _maxHP;
    private float _maxShield;

    private void Awake()
    {
        instance = this;
    }

    public void InitMaxValue(float _hp, float _shield)
    {
        
        _maxHP = 1 / _hp;
        _maxShield = 1 / _shield;
    }

    public void SetHpGauge(float _hp)
    {
        gauge_HP.fillAmount = _hp * _maxHP;
    }

    public void SetShieldGauge(float _shield)
    {
        gauge_Shield.fillAmount = _shield * _maxShield;
    }
}
