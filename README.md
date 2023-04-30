# Raiders

---

[![플레이핵심완성](https://user-images.githubusercontent.com/46564046/233078635-f2e0a485-f1a0-4e0a-b409-a18958122472.gif)](https://youtu.be/kSDWP_GfiOQ)

[![uI중심](https://user-images.githubusercontent.com/46564046/235346555-7c71c85d-66e9-4149-bd13-4da8339f69d9.gif)](https://youtu.be/kSDWP_GfiOQ)

[전체영상링크](https://youtu.be/kSDWP_GfiOQ)

---

## 프로젝트 소개  |  엔터더건전의 리소스를 활용하여 속도감 있는 탑다운뷰 슈팅 게임 구현
- 게임 장르 : 탑다운 2D 슈팅
- 목표 : 다수의 오브젝트가 등장하면서도 속도감 있는 최적화 된 게임 제작
- 플랫폼 : Windows, Mac
- 사용 엔진과 언어 : Unity, C#


---

## 구현목표

dkljls
- Fever 아이템을 

## 목표를 위한 구현사항

--- 

### 오브젝트 풀링  [구현예시영상](https://youtu.be/kSDWP_GfiOQ?t=39)


![풀링_용량최적화](https://user-images.githubusercontent.com/46564046/235311704-d339b2c3-2948-469e-b3e9-0d7056496190.gif)

- 게임 내 중점적으로 생성 / 소멸할 적과 총알 객체에 대한 오브젝트 풀링 적용

[PoolManager 전체 코드 바로가기](https://github.com/YosephKim0207/Raiders/blob/main/Assets/Scripts/Manager/PoolManager.cs)
<details>
<summary>PoolManager 전체 코드 펼치기</summary>



```csharp
public class PoolManager {

    class Pool {
        Stack _poolStack = new Stack();

        // initNum만큼 Pool에 child Push하는 초기화 진행
        public void Init(GameObject originObj, GameObject parentObj, int initNum = 5) {
            for (int i = 0; i < initNum; ++i) {
                GameObject childObj = Object.Instantiate(originObj, parentObj.transform);
                childObj.name = $"{originObj.name}";
                childObj.SetActive(false);
                _poolStack.Push(childObj);
            }
        }

        public void Push(GameObject originObj, GameObject parentObj = null) {
            originObj.SetActive(false);
            _poolStack.Push(originObj);
        }

        public GameObject Pop(Vector3 pos, Quaternion rot) {
            GameObject childObj = (GameObject)_poolStack.Pop();

            childObj.transform.position = pos;
            childObj.transform.rotation = rot;

            childObj.SetActive(true);
            return childObj;
        }

        public GameObject Pop(Vector3 pos, Quaternion rot, Vector3 shootDir) {
            GameObject childObj = (GameObject)_poolStack.Pop();

            childObj.transform.position = pos;
            childObj.transform.rotation = rot;
            
            BulletController bullet = childObj.GetComponent<BulletController>();
           
            bullet.DestPos = shootDir;

            childObj.SetActive(true);
            return childObj;
        }

        public GameObject Pop(Vector3 pos, Quaternion rot, Vector3 shootDir, CreatureController creatureCS) {
            GameObject childObj = (GameObject)_poolStack.Pop();

            childObj.transform.position = pos;
            childObj.transform.rotation = rot;

            BulletController bullet = childObj.GetComponent<BulletController>();
            bullet.DestPos = shootDir;
            bullet.SetCreature = creatureCS;

            childObj.SetActive(true);
            return childObj;
        }

        public int Count() {
            return _poolStack.Count;
        }
    }

    // Obj당 Pool들 관리
    Dictionary<string, Pool> _poolDic = new Dictionary<string, Pool>();    
    GameObject _root;

    public void Init() {
        _root = GameObject.Find("@RootPool");

        if(_root == null) {
            _root = new GameObject("@RootPool");
        }
    }

    public GameObject UsePool(GameObject originObj) { 
        Transform poolObj = _root.transform.Find(originObj.name);
        if (poolObj == null) {
            // Pool들의 root 하위로 originObj에 대한 pool 생성
            poolObj = InitPoolObj(originObj, _root.transform).transform;

            // return PopPoolChild(originObj, poolObj);
            return PopPoolChild(originObj, poolObj.gameObject, Vector3.zero, Quaternion.identity);
        }
        else {
            // return PopPoolChild(originObj, poolObj);
            return PopPoolChild(originObj, poolObj.gameObject, Vector3.zero, Quaternion.identity);
        }
    }
    public GameObject UsePool(GameObject originObj, Vector3 pos, Quaternion rot, Vector3 shootDir) {
        //GameObject poolObj = GameObject.Find(originObj.name);
        Transform poolObj = _root.transform.Find(originObj.name);
        if (poolObj == null) {
            // Pool들의 root 하위로 originObj에 대한 pool 생성
            poolObj = InitPoolObj(originObj, _root.transform).transform;

            // return PopPoolChild(originObj, poolObj);
            return PopPoolChild(originObj, poolObj.gameObject, pos, rot, shootDir);
        }
        else {
            // return PopPoolChild(originObj, poolObj);
            return PopPoolChild(originObj, poolObj.gameObject, pos, rot, shootDir);
        }
    }

    public GameObject UsePool(CreatureController creatureSC, GameObject originObj, Vector3 pos, Quaternion rot, Vector3 shootDir) {
        Transform poolObj = _root.transform.Find(originObj.name);
        if (poolObj == null) {
            // Pool들의 root 하위로 originObj에 대한 pool 생성
            poolObj = InitPoolObj(originObj, _root.transform).transform;

            return PopPoolChild(creatureSC, originObj, poolObj.gameObject, pos, rot, shootDir);
        }
        else {
            return PopPoolChild(creatureSC, originObj, poolObj.gameObject, pos, rot, shootDir);
        }
    }

    GameObject InitPoolObj(GameObject originObj, Transform parent) {
        // Pooling된 Obj들이 들어갈 부모 Obj 만들기
        GameObject poolObj = new GameObject(name : originObj.name);
        poolObj.transform.parent = parent;

        // 새로 생긴 poolObj 초기화
        Pool pool = new Pool();
        pool.Init(originObj, poolObj);

        // 원본obj이름을 key로 딕셔너리에 Stack add
        _poolDic.Add(originObj.name, pool);

        return poolObj;
    }

    GameObject PopPoolChild(GameObject originObj, GameObject parentObj, Vector3 pos, Quaternion rot) {
        // 해당 오브젝트의 pool이 없는 경우
        if (_poolDic.ContainsKey(originObj.name).Equals(false)) {
            //Debug.Log("PopPoolChild_ContainsKey : True");

            Pool pool = new Pool();
            pool.Init(originObj, parentObj);
            _poolDic.Add(originObj.name, pool);
            return pool.Pop(pos, rot);
        }
        // 해당 오브젝트의 pool이 있는 경우 
        else {
            Pool pool = _poolDic[originObj.name];

            // 비활성화 된 Obj가 남아있는 경우
            if (!pool.Count().Equals(0)) {
                //return pool.Pop();
                return pool.Pop(pos, rot);
            }
            // 비활성화 된 Obj가 남아있지 않은 경우
            else {
                // Obj를 Pool에 추가 생성(Init을 이용) 및 Pop
                pool.Init(originObj, parentObj, 1);
                //return pool.Pop();
                return pool.Pop(pos, rot);
            }
        }
    }

    // Pool에서 Obj pop하여 사용하기 
    GameObject PopPoolChild(CreatureController creatureCS, GameObject originObj, GameObject parentObj, Vector3 pos, Quaternion rot, Vector3 shootDir) {
        // 해당 오브젝트의 pool이 없는 경우
        if (_poolDic.ContainsKey(originObj.name).Equals(false)) {
            //Debug.Log("PopPoolChild_ContainsKey : True");

        Pool pool = new Pool();
        pool.Init(originObj, parentObj);
        _poolDic.Add(originObj.name, pool);
        return pool.Pop(pos, rot, shootDir, creatureCS);

        }
        // 해당 오브젝트의 pool이 있는 경우 
        else {
            Pool pool = _poolDic[originObj.name];

            // 비활성화 된 Obj가 남아있는 경우
            if (!pool.Count().Equals(0)) {
                return pool.Pop(pos, rot, shootDir, creatureCS);
            }
            // 비활성화 된 Obj가 남아있지 않은 경우
            else {
                // Obj를 Pool에 추가 생성(Init을 이용) 및 Pop
                pool.Init(originObj, parentObj, 1);
                return pool.Pop(pos, rot, shootDir, creatureCS);
            }
        }
    }

    // Pool에서 Obj pop하여 사용하기 
    GameObject PopPoolChild(GameObject originObj, GameObject parentObj, Vector3 pos, Quaternion rot, Vector3 shootDir) {
        // 해당 오브젝트의 pool이 없는 경우
        if (_poolDic.ContainsKey(originObj.name).Equals(false)) {
            Debug.Log("PopPoolChild_ContainsKey : True");

            Pool pool = new Pool();
            pool.Init(originObj, parentObj);
            _poolDic.Add(originObj.name, pool);
            return pool.Pop(pos, rot, shootDir);

        }
        // 해당 오브젝트의 pool이 있는 경우 
        else {
            Pool pool = _poolDic[originObj.name];

            // 비활성화 된 Obj가 남아있는 경우
            if (!pool.Count().Equals(0)) {
                return pool.Pop(pos, rot, shootDir);
            }
            // 비활성화 된 Obj가 남아있지 않은 경우
            else {
                // Obj를 Pool에 추가 생성(Init을 이용) 및 Pop
                pool.Init(originObj, parentObj, 1);
                //return pool.Pop();
                return pool.Pop(pos, rot, shootDir);
            }
        }
    }

    // 사용한 오브젝트를 PoolRoot에 반환 
    public void PushPoolChild(GameObject childObj) {
        Pool pool = _poolDic[childObj.name];
        childObj.SetActive(false);
        pool.Push(childObj);
    }

    // Pool 제거
    public void DeletePool(GameObject originObj, Transform parent = null) {
        if(parent == null) {
            GameObject poolObj = GameObject.Find(originObj.name);
            Object.Destroy(poolObj);
            _poolDic.Remove(originObj.name);
        }
        else {
            // TODO
            // parent를 이용하는 경우 추가하기
            Debug.Log("DeletePool Use parent need TODO");
        }
    }

}
```

</details>


### 길찾기 함수 콜 최소화  [구현예시영상](https://youtu.be/kSDWP_GfiOQ?t=143)


![길찾기_용량최적화](https://user-images.githubusercontent.com/46564046/235311817-ffe93472-bddd-450d-aa97-f68bf0f40e0b.gif)

- 코루틴 사용 
- 캐릭터 이동 속도, 필드의 장애물 등을 고려하여 swich문을 통해 필요한 경우에만 길찾기 함수 콜 감소

[EnemyController 전체 코드 바로가기](https://github.com/YosephKim0207/Raiders/blob/main/Assets/Scripts/Controller/EnemyController.cs) /
[MapManager 전체 코드 바로가기](https://github.com/YosephKim0207/Raiders/blob/main/Assets/Scripts/Manager/MapManager.cs)

<details>
<summary>길찾기 코루틴 코드만 펼치기</summary>



```csharp
 // 플레이어를 타겟으로 길을 찾는 코루틴
    IEnumerator CoFindPath() {
        // 플레이어와 일정 거리 이내인 경우 다음 길찾기 중단
        if ((_shootTargetTransform.position - transform.position).magnitude < 2.5f) {
            State = CreatureState.Attack;
        }

        // 진행방향 전방 충돌 점검
        RaycastHit2D rayHit;
        rayHit = Physics2D.Raycast(this.transform.position, DestPos, 2.5f, LayerMask.GetMask("Collision"));
   
        Debug.DrawRay(this.transform.position, DestPos * 1.5f, Color.red, 1.0f);

        // Enemy가 길찾기에 사용할 조건 설정
        // rayHit1에 충돌한 오브젝트가 없고 && Manager의 FindPath로 탐색해둔 경로가 없으면 직선이동
        if (rayHit.transform == null && _pathStack == null) {

            PathState = FindPathState.UseDirect;

        }
        // Manager의 FindPath로 탐색해둔 경로가 _pathStack상에 없거나 || _pathStack가 비어있으면Manager의 FindPath를 호출하기 위해 PathStated를 ReFindPath로
        else if (_pathStack == null || _pathStack.Count == 0) {
            PathState = FindPathState.ReFindPath;
            }
        // Manager의 FindPath를 통해 찾은 경로를 정해진 횟수 이상 사용한 경우 경로 재탐색 및 사용횟수 초기화
        else if (usePathStackCount > pathStackUsageCount) {
            PathState = FindPathState.ReFindPath;
            usePathStackCount = 0;
            }
        // Manager의 FindPath로 찾아둔 경로를 _pathStack에서 가져와 사용
         else {
            PathState = FindPathState.UsePathStack;
         }

        // 정해진 조건대로 분기하여 이동경로 설정
        switch (PathState) {
            case FindPathState.UseDirect:
                DestPos = (_shootTargetTransform.position - transform.position).normalized;
                break;
            case FindPathState.ReFindPath:
                _pathStack = Manager.Map.FindPath(this.transform, _shootTargetTransform);
                SetPathUseStack();
                break;
            case FindPathState.UsePathStack:
                SetPathUseStack();
                ++usePathStackCount;
                break;
        }

        yield return WaitFindPathTime;

        _coFindPath = null;
    }

    // _pathStack에 저장된 경로를 이용하여 경로 설정 
    void SetPathUseStack() {
        Vector3 nextPos;
        nextPos = _pathStack.Pop();
        if ((_pathStack.Count > 0) && (nextPos - transform.position).magnitude < 0.5) {
            nextPos = _pathStack.Pop();
        }
        DestPos = (nextPos - transform.position).normalized;
    }
```

</details>


### 함수 최적화

- Rigidbody.MovePosisition등 Unity Documentation 권장사항을 참고한 물리 함수 사용
- 총알 스크립트 내부 연산을 최소화하여 CPU 사용 GC 0.98ms, 스크립트 0.47ms 감소

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
