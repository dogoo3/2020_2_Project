using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropTrapGround : MonoBehaviour
{
    private List<Collider2D> _onObject = new List<Collider2D>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer != LayerMask.NameToLayer("GroundCollider"))
            _onObject.Add(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("GroundCollider"))
            _onObject.Remove(collision);
    }

    public void ChangeDisPlace(float _displace)
    {
        for (int i = 0; i < _onObject.Count; i++)
            _onObject[i].transform.Translate(0, _displace, 0);
    }
}
