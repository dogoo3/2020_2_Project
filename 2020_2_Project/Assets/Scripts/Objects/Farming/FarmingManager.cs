using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmingManager : MonoBehaviour
{
    public static FarmingManager instance;

    [SerializeField] private List<FarmingPoint> _farmingPos = default; // 랜덤 생성 위치
    [SerializeField] private List<FarmingPoint> _auraFarmingPos = default; // 고정 생성 위치 

    private List<FarmingItem> _nowactiveItem = new List<FarmingItem>(); // 현재 활성화된 아이템의 정보를 가지고 있는 List
    private List<FarmingItem> _nowAuraActiveItem = new List<FarmingItem>(); // 고정생성위치에 활성화된 아이템의 정보를 가지고 있는 List

    private Dictionary<int, int> _activeItems = new Dictionary<int, int>(); // 어떤 아이템을 스폰할지 판독하는 딕셔너리. <난수값, 아이템번호>

    private int _randFarmPos, _randItemNum;

    [Header("게임 시작 후 몇 초 후에 아이템을 스폰하게 할 건가?")]
    [SerializeField] private float _startItemSpawnTime = default;
    [Header("몇 초에 한번씩 아이템을 스폰하게 할 건가?")]
    [SerializeField] private float _repeatSpawnTime = default;
    private void Awake()
    {
        int i = 0;
        instance = this;

        // 무기 해금여부에 따라서 스폰할 무기의 종류를 넣어줌
        if (FileManager.weaponembargo["pistol"])
            _activeItems.Add(i++, 0);
        if (FileManager.weaponembargo["smg"])
            _activeItems.Add(i++, 1);
        if (FileManager.weaponembargo["sniper"])
            _activeItems.Add(i++, 2);
        if (FileManager.weaponembargo["ar"])
            _activeItems.Add(i++, 3);
        if (FileManager.weaponembargo["sg"])
            _activeItems.Add(i++, 4);
        if (FileManager.weaponembargo["grenade"])
            _activeItems.Add(i++, 5);

        // HP & 쉴드 아이템(6번, 7번)
        _activeItems.Add(i++, 6);
        _activeItems.Add(i++, 7);
    }

    #region GetItemInput&Output
    public void InputItem(FarmingItem _item) // 아이템 활성화 시 
    {
        _nowactiveItem.Add(_item);
    }

    public void OutputItem(FarmingItem _item) // 아이템 획득 시 
    {
        _nowactiveItem.Remove(_item);
    }

    public void InputAuraItem(FarmingItem _item) // 고정 아이템 활성화 시
    {
        _nowAuraActiveItem.Add(_item);
    }

    public void OutputAuraItem(FarmingItem _item) // 고정 아이템 획득 시 
    {
        _nowAuraActiveItem.Remove(_item);
    }
    #endregion

    public void ClearItem() // 라운드 종료로 인해 아이템을 비활성화하는 함수
    {
        for (int i = 0; i < _nowactiveItem.Count; i++)
        {
            _nowactiveItem[i].InsertQueue();
            _farmingPos.Add(_nowactiveItem[i].GetSpawnPoint());
        }
        for (int i = 0; i < _nowAuraActiveItem.Count; i++)
        {
            _nowAuraActiveItem[i].InsertQueue();
            _auraFarmingPos.Add(_nowAuraActiveItem[i].GetSpawnPoint());
        }
        _nowactiveItem.Clear();
        _nowAuraActiveItem.Clear();
    }

    #region FarmingPointAdd&Delete
    public void AddPoint(FarmingPoint _point)
    {
        _farmingPos.Add(_point);
    }

    public void DeletePoint(FarmingPoint _point)
    {
        _farmingPos.Remove(_point);
    }

    public void AddAuraPoint(FarmingPoint _point)
    {
        _auraFarmingPos.Add(_point);
    }

    public void DeleteAuraPoint(FarmingPoint _point)
    {
        _auraFarmingPos.Remove(_point);
    }
    #endregion

    public void StartItemSpawn()
    {
        if (_farmingPos.Count != 0)
        { 
            // 가변
            _randFarmPos = Random.Range(0, _farmingPos.Count); // 어느 위치에 Spawn할건지 난수 생성
            _randItemNum = Random.Range(0, _activeItems.Count); // 어떤 Item을 Spawn할건지 난수 생성
            _farmingPos[_randFarmPos].Spawn(_activeItems[_randItemNum]); // 아이템 번호 난수값을 넣어주면서 Spawn함
        }

        // 고정(오오라)
        while (_auraFarmingPos.Count != 0)
        {
            _randFarmPos = Random.Range(0, _auraFarmingPos.Count); // 어느 위치에 Spawn할건지 난수 생성
            _randItemNum = Random.Range(0, _activeItems.Count); // 어떤 Item을 Spawn할건지 난수 생성
            _auraFarmingPos[_randFarmPos].Spawn(_activeItems[_randItemNum]);
        }
    }

    public void GameStart() // Animation Func
    {
        InvokeRepeating("StartItemSpawn", _startItemSpawnTime, _repeatSpawnTime);
    }

    public void GameEnd()
    {
        CancelInvoke("StartItemSpawn");
    }
}
