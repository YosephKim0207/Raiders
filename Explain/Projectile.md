# Projectile 

문제
- 다수의 총알이 발사되는 Fever 스킬 사용시 및 다수의 NPC가 사격시 성능 저하

원인
- 총알을 발사하는 캐릭터 판정 및 충돌 판정에 대한 불필요한 정보 전달
- BulletController의 Update함수 내부에 존재하는 불필요한 연산

해결
- 오브젝트 풀에서 총알 호출 시 총알의 Transform과 총알을 생성하는 클래스만 전달
- Update함수에서는 총알이 카메라를 벗어났는지만 확인

결과
- 스크립트 CPU 사용률 50% 감소

[BulletController 전체 코드 바로가기](https://github.com/YosephKim0207/Raiders/blob/main/Assets/Scripts/Controller/BulletController.cs)
<details>
<summary>BulletController 전체 코드 펼치기</summary>



```csharp
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
```

</details>
