using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrap : MonoBehaviour
{
    private Animator _animator;

    [SerializeField] private float _damage = default;
    [SerializeField] private float _activeWaitTime = default;

    private bool _isCountStart;
    
    private float _elapsedTime;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _animator.SetTrigger("active");
    }

    private void Update()
    {
        if(_isCountStart)
        {
            _elapsedTime += Time.deltaTime;
            if (_elapsedTime >= _activeWaitTime)
            {
                _elapsedTime = 0f;
                _animator.SetTrigger("active");
                _isCountStart = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player.instance.Attacked(_damage);
            SoundManager.instance.PlaySFX("attackfiretrap");
        }
    }

    public void StartCount() // Animation Func
    {
        _isCountStart = true;
    }
}
