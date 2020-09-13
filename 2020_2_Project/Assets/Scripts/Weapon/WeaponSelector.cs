using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WeaponSelector : MonoBehaviour, IPointerDownHandler
{
    public GameObject nonUseWeaponSignal; // 총알을 다 소진했을 때 표시할 UI
    public RectTransform selectWeaponSignal; // 현재 선택한 무기를 표시할 UI

    public Text text_bulletcount; // 

    private int bulletcount;

    private void Awake()
    {
        text_bulletcount.text = "(" + bulletcount.ToString() + ")";
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (bulletcount == 0) // 총알이 없으면
            return; // 선택할 수 없다.
        selectWeaponSignal.transform.position = transform.position; // 무기선택 시그널을 옮겨줌
    }
}
