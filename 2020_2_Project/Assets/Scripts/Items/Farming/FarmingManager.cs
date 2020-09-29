using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmingManager : MonoBehaviour
{
    public static FarmingManager instance;

    [SerializeField] private List<FarmingPoint> _farmingPos;

    public List<FarmingItem> _nowactiveItem = new List<FarmingItem>(); // 현재 활성화된 아이템의 정보를 가지고 있는 List
    private Dictionary<int, int> _activeItems = new Dictionary<int, int>(); // 어떤 아이템을 스폰할지 판독하는 딕셔너리. <난수값, 아이템번호>

    private int _randFarmPos, _randItemNum;

    private void Awake()
    {
        int i = 0;
        instance = this;

        // 무기 해금여부에 따라서 스폰할 무기의 종류를 넣어줌
        if (FileManager.weaponembago["pistol"])
            _activeItems.Add(i++, 0);
        if (FileManager.weaponembago["smg"])
            _activeItems.Add(i++, 1);
        if (FileManager.weaponembago["sniper"])
            _activeItems.Add(i++, 2);
        if (FileManager.weaponembago["ar"])
            _activeItems.Add(i++, 3);
        if (FileManager.weaponembago["sg"])
            _activeItems.Add(i++, 4);
        if (FileManager.weaponembago["grenade"])
            _activeItems.Add(i++, 5);

        // HP & 쉴드 아이템(6번, 7번)
        _activeItems.Add(i++, 6);
        _activeItems.Add(i++, 7);
    }

    public void InputItem(FarmingItem _item) // 아이템 활성화 시 
    {
        _nowactiveItem.Add(_item);
    }

    public void OutputItem(FarmingItem _item) // 아이템 획득 시 
    {
        _nowactiveItem.Remove(_item);
    }

    public void ClearItem() // 라운드 종료로 인해 아이템을 비활성화하는 함수
    {
        Debug.Log(_nowactiveItem.Count);
        for (int i = 0; i < _nowactiveItem.Count; i++)
            _nowactiveItem[i].InsertQueue();
        _nowactiveItem.Clear();
    }

    public void ResetRound()
    {
        for (int i = 0; i < _nowactiveItem.Count; i++)
            _farmingPos.Add(_nowactiveItem[i].GetSpawnPoint());
    }

    public void AddPoint(FarmingPoint _point)
    {
        _farmingPos.Add(_point);
    }

    public void DeletePoint(FarmingPoint _point)
    {
        _farmingPos.Remove(_point);
    }

    public void StartItemSpawn()
    {
        if (_farmingPos.Count != 0)
        {
            _randFarmPos = Random.Range(0, _farmingPos.Count); // 어느 위치에 Spawn할건지 난수 생성
            _randItemNum = Random.Range(0, _activeItems.Count); // 어떤 Item을 Spawn할건지 난수 생성
            _farmingPos[_randFarmPos].Spawn(_activeItems[_randItemNum]); // 아이템 번호 난수값을 넣어주면서 Spawn함
        }
    }

    public void GameStart() // Animation Func
    {
        InvokeRepeating("StartItemSpawn", 6.0f, 3.0f);
    }

    public void GameEnd()
    {
        CancelInvoke("StartItemSpawn");
    }
}
