using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {
    [SerializeField]
    public float speed = 28.0f;

    public CreatureController SetCreature { set => _creature = value; }
    public Vector3 DestPos { get; set; }

    Rigidbody2D _rigidbody;
    Vector3 _bullPos;
    Camera _cam;
    CreatureController _creature;
    CreatureController _collisionCreature;
    const float _outRange = 0.05f;

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody2D>();
        _cam = Camera.main;
    }

    private void OnEnable() {
        _rigidbody.velocity = DestPos * speed;
    }


    protected virtual void Update() {
        // 카메라 영역+a 벗어나는 경우 총알 제거
        _bullPos = _cam.WorldToViewportPoint(transform.position);
        if (_bullPos.x <= -_outRange || _bullPos.x >= 1.0f + _outRange || _bullPos.y <= -_outRange || _bullPos.y >= 1.0f + _outRange) {
            _creature = null;
            Manager.Pool.PushPoolChild(this.gameObject);
        }
    }

    
    private void OnTriggerEnter2D(Collider2D collision) {
        // 총알 간의 충돌이 발생하는 경우 || 아이템과 충돌하는 경우 
        if (collision.gameObject.layer.Equals(8) || collision.gameObject.layer.Equals(9)) {
            return;
        }

        if (_creature == null) {
            return;
        }

        if (collision.gameObject.layer != _creature.gameObject.layer) {
            _collisionCreature = collision.GetComponent<CreatureController>();

            if (_collisionCreature != null) {
                _collisionCreature.HP -= _creature.GunInfo.damage;
                Debug.Log($"{_creature.name}'s Bullet Hit {_collisionCreature.name}");
                _collisionCreature = null;
            }

            _creature = null;

            Manager.Pool.PushPoolChild(gameObject);
        }
    }
}
