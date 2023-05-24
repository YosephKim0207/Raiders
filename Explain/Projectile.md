# Projectile 

- 캐릭터의 Fever 스킬 사용 및 다수의 NPC가 사격시 성능 저하
- Rigidbody.MovePosisition등 Unity Documentation 권장사항을 참고한 물리 함수 사용
- 총알 스크립트 내부 연산을 최소화하여 CPU 사용량 50% 감소 (GC 0.98ms, 스크립트 0.47ms)

[BulletController 전체 코드 바로가기](https://github.com/YosephKim0207/Raiders/blob/main/Assets/Scripts/Controller/BulletController.cs)
<details>
<summary>BulletController 전체 코드 펼치기</summary>



```csharp
public class BulletController : MonoBehaviour {
    public CreatureController SetCreature { set => _creature = value; }
    public Vector3 DestPos { get; set; }

    Rigidbody2D _rigidbody;
    Vector3 _bullPos;
    Camera _cam;
    CreatureController _creature;
    const float _outRange = 0.05f;

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody2D>();
        _cam = Camera.main;
    }

    private void OnEnable() {
        float speed = 28.0f;
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
        // Player가 쏜 총알이 Enemy와 충돌하는 경우
        else if (collision.gameObject.layer.Equals(10)) {
            if (_creature is PlayerController) {
                EnemyController enemy = collision.GetComponent<EnemyController>();

                if (enemy != null) {
                    enemy.HP -= _creature.GunInfo.damage;
                    _creature = null;
                    Manager.Pool.PushPoolChild(gameObject);

                    Debug.Log("Player's Bullet Hit Enemy");
                }
            }
        }
        // Enemy가 쏜 총알이 Player와 충돌하는 경우
        else if (collision.gameObject.layer.Equals(6)) {
            if (_creature is EnemyController) {
                PlayerController player = collision.GetComponent<PlayerController>();
                // player Fever인 경우 무적
                if (player.IsFever == false) {
                    player.HP -= _creature.GunInfo.damage;

                    Debug.Log("Enemy's Bullet Hit Player");
                }

                _creature = null;
                Manager.Pool.PushPoolChild(gameObject);
            }
        }
        else {
            Manager.Pool.PushPoolChild(gameObject);
        }

        
    }
}
```

</details>