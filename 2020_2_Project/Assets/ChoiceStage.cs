using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class StageInfo
{
    public Sprite image_planet; // 행성 이미지
}

public class ChoiceStage : MonoBehaviour
{
    public StageInfo[] stageinfo;

    public Image image_planet; // 행성 이미지
    public Button lButton, rButton; // 스테이지 이동 버튼

    private int _stagelength; // 스테이지 길이
    private int _stageIndex; // 현재 선택된 스테이지

    private void Awake()
    {
        _stagelength = stageinfo.Length; // 제작 스테이지 길이 확인
        _stageIndex = 0; // 기본 지정 스테이지
    }

    public void ClickLeftButton()
    {
        image_planet.sprite = stageinfo[--_stageIndex].image_planet;
        if (_stageIndex == 0) // 현재 가리키는 스테이지가 1스테이지이면
            lButton.gameObject.SetActive(false); // 왼쪽 화살표를 비활성화시킨다
        if (_stageIndex == _stagelength - 2) // 마지막 스테이지에서 왼쪽 화살표를 누르면
            rButton.gameObject.SetActive(true); // 오른쪽 화살표를 활성화시킨다
    }

    public void ClickRightButton()
    {
        image_planet.sprite = stageinfo[++_stageIndex].image_planet;
        if (_stageIndex == _stagelength - 1) // 현재 가리키는 스테이지가 마지막 스테이지면
            rButton.gameObject.SetActive(false); // 오른쪽 화살표를 비활성화시키고
        if (_stageIndex == 1) // 첫 번째 스테이지에서 오른쪽 화살표를 누르면
            lButton.gameObject.SetActive(true); // 왼쪽 화살표를 활성화시킨다. 
    }

    public void ClickPlanet()
    {
        SceneManager.LoadScene("Stage" + (_stageIndex + 1).ToString()); // 스테이지 로딩
    }
}
