using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletTestController : MonoBehaviour
{
    Rigidbody2D _rigidbody;
    Vector2 _velo;
    float _speed = 5.0f;
    Coroutine _coTest = null;
    
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_coTest == null) {
            _coTest = StartCoroutine("GetMoveDir");
        }        
    }

    IEnumerator GetMoveDir() {
        if (Input.GetKey(KeyCode.W)) {
            _velo = Vector2.up;

            if (Input.GetKey(KeyCode.A)) {
                _velo = (Vector2.up + Vector2.left).normalized;
            }
            else if (Input.GetKey(KeyCode.D)) {
                _velo = (Vector2.up + Vector2.right).normalized;
            }
        }
        else if (Input.GetKey(KeyCode.S)) {
            _velo = Vector2.down;

            if (Input.GetKey(KeyCode.A)) {
                _velo = (Vector2.down + Vector2.left).normalized;
            }
            else if (Input.GetKey(KeyCode.D)) {
                _velo = (Vector2.down + Vector2.right).normalized;
            }
        }
        else if (Input.GetKey(KeyCode.A)) {
            _velo = Vector2.left;
        }
        else if (Input.GetKey(KeyCode.D)) {
            _velo = Vector2.right;
        }
        else {
            _velo = Vector2.zero;
        }

        _rigidbody.velocity = _velo * _speed;

        yield return new WaitForSeconds(2.0f);
        _coTest = null;
    }
}
