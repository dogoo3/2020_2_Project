using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolItem : MonoBehaviour
{
    private FarmingItem _farmingitem;
    private float _elapsetime; // 경과시간
    private bool _isPatrol; // 지면에 닿으면 둥둥 떠다니기 시작
    private Vector2 _underPos, _topPos; // 보간 최대좌표

    private void Awake()
    {
        _farmingitem = GetComponentInParent<FarmingItem>();
    }
    private void OnEnable()
    {
        // 시간 경과 변수 초기화
        _elapsetime = 0;
        // 방향 설정(시작은 아래 -> 위)
        _isPatrol = false;
    }

    private void Update()
    {
        if (_isPatrol)
        {
            _elapsetime += Time.deltaTime;
            transform.parent.position = Vector2.Lerp(_underPos, _topPos, Mathf.Abs(Mathf.Sin(_elapsetime++ * Mathf.Deg2Rad)));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            _isPatrol = true;

            // 고정 보간좌표 설정
            _underPos = transform.parent.position;
            _topPos = _underPos;
            _topPos.y = _underPos.y + 0.23f;
            _farmingitem.SetGround();
        }
    }
}
