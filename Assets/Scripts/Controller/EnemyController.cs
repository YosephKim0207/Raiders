using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnumList;

public class EnemyController : CreatureController {
    [SerializeField]
    public int pathStackUsageCount = 5;

    Transform _shootTargetTransform;
    FindPathState PathState = FindPathState.UseDirect;
    Coroutine _coDoSomething;
    Coroutine _coFindPath;
    WaitForSeconds WaitFindPathTime;
    
    int usePathStackCount = 0;
    Stack<Vector3> _pathStack;
    
    bool _isDead = false;

    protected override void Init() {
        base.Init();
        HP = 10;
        _speed *= 0.4f;

        GunInit();

        GameObject player;
        player = GameObject.Find("Player");
        _shootTargetTransform = player.transform;

        float coFIndPathTime = 0.25f;
        WaitFindPathTime = new WaitForSeconds(coFIndPathTime);
    }


    protected override void UpdateController() {
        if (_coDoSomething == null && Manager.PlayerData.GamePlayState == GameState.Playing) {
            _coDoSomething = StartCoroutine("CoDoSomething");
        }
        else if(Manager.PlayerData.GamePlayState == GameState.GameOver || Manager.PlayerData.GamePlayState == GameState.Ending){
            State = CreatureState.Idle;
        }

        switch (State) {
            case CreatureState.Idle:
                UpdateIdle();
                break;
            case CreatureState.Move:  
                UpdateAttack();
                UpdateMoving();
                break;
            case CreatureState.Attack: 
                UpdateAttack();
                break;
            case CreatureState.Jump:
                UpdateJump();
                break;
            case CreatureState.Damaged:
                UpdateDamaged();
                break;
            case CreatureState.Dead:
                UpdateDead();
                break;
        }
    }

    protected override void UpdateMoving() {
        if (_coFindPath == null) {
            _coFindPath = StartCoroutine("CoFindPath");
        }

        base.UpdateMoving();
    }
    
    protected override void UpdateDead() {

        // temp random rate
        if (!_isDead) {
            Debug.Log("Eenemy : Dead!");

            _isDead = true;
            int rand = UnityEngine.Random.Range(0, 100);

            // 70프로의 확률로 아무 것도 드랍 안 함
            // 10프로의 확률로 HP 포션
            // HP 포션은 접근시 자동 섭취
            if (rand >= 15 && rand < 65) {
                ChooseDropItem(ItemType.Item, rand);
            }
            // 10프로의 확률로 normal 총기
            // 그 중에서 5/5로 pistol or assaultRifle
            else if (rand >= 65 && rand < 80) {
                ChooseDropItem(ItemType.Gun, rand, 0);
            }
            // 5프로의 확률로 rare 총기
            // 그 중 5/5로 pistol or assaultRifle
            else if (rand >= 80 && rand < 85) {
                ChooseDropItem(ItemType.Gun, rand, 1);
            }
            // 5프로의 확률로 epic 총기
            //else if (rand >= 85 && rand < 90) {

            //}
            // 2프로의 확률로 fever time 아이템
            // fever 아이템은 접근시 자동 섭취, 원하는 떄에 아이템 사용 가능
            // UI로 아이템 먹었는지 표시하기
            else if (rand >= 90 && rand < 100) {
                ChooseDropItem(ItemType.Item, rand);
            }

            Manager.EnemyRespawn.RemainEnemy -= 1;
            Debug.Log(Manager.EnemyRespawn.RemainEnemy);

            Manager.Pool.PushPoolChild(this.gameObject);
        }
    }

    // 아이템 종류와 레어도에 따른 드랍할 아이템 선택
    void ChooseDropItem(ItemType itemType, int rand, int itemRare = 0) {
        switch (itemType) {
            case ItemType.Item:
                // 포션
                if (rand >= 15 && rand < 65) {
                    DropItem($"{itemType}_HpPotion");
                }
                // Fever
                else if (rand >= 98 && rand < 100) {
                    DropItem($"{itemType}_Fever");
                }
                break;

            case ItemType.Gun:
                int gunType = (rand % 2);
                if (gunType == 0) {
                    DropItem($"{itemType}_{gunType}{itemRare}1");
                }
                else if (gunType == 1) {
                    DropItem($"{itemType}_{gunType}{itemRare}1");
                }
                break;
        }
    }

    // 게임월드상에 아이템 드롭
    void DropItem(string itemName) {
        GameObject dropItem;
        dropItem = Resources.Load<GameObject>($"Prefabs/Items/{itemName}");
        if (dropItem != null) {
            dropItem = Object.Instantiate<GameObject>(dropItem, transform.position, Quaternion.identity);
            dropItem.name = itemName;
        }
    }

    void OnEnable() {
        _isDead = false;
        GameObject player;
        player = GameObject.Find("Player");
        if (player) {
            _shootTargetTransform = player.transform;
        }
    }

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

    // Enemy의 행동을 coDoTime마다 설정
    IEnumerator CoDoSomething() {
        int rand = UnityEngine.Random.Range(0, 10);

        if (rand >= 0 && rand < 1) {
            State = CreatureState.Attack;
        }
        else if (rand >= 1 && rand < 10) {
            State = CreatureState.Move;
        }

        float coDoTime;
        switch (State) {
            case CreatureState.Attack:
                coDoTime = UnityEngine.Random.Range(0, 2);
                break;
            case CreatureState.Move:
                coDoTime = UnityEngine.Random.Range(2, 7);
                break;
            default:
                coDoTime = 1.0f;
                break;
        }

        yield return new WaitForSeconds(coDoTime);

        _coDoSomething = null;
    }
}