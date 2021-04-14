using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameNoticePanel : MonoBehaviour
{
    [SerializeField] private GameObject _noticeImage = default;

    private void OnMouseDown()
    {
        if(!_noticeImage.gameObject.activeSelf)
            _noticeImage.gameObject.SetActive(true);
    }
}
