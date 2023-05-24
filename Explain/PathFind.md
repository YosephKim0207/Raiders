### 길찾기 함수 콜 최적화  

[구현예시영상](https://youtu.be/kSDWP_GfiOQ?t=143)


![길찾기_용량최적화](https://user-images.githubusercontent.com/46564046/235311817-ffe93472-bddd-450d-aa97-f68bf0f40e0b.gif)

- 다수의 NPC 출현시 모든 NPC가 주기적으로 길찾기 함수를 호출하여 성능 부담
- 전투 시스템 및 레벨 디자인 방향성을 고려하여 장애물 발견시에만 길찾기 함수 호출
- 캐릭터의 이동속도가 느린 점을 이용하여 일정 시간 동안 기존의 경로Stack 재사용
- 코루틴 사용 

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
