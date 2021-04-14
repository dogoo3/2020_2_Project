using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;

    [SerializeField] private float[] _second = default;

    private TimeManager _myScript;
    private Text _text;

    private Color _color;
    private int _min;

    private bool _start;

    private float _sec;
    private float _remainder;

    private string _minstr;
    private string _secstr;
    private string _milsecstr;

    private void Awake()
    {
        instance = this;

        _text = GetComponent<Text>();
        _myScript = GetComponent<TimeManager>();
    }

    private void Update()
    {
        if(_start)
        {
            _secstr = _sec >= 10.0f ? ((int)_sec).ToString() : "0" + ((int)_sec).ToString();
            _milsecstr = ((int)((_sec - (int)_sec) * 100)) >= 10 ? ((int)((_sec - (int)_sec) * 100)).ToString() : "0" + ((int)((_sec - (int)_sec) * 100));

            _text.text = _minstr + ":" + _secstr + ":" + _milsecstr;
            _sec -= Time.deltaTime;

            if (_sec <= 0f)
            {
                if (_min == 0)
                {
                    Player.instance.AlwaysDead();
                    SetTimer(false);
                }
                else
                {
                    _min--;
                    _sec += 60.0f;
                    _minstr = _min >= 10 ? _min.ToString() : "0" + _min.ToString();
                }
            }

            // Changing Color
            if(_min == 0)
            {
                if(_sec <= 30.0f && _sec >= 10.0f)
                {
                    _color.g = Mathf.Clamp(_color.g - 0.001f, 0, 255);                
                    _color.b = Mathf.Clamp(_color.b - 0.001f, 0, 255);
                    _text.color = _color;
                }
            }
        }
    }

    public void Init(int _round)
    {
        _color = Color.white;
        _text.color = _color;

        _min = (int)_second[_round] / 60;
        _sec = _second[_round] - _min * 60.0f;

        _minstr = _min >= 10 ? _min.ToString() : "0" + _min.ToString();
    }

    public void SetTimer(bool _is)
    {
        _start = _is;
    }

    public void DeleteText()
    {
        _text.text= "";
    }
}
