using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameNotice : MonoBehaviour, IPointerClickHandler
{
    private RectTransform _image;

    private Vector2 _size;

    private bool _isOpen, _isClose;

    private float _lerpValue;

    private void Awake()
    {
        _image = GetComponent<RectTransform>();
        _size = new Vector2(1273.6f, 561.8f);
    }

    private void OnEnable()
    {
        _isOpen = true;
        _isClose = false;
        Time.timeScale = 0;
    }

    private void Update()
    {
        if (_isOpen)
        {
            _lerpValue += 0.02f;
            _image.sizeDelta = Vector2.Lerp(Vector2.zero, _size, _lerpValue);
            if (_lerpValue >= 1.0f)
                _isOpen = false;
        }

        if(_isClose)
        {
            _lerpValue -= 0.02f;
            _image.sizeDelta = Vector2.Lerp(Vector2.zero, _size, _lerpValue);
            if (_lerpValue <= 0f)
                gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        _image.sizeDelta = Vector2.zero;
        Time.timeScale = 1;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!_isOpen)
            _isClose = true;
    }
}
