using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour {
    public CreatureController Creature { get; private set; }
    public bool Equiped { get; set; }
    public Gun GetGunInfo {
        get {
            if (_gunInfo.bulletType != null) {
                return _gunInfo;
            }
            else {
                return GetGunInfoFromPrefab();
            }
        }
    }

    public int GetRemainBullet { get => _ammo; }

    public bool IsFever {
        set {
            if (value) {
                _coShootWaitSecReload = new WaitForSeconds(0.0f);
                _coShootWaitSecShootCoolTime = new WaitForSeconds(0.05f);
            }
            else {
                _coShootWaitSecReload = new WaitForSeconds(_gunInfo.reloadTime);
                _coShootWaitSecShootCoolTime = new WaitForSeconds(_gunInfo.shootCoolTime);
            }
        }
    }

    public bool SetTriggerState { set => _triggerState = value; }
    public bool GetReload { get => _reload; }

    Transform _shootPoint;
    Vector3 _gunLook;
    Vector3 _aimTargetPos;
    Vector3 _shootPointCorrectionPistol = new Vector3(0.0f, 0.2f, 0.0f);
    Vector3 _shootPointCorrectionRifle = new Vector3(0.0f, 0.0f, 0.0f);
    const float rad2Deg = 57.29298f;
    const float rad90Deg = 1.578f;
    GameObject _shootPointGo;
    GameObject _bullet;
    Gun _gunInfo = new Gun();
    int _ammo;
    int _gunName;
    GameObject _bulletType;
    SpriteRenderer _spriteRenderer;
    WaitForSeconds _coShootWaitSecReload;
    WaitForSeconds _coShootWaitSecShootCoolTime;
    PlayerController _playerController;
    bool _triggerState = false;
    bool _reload = false;
    Coroutine _coShoot;
    
    private void OnEnable() {

        // 총기 소유 피아식별
        Transform shooter;
        shooter = transform.parent.parent;

        Creature = shooter.GetComponent<CreatureController>();

        _playerController = GameObject.Find("Player").GetComponent<PlayerController>();

        if (Creature is PlayerController) {
            _aimTargetPos = Manager.Mouse.CheckMousePos();
        }
        else if (Creature is EnemyController) {
            _aimTargetPos = _playerController.transform.position;
        }
        else {
            Debug.Log("Shooter is UnDefined");
        }

        GetGunInfoFromPrefab();
        _gunName = int.Parse(_gunInfo.name);
    }

    private void Start() {
        Init();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

    }

    private void OnDisable() {
        if(Creature is PlayerController && !Equiped) {
            Transform bulletPool = _bullet.transform.parent;
            if (bulletPool != null) {
                Manager.Pool.DeletePool(_bulletType);
            }
        }
    }

    private void Update() {
        RotateGun();

        if (_triggerState && (_ammo > 0)) {
            _triggerState = false;
            _coShoot = StartCoroutine("CoShoot");
        }
        
    }

    protected void Init() {
        GetGunInfoFromPrefab();
    }

    protected Gun GetGunInfoFromPrefab() {
        // JSON으로부터 해당 Obj의 총기 정보 가져오기
        Dictionary<string, Gun> gunDict = Manager.Data.gunDict;
        int idxValue = gameObject.name.IndexOf("_");
        string name = gameObject.name.Substring(idxValue + 1);

        gunDict.TryGetValue(name, out _gunInfo);

        _bulletType = (GameObject)Resources.Load($"Prefabs/Bullets/Bullet_{_gunInfo.bulletType}");
        
        // 총구 위치 정보 가져오기
        _shootPointGo = transform.Find("Point").gameObject;
        _shootPoint = _shootPointGo.transform;
        _ammo = _gunInfo.ammo;


        

        _coShootWaitSecReload = new WaitForSeconds(_gunInfo.reloadTime);
        _coShootWaitSecShootCoolTime = new WaitForSeconds(_gunInfo.shootCoolTime);

        return _gunInfo;
    }

    protected void RotateGun() {
        if(Creature is PlayerController) {
            _aimTargetPos = Manager.Mouse.CheckMousePos();

        }
        else {
            _aimTargetPos = _playerController.transform.position;
        }

        // 총기가 향하는 방향에 따른 스프라이트 flip
        _gunLook = (_aimTargetPos - transform.position).normalized;

        if (_gunLook.x < 0 && _gunName < 100) {
            _spriteRenderer.flipY = true;
        }
        else if(_gunLook.x >= 0 && _gunName < 100) {
            _spriteRenderer.flipY = false;
        }
        else if (_gunLook.x < 0 && _gunName >= 100) {
            _spriteRenderer.flipX = true;
        }
        else if (_gunLook.x >= 0 && _gunName >= 100) {
            _spriteRenderer.flipX = false;
        }



        float angle;
        if (_gunName > 100) {
            angle = (Mathf.Atan2(_gunLook.y, _gunLook.x) + rad90Deg) * rad2Deg;  // target에 대한 xy방향벡터를 통해 tan 각도 구하기
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);  //Z축 중심으로 angle만큼 회전
        }
        else {
            angle = Mathf.Atan2(_gunLook.y, _gunLook.x) * rad2Deg;  // target에 대한 xy방향벡터를 통해 tan 각도 구하기
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);  //Z축 중심으로 angle만큼 회전
        }

        _shootPoint = _shootPointGo.transform;

    }

    float Atan2Degree(Vector3 dest) {
        return Mathf.Atan2(dest.y, dest.x) * rad2Deg; // 57.29578 == Mathf.Rad2Deg
    }

    

    IEnumerator CoShoot() {


        Vector3 shootDir = (_aimTargetPos - _shootPoint.position).normalized;

        float rot = Atan2Degree(shootDir);  
        if (_gunName < 100 && _spriteRenderer.flipY) {
            // 총알 생성지점 보정으로 인한 shootDir 보정(Cursor 위치 보정)
            shootDir = (_aimTargetPos - _shootPointCorrectionPistol - _shootPoint.position).normalized;
            rot = Atan2Degree(shootDir);
            _bullet = Manager.Pool.UsePool(Creature, _bulletType, _shootPoint.position + (shootDir * 0.5f) + _shootPointCorrectionPistol, Quaternion.Euler(0.0f, 0.0f, rot), shootDir);
        }
        else if (_gunName >= 100 && _spriteRenderer.flipX) {
            _bullet = Manager.Pool.UsePool(Creature, _bulletType, _shootPoint.position + (shootDir * 0.5f) + _shootPointCorrectionRifle, Quaternion.Euler(0.0f, 0.0f, rot), shootDir);
        }
        else {
            _bullet = Manager.Pool.UsePool(Creature, _bulletType, _shootPoint.position + (shootDir * 0.5f), Quaternion.Euler(0.0f, 0.0f, rot), shootDir);
        }

        // 총알 소모 카운트
        _ammo -= 1;

        // 총알 잔량 UI에 전달
        if(Creature is PlayerController) {
            PlayerController.RemainAmmoAction.Invoke(_ammo);
        }

        if (_ammo == 0) {
            _reload = true;
            if (Creature is PlayerController && !((PlayerController)Creature).IsFever) {
                Manager.Mouse.ReloadMouseShape();
            }
            yield return _coShootWaitSecReload;

            _coShoot = null;
            _ammo = _gunInfo.ammo;
            _reload = false;
            if (Creature is PlayerController && !((PlayerController)Creature).IsFever) {
                Manager.Mouse.DefaultMouseShape();
            }

            PlayerController.RemainAmmoAction.Invoke(_ammo);
        }
        else {

            yield return _coShootWaitSecShootCoolTime;

            _coShoot = null;
        }
    }
}