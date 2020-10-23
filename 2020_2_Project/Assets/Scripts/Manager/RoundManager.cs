using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class RoundManager : MonoBehaviour
{
    public static RoundManager instance;

    [SerializeField] private SpawnMonstersManager[] rounds = default;

    [SerializeField] private Transform[] roundStartPos = default;

    [SerializeField] private Collider2D[] cameraCollider = default;
    [SerializeField] private CinemachineConfiner cinemachineConfiner = default;

    public string[] bgmname;

    private Animator _animator;

    [HideInInspector]
    public int nowRound;

    private void Awake()
    {
        instance = this;

        _animator = GetComponent<Animator>();
        StopBGM();
    }

    public void StopBGM() // Animation Func
    {
        SoundManager.instance.StopBGM();
    }

    public void GameStart() // Animator Func
    {
        SoundManager.instance.PlayBGM(bgmname[nowRound]);
        SpawnMonstersManager.instance.GameStart();
        FarmingManager.instance.GameStart();
    }

    public void PlayBeepSound() // Animator Func
    {
        SoundManager.instance.PlaySFX("beepsound");
    }

    public void EnableAnimator() // Animation Func
    {
        _animator.Play("countdown", -1, 0);
    }

    public void SetRound(bool _is) // 몬스터 및 아이템의 파밍을 관리하는 매니저 변수를 조정해주는 함수
    {
        rounds[nowRound].gameObject.SetActive(_is);
    }

    public void SetCameraCollider() // 라운드 변경 시 CineMachineConfiner를 조정해주는 함수
    {
        cinemachineConfiner.m_BoundingShape2D = cameraCollider[nowRound];
    }

    public Vector3 GetRoundStartPos(int _index) // 라운드를 새로 시작할 때 플레이어의 위치를 조정해 주는 함수
    {
        return roundStartPos[_index].position;
    }
}
