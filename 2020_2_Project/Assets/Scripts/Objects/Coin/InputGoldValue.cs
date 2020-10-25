using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputGoldValue : MonoBehaviour
{
    [SerializeField] private int _value = default;

    private void Awake()
    {
        gameObject.GetComponent<Gold>().SetGoldValue(_value);
    }
}
