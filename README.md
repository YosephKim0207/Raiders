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


## 목표를 위한 구현사항

--- 

### 길찾기 함수 콜 최적화  

[구현예시영상](https://youtu.be/kSDWP_GfiOQ?t=151)


![길찾기_용량최적화](https://user-images.githubusercontent.com/46564046/235311817-ffe93472-bddd-450d-aa97-f68bf0f40e0b.gif)

문제
- 다수의 NPC 출현시 성능 저하

원인
- 모든 NPC가 주기적으로 길찾기 함수를 호출하여 성능 부담

해결
- 전투 시스템 및 레벨 디자인 방향성을 고려하여 장애물 발견시에만 길찾기 함수 호출
- 캐릭터의 이동속도가 느린 점을 이용하여 일정 시간 동안 기존의 경로Stack 재사용
- 코루틴 사용 

결과
- 스크립트 CPU 사용률 80% 감소

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
        // rayHit에 충돌한 오브젝트가 없고 && Manager의 FindPath로 탐색해둔 경로가 없으면 직선이동
        if (rayHit.transform == null && _pathStack == null) {

            PathState = FindPathState.UseDirect;

        }
        // 전방에 장애물 없고 && Manager의 FindPath를 통해 찾은 경로를 정해진 횟수 이상 사용한 경우 직선이동으로 전환
        else if (rayHit.transform == null && usePathStackCount > pathStackUsageCount) {
            PathState = FindPathState.UseDirect;
            _pathStack = null;
            usePathStackCount = 0;
        }
        // Manager의 FindPath로 탐색해둔 경로 stack이 없거나 || _pathStack가 비어있으면 Manager의 FindPath를 호출하기 위해 PathStated를 ReFindPath로
        else if (_pathStack == null || _pathStack.Count == 0) {
            PathState = FindPathState.ReFindPath;
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
                break;
        }

        yield return WaitFindPathTime;

        _coFindPath = null;
    }

    // _pathStack에 저장된 경로를 이용하여 경로 설정 
    void SetPathUseStack() {
        Vector3 nextPos;
        if (_pathStack == null || _pathStack.Count == 0) {
            Debug.Log($"Error : {gameObject.name}'s _pathStack is null whlie using SetPathUseStack()");
            _pathStack = null;
            DestPos = (_shootTargetTransform.position - transform.position).normalized;
            return;
        }

        nextPos = _pathStack.Pop();
        ++usePathStackCount;
        if ((_pathStack.Count > 0) && (nextPos - transform.position).magnitude < 0.5) {
            nextPos = _pathStack.Pop();
        }
        DestPos = (nextPos - transform.position).normalized;
    }
```

</details>




### 투사체 발사 코드 개선

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

    
   


### 오브젝트 풀링  [구현예시영상](https://youtu.be/kSDWP_GfiOQ?t=39)


![풀링_용량최적화](https://user-images.githubusercontent.com/46564046/235311704-d339b2c3-2948-469e-b3e9-0d7056496190.gif)

문제
- 다수의 NPC가 사격시 성능 저하

원인
- Bullet 객체가 빈번하게 생성 및 소멸하여 메모리 할당 / 해제, GC, 메모리 파편화 문제 발생
- Bullet 객체뿐만 아니라 NPC의 잦은 생성 및 소멸도 문제

해결
- 오브젝트 풀링 구현

결과
- CPU 사용률 50% 감소

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

