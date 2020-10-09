using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    private Animator _animator;

    private RaycastHit2D _ray2D;
    private Vector3 _vectordir;

    private bool _isdetect; // 플레이어를 감지했을 때
    private bool _isSuicide; // 플레이어와 붙어 자살할 때

    private float _changedirTime; // 시점 변환 시간

    public int damage;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _vectordir = Vector2.right;
        _animator.SetFloat("direction", _vectordir.x);
    }

    private void Update()
    {
        if (!_isSuicide) // 플레이어에게 붙어 자살할 때는 움직이게 하지 않는다.
        {
            _changedirTime += Time.deltaTime;

            if (!_isdetect) // 플레이어를 감지하는 상태이다.
            {
                transform.Translate(_vectordir * 3.0f * Time.deltaTime);
                if (_changedirTime > 3.0f) // 일정 시간이 지나면 시점을 바꿔준다.
                {
                    _vectordir.x *= -1;
                    _animator.SetFloat("direction", _vectordir.x);
                    _changedirTime = 0;
                }

                if (Vector2.Distance(Player.instance.transform.position, transform.position) < 5.0f) // 매 프레임마다 Raycast는 Performance를 낭비할 수 있으므로, 일정 거리 안으로 들어오면 Ray를 쏜다.
                {
                    // Player를 RayCast로 찾는다.
                    _ray2D = Physics2D.Raycast(transform.position, _vectordir, 5.0f, 1 << LayerMask.NameToLayer("Player"));

                    if (_ray2D.collider != null) // 플레이어가 감지되면 인페테란처럼 빠르게 플레이어가 있는 방향으로 달려간다.
                    {
                        _isdetect = true;
                        _animator.SetBool("Run", _isdetect);
                        _changedirTime = 0;
                    }
                }
            }
            else // 플레이어를 감지해서 플레이어의 방향으로 빠르게 달려가는 상태이다.
            {
                transform.Translate(_vectordir * 9.0f * Time.deltaTime);
                if (_changedirTime > 2.0f) // 3초동안 플레이어를 만나 자살하지 못할 경우
                {
                    _isdetect = false; // 다시 탐색 상태로 돌아간다.
                    _vectordir.x *= -1; // 방향 전환
                    _changedirTime = 0;
                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            _animator.SetTrigger("suicide");
            _isSuicide = true;
        }
    }

    public void Delete() // Animation Func
    {
        gameObject.SetActive(false);
    }

    public void StopObject() // Animation Func
    {
        _isSuicide = true;
    }

    public void Attacked() // Animation Func, 폭발 프레임 쯤에 심어놓을 예정.
    {
        SpawnMonstersManager.instance.CatchMonster();
        Player.instance.Attacked(damage);
    }
}
