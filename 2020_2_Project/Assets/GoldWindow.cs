using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldWindow : MonoBehaviour
{
    public static GoldWindow instance;

    private Text _goldText;

    private void Awake()
    {
        instance = this;
        _goldText = GetComponentInChildren<Text>();
    }

    public void UpdateText(int _nowGetGold)
    {
        _goldText.text = _nowGetGold.ToString();
    }
}
