using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WeaponSelector : MonoBehaviour, IPointerDownHandler
{
    public GameObject nonUseWeaponSignal; // 총알을 다 소진했을 때 표시할 UI
    public RectTransform selectWeaponSignal; // 현재 선택한 무기를 표시할 UI

    public int weaponNum;

    private bool _isExhaustion; // 무기를 모두 사용했는지 여부, true면 다 사용, false면 아직 남음.

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!_isExhaustion)
        {
            WeaponManager.instance.ChangeSelectWeapon((WeaponName)weaponNum);
            selectWeaponSignal.transform.position = transform.position; // 무기선택 시그널을 옮겨줌
        }
    }

    public void Exhaust(bool _is = true)
    {
        nonUseWeaponSignal.SetActive(_is);
        _isExhaustion = _is;
    }
}
