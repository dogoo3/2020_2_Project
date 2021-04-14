using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameNoticePanel : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject _noticeImage = default;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!_noticeImage.activeSelf)
            _noticeImage.SetActive(true);
    }}
