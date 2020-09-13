using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Weapon : MonoBehaviour, IPointerDownHandler
{
    public GameObject nonUseWeaponSignal; // 총알을 다 소진했을 때 표시할 UI
    public RectTransform selectWeaponSignal; // 현재 선택한 무기를 표시할 UI

    public Text text_bulletcount;

    [SerializeField] private int bulletcount = default;
    [SerializeField] private int weaponNum = default;

    [SerializeField] private float cooltime = default;

    private bool _isShot; // 무기를 사용할 수 있는지를 판별
    private float _elapsetime; // 무기 사용 후 경과 시간

    private void Awake()
    {
        text_bulletcount.text = "(" + bulletcount.ToString() + ")";
        _elapsetime = 0f;
        _isShot = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (bulletcount == 0) // 총알이 없으면
            return; // 선택할 수 없다.
        selectWeaponSignal.transform.position = transform.position; // 무기선택 시그널을 옮겨주고
        WeaponManager.instance.SelectWeapon(weaponNum);
    }

    public bool IsShotWeapon()
    {
        if (bulletcount > 0) // 총알이 남아있는가?
        {
            if (_isShot) // 쿨타임이 경과했는가?
            {
                _isShot = false; // 쿨타임 상태로 전환한다
                return true;
            }
            else
                return false;
        }
        else
            return false;
    }

    public void MinusBulletCount()
    {
        bulletcount--;
        text_bulletcount.text = "(" + bulletcount.ToString() + ")";
        if (bulletcount == 0)
            nonUseWeaponSignal.SetActive(true);
    }

    private void Update()
    {
        if(!_isShot) // 쿨타임 중이면
        {
            _elapsetime += Time.deltaTime;
            if (_elapsetime > cooltime)
            {
                _isShot = true;
                _elapsetime = 0;
            }
        }
    }
}
