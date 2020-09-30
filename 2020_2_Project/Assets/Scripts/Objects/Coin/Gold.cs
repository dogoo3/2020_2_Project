using UnityEngine;
using System.Collections;

public class Gold : MonoBehaviour
{
    private Rigidbody2D _rigidbody2d;
    private Collider2D _collider2d;
    
    public MeshRenderer meshRenderer;
    private Color _coloralpha;

    private bool _isGet;
    private int _goldValue;
    
    private void Awake()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _collider2d = GetComponent<Collider2D>();
        _coloralpha = new Color(1, 1, 1, 1);
    }

    public void SetGoldValue()
    {
        _goldValue = Random.Range(50, 201);
    }

    private void OnEnable()
    {
        _rigidbody2d.velocity = new Vector2(0, 4.0f);
        _coloralpha.a = 1; // 다시 보여야 하기 때문에 알파값 1로 설정
        _isGet = false; // 획득 설정 초기화
        meshRenderer.material.color = _coloralpha; // 머터리얼의 색상 설정 변경
        meshRenderer.sortingLayerName = "Object";
        meshRenderer.sortingOrder = 2;
        Invoke("EnableCollider", 1.0f); // 1초뒤 활성화
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            GoldManager.instance.GetGold(_goldValue);
            _isGet = true;
        }
    }

    private void EnableCollider() // Invoke Func
    {
        _collider2d.enabled = true;
    }

    private void Update()
    {
        if(_isGet)
        {
            _coloralpha.a -= 0.0166f;
            meshRenderer.material.color = _coloralpha;
            if(_coloralpha.a <= 0)
                ObjectPoolingManager.instance.InsertQueue(this, ObjectPoolingManager.instance.queue_gold);
        }
        else
            transform.Rotate(0, 1.5f, 0);
    }

    private void OnDisable()
    {
        _collider2d.enabled = false;    
    }
}
