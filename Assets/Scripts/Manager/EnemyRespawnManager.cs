using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static EnumList;

public class EnemyRespawnManager {
    public int RemainEnemy { get => _remainEnemy; set => _remainEnemy = value; }
    
    int _respawnRange = 5;
    int _remainEnemy = 0;

    Camera _cam;
    MapManager.Pos _pos;
    MapManager.Pos _playerPos;
    MapManager.Pos _respawnRangePos;
    Stack<MapManager.Pos> respawnPosStack = new Stack<MapManager.Pos>();
    Vector3 _viewLeftDownPos = new Vector3(0.0f, 0.0f, -10.0f);
    MapManager.Pos[] _respawnArray;


    public void Init() {
        _cam = Camera.main;
        CheckMaxCamRange();
    }

    // playerTransPos를 기준으로 enemyGameObject를 respawnPattern에 따라 density만큼 Respawn
    public void Respawn(Vector3 playerTransPos, GameObject enemyGameObject, RespawnPattern respawnPattern = RespawnPattern.NormalRandom, int density = 1) {
        respawnPosStack.Clear();
        _playerPos = Manager.Map.CellToArrPos(Manager.Map.Grid.WorldToCell(playerTransPos));


        switch (respawnPattern) {
            case RespawnPattern.NormalRandom:
                break;
            case RespawnPattern.Square:
               
                SetMaxRespawnRange(playerTransPos, CheckMaxCamRange());

                SetSquarePattern(_respawnRange, _playerPos);
                MakeEnemy(enemyGameObject, density);
                break;
        }
    }

    // Respawn에서 전달 받은 GameObject를 density만큼 PoolManager에서 호출 및 생성한다
    void MakeEnemy(GameObject enemyGameObject, int density = 1) {
        if (density < 1 || density > respawnPosStack.Count) {
            density = respawnPosStack.Count / 4;
        }

        for (int i = 0; i < density; ++i) {
            _pos = _respawnArray[i];
            Vector3Int cellPos = Manager.Map.ArrToCellPos(_pos);
            GameObject _enemyGameObject;
            _enemyGameObject = Manager.Pool.UsePool(enemyGameObject);
            _enemyGameObject.transform.position = cellPos;

            RemainEnemy += 1;

            Debug.Log(Manager.EnemyRespawn.RemainEnemy);
        }

    }

    // 메인 카메라로부터 _viewLeftDownPos를 기준으로 카메라 외곽 x좌표를 반환
    float CheckMaxCamRange() {
        // cam이 현재 반대에서 촬영 중인 것 감안하여 min/max 바뀌어서 변수에 저장
        Vector3 worldLeftDownPos = _cam.ViewportToWorldPoint(_viewLeftDownPos);

        float _maxX;
        _maxX = worldLeftDownPos.x;

        return _maxX;
    }

    // 꼭지점 제외한 사각형 꼴의 리스폰 희망 위치들을 Map Manger로부터 collision체크 후 respawnPosStack에 Push
    void SetSquarePattern(int distance, MapManager.Pos playerArrPos) {
        if (distance < 1) {
            distance = 10;
        }


        MapManager.Pos respawnPos = playerArrPos;
        respawnPos.X -= distance;
        respawnPos.Y -= distance;
        int initYPos = respawnPos.Y;

        // 리스폰 지역 설정 및 충돌 체크
        for (int i = 0; i <= distance * 2; ++i) {
            for (int j = 0; j <= distance * 2; ++j) {

                // 좌상단 / 우하단 모서리
                if (i == j) {
                    ++respawnPos.Y;
                    continue;
                }

                // 프레임 내부
                if ((i != 0 && i != distance * 2) && (j > 0 && j < distance * 2)) {
                    ++respawnPos.Y;
                    continue;
                }

                // 우상단 / 좌하단 모서리
                if (i == (distance * 2 - j)) {
                    ++respawnPos.Y;
                    continue;
                }

                // collision이 없고, map 영역 이내에서 respawn 되도록 설정
                if (Manager.Map.CheckCollision(respawnPos)) {
                    respawnPosStack.Push(respawnPos);
                    ++respawnPos.Y;
                }

                // enemy respawn 위치를 랜덤 순서로 저장
                _respawnArray = respawnPosStack.OrderBy(pos => Random.value).ToArray();
            }

            respawnPos.Y = initYPos;
            ++respawnPos.X;
        }
    }

    // maxCamRange와 playerTransPos를 기준으로 리스폰할 위치 지정
    void SetMaxRespawnRange(Vector3 playerTransPos, float maxCamRange) {
        playerTransPos.x = maxCamRange - playerTransPos.x;
        _respawnRangePos = Manager.Map.CellToArrPos(Manager.Map.Grid.WorldToCell(playerTransPos));

        _respawnRange = _respawnRangePos.X - _playerPos.X + 1;
    }
}