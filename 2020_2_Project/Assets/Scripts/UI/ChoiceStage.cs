using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class StageInfo
{
    private bool isClear; // 스테이지 클리어 여부
    public Sprite image_planet; // 행성 이미지

    public void SetIsClear(bool _is)
    {
        isClear = _is;
    }

    public bool GetIsClear()
    {
        return isClear;
    }
}

public class ChoiceStage : MonoBehaviour
{
    public StageInfo[] stageinfo;

    public Image image_planet; // 행성 이미지
    public Button lButton, rButton; // 스테이지 이동 버튼
    public GameObject clearImage; // 스테이지 클리어 여부를 나타내는 이미지.

    private int _stagelength; // 스테이지 길이
    private int _stageIndex; // 현재 선택된 스테이지

    private void Awake()
    {
        _stagelength = stageinfo.Length; // 제작 스테이지 길이 확인
        _stageIndex = 0; // 기본 지정 스테이지
    }

    private void OnEnable()
    {
        int i = 0;
        foreach(KeyValuePair<string,bool> items in FileManager.stageClear)
        {
            if(i<stageinfo.Length)
                stageinfo[i].SetIsClear(items.Value);
            i++;
        }
        if (stageinfo[_stageIndex].GetIsClear())
            clearImage.SetActive(true);
    }

    public void ClickLeftButton()
    {
        image_planet.sprite = stageinfo[--_stageIndex].image_planet;
        if (_stageIndex == 0) // 현재 가리키는 스테이지가 1스테이지이면
            lButton.gameObject.SetActive(false); // 왼쪽 화살표를 비활성화시킨다
        if (_stageIndex == _stagelength - 2) // 마지막 스테이지에서 왼쪽 화살표를 누르면
            rButton.gameObject.SetActive(true); // 오른쪽 화살표를 활성화시킨다

        if (stageinfo[_stageIndex].GetIsClear())
            clearImage.SetActive(true);
        else
            clearImage.SetActive(false);
    }

    public void ClickRightButton()
    {
        image_planet.sprite = stageinfo[++_stageIndex].image_planet;
        if (_stageIndex == _stagelength - 1) // 현재 가리키는 스테이지가 마지막 스테이지면
            rButton.gameObject.SetActive(false); // 오른쪽 화살표를 비활성화시키고
        if (_stageIndex == 1) // 첫 번째 스테이지에서 오른쪽 화살표를 누르면
            lButton.gameObject.SetActive(true); // 왼쪽 화살표를 활성화시킨다. 

        if (stageinfo[_stageIndex].GetIsClear())
            clearImage.SetActive(true);
        else
            clearImage.SetActive(false);
    }

    public void ClickPlanet()
    {
        SceneManager.LoadScene("Stage" + (_stageIndex + 1).ToString()); // 스테이지 로딩
    }
}
