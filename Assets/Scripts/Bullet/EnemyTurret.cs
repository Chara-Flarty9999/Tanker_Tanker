using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;
using static Enums;

public class EnemyTurret : MonoBehaviour
{
    [Header("タレット専用の設定")]
    [SerializeField, Tooltip("射出するオブジェクトをここに割り当てる(Normal以外)")]
    private GameObject _throwingObject;

    [SerializeField, Tooltip("標的のオブジェクトをここに割り当てる(Normal以外)")]
    private GameObject _targetObject;

    [SerializeField, Tooltip("射出する速度")]
    private float _projectileSpeed = 20f;

    [SerializeField, Tooltip("射出する座標")]
    private Transform _turretMuzzle;

    [SerializeField, Tooltip("弾を発射する間隔")]
    private float _turretFireRate;

    [SerializeField, Tooltip("ONの場合高い弾道になる")]
    private bool _highRange = false;

    private float _attackRange;

    float _turretRateTime = 0;
    [SerializeField] GameObject _turretBase;
    [SerializeField] GameObject _turretBarrel;
    Vector3 _enemyBulletDirection;

    [Header("共通設定")]
    [SerializeField] float _maxEnemyLife = 20;
    [SerializeField] WeaponType _weaponType;
    BehaviorType _behaviorType;
    float _enemyLife;
    Rigidbody rb;
    MeshRenderer mesh;
    SpriteRenderer spriteRenderer;
    GameObject _enemyLifeGage;
    GameObject _enemyCanvas;
    Image _gage_image;

    EnemyParam _param;


    // Start is called before the first frame update
    void Start()
    {
        _targetObject = GameObject.FindWithTag("Player");
        _enemyLife = _maxEnemyLife;
        _enemyLifeGage = transform.Find("Canvas/EnemyLifeGageRoot/EnemyLifeGage").gameObject;
        _turretBase = transform.Find("TurretBase").gameObject;
        _turretBarrel = _turretBase.transform.Find("TurretBarrelBase").gameObject;
        _turretMuzzle = _turretBarrel.transform.Find("TurretBarrel/TurretMuzzle").gameObject.transform;

        _enemyCanvas = transform.GetChild(0).gameObject;
        _gage_image = _enemyLifeGage.GetComponent<Image>();
        rb = GetComponent<Rigidbody>();
        _enemyCanvas.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        switch (_weaponType)
        {
            case WeaponType.Cannon:
                Vector3 dirToTarget = _targetObject.transform.position - _turretBase.transform.position;
                Vector3 flatDir = new Vector3(dirToTarget.x, 0f, dirToTarget.z);
                _turretBase.transform.rotation = Quaternion.LookRotation(flatDir);

                Vector3 firingDirection;

                // 常にレートタイムを進める（近距離撃発のためにも必要）
                _turretRateTime += Time.deltaTime;

                // まず直線距離で攻撃範囲内かをチェックし、範囲内かつレート時間を満たしていれば
                // 直接狙い（直線）で発射する
                float straightDistance = Vector3.Distance(_targetObject.transform.position, (_turretMuzzle != null ? _turretMuzzle.position : _turretBase.transform.position));
                if (straightDistance <= _attackRange && _turretRateTime > _turretFireRate && _behaviorType != BehaviorType.Tactical)
                {
                    // 近距離なら単純に標的方向へ直線的に射出する
                    Vector3 origin = _turretMuzzle != null ? _turretMuzzle.position : this.transform.position;
                    Vector3 dirToTargetFull = (_targetObject.transform.position - origin).normalized;
                    Vector3 directVelocity = dirToTargetFull * _projectileSpeed;

                    _turretBarrel.transform.rotation = Quaternion.LookRotation(directVelocity);
                    ThrowingBall(directVelocity);
                    _turretRateTime = 0f;
                }
                else
                {
                    //TryCalculateBallisticVelocity（より正確化）
                    Vector3 origin = _turretMuzzle != null ? _turretMuzzle.position : this.transform.position;

                    // 最初は設定された速度で計算を試みる。届かない場合は速度を増加させて届く速度を探索する。
                    float testSpeed = _projectileSpeed;
                    bool canHit = TryGetVelocity(origin, _targetObject.transform.position, testSpeed, out firingDirection);
                    if (!canHit)
                    {
                        float maxSpeed = Mathf.Max(testSpeed * 3f, testSpeed + 50f);
                        float step = Mathf.Max(1f, testSpeed * 0.2f);
                        for (float s = testSpeed + step; s <= maxSpeed; s += step)
                        {
                            if (TryGetVelocity(origin, _targetObject.transform.position, s, out firingDirection))
                            {
                                testSpeed = s;
                                canHit = true;
                                break;
                            }
                        }
                    }

                    if (canHit)
                    {
                        _enemyBulletDirection = firingDirection;
                        _turretBarrel.transform.rotation = Quaternion.LookRotation(firingDirection);
                        if (_turretRateTime > _turretFireRate)
                        {
                            ThrowingBall(firingDirection);
                            _turretRateTime = 0f;
                        }
                    }
                }
                break;


        }
    }

    private void FixedUpdate()
    {
        
    }
    /// <summary>
    /// 自身から対象への距離を測定し、一定の速度で弾が対象に届く向きベクトルを検索する。
    /// 平たく言うと斜方投射の改造。
    /// </summary>
    /// <param name="origin">射出場所</param>
    /// <param name="target">設定された標的</param>
    /// <param name="speed">射出速度</param>
    /// <param name="velocity">設定された射出速度で弾が対象に届くような向きベクトル</param>
    /// <returns>対象に届くか否かをBool型で返す。ついでにベクトルを出力する。</returns>
    bool TryGetVelocity(Vector3 origin, Vector3 target, float speed, out Vector3 velocity)
    {
        Vector3 toTarget = target - origin;
        Vector3 toTargetXZ = new Vector3(toTarget.x, 0, toTarget.z);
        float y = toTarget.y;
        float xz = toTargetXZ.magnitude;
        //ここまでで自分からターゲットまでの距離を測る。
        // Use positive gravity magnitude for calculations
        float g = Mathf.Abs(Physics.gravity.y);

        float v2 = speed * speed;
        float underSqrt = v2 * v2 - g * (g * xz * xz + 2f * y * v2);

        if (underSqrt < 0f)
        {
            velocity = Vector3.zero;
            return false; // 到達不能
        }

        float sqrt = Mathf.Sqrt(underSqrt);
        // angles (radians)
        float lowAngle = Mathf.Atan2(v2 - sqrt, g * xz); // 低い弾道
        float highAngle = Mathf.Atan2(v2 + sqrt, g * xz); // 高い弾道
        float angle = _highRange ? highAngle : lowAngle;

        // 方向 (XZ平面)
        Vector3 dir = xz > 0.0001f ? toTargetXZ.normalized : Vector3.zero;

        // horizontal and vertical components
        float vx = speed * Mathf.Cos(angle);
        float vy = speed * Mathf.Sin(angle);

        Vector3 horizontal = dir * vx;

        // if target is almost directly above/below, aim straight up/down
        if (dir == Vector3.zero)
        {
            horizontal = Vector3.zero;
            // if target is directly above and within vertical reach, shoot straight up/down
            // vy sign should aim toward target's height
            vy = y > 0f ? Mathf.Abs(vy) : -Mathf.Abs(vy);
        }

        velocity = horizontal + Vector3.up * vy;
        return true;
    }

    /// <summary>
    /// ボールを射出する
    /// </summary>
    private void ThrowingBall(Vector3 shootVector)
    {
        if (_throwingObject != null && _targetObject != null)
        {
            // Ballオブジェクトの生成
            Vector3 spawnPos = _turretMuzzle != null ? _turretMuzzle.position : this.transform.position;
            GameObject ball = Instantiate(_throwingObject, spawnPos, Quaternion.identity);
            Collider collider = ball.GetComponent<Collider>();
            StartCoroutine(SwitchTrigger(0.01f, collider));
            // 射出
            Rigidbody rid = ball.GetComponent<Rigidbody>();
            if (rid != null)
            {
                rid.linearVelocity = shootVector;
            }
        }
        else
        {

            throw new System.Exception("射出するオブジェクトまたは標的のオブジェクトが未設定です。");

        }
    }

    public void Initialize(EnemyParam enemyParam)
    {
        _param = enemyParam;
        _turretFireRate = _param.WeaponFireRate;
        _weaponType = _param.WeaponType;
        _attackRange = _param.AttackRange;
        _behaviorType = _param.BehaviorType;

    }

    IEnumerator SwitchTrigger(float delay, Collider collider)
    {
        yield return new WaitForSeconds(delay);
        collider.enabled = true;
    }



    #region Ballistic/Liner

    [Header("弾道表示")]
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private int _resolution = 30; // 点の数
    [SerializeField] private float _timeStep = 0.1f; // 何秒刻みで点を打つか

    void DrawTrajectory(Vector3 startPos, Vector3 startVelocity)
    {
        List<Vector3> points = new List<Vector3>();

        for (int i = 0; i < _resolution; i++)
        {
            float t = i * _timeStep;
            Vector3 point = startPos + startVelocity * t + 0.5f * Physics.gravity * t * t;
            points.Add(point);
        }

        _lineRenderer.positionCount = points.Count;
        _lineRenderer.SetPositions(points.ToArray());
    }

    #endregion
}
