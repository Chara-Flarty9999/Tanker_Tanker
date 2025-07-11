using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("タレット専用の設定")]
    /// <summary>
    /// 射出するオブジェクト
    /// </summary>
    [SerializeField, Tooltip("射出するオブジェクトをここに割り当てる(Normal以外)")]
    private GameObject _throwingObject;

    /// <summary>
    /// 標的のオブジェクト
    /// </summary>
    [SerializeField, Tooltip("標的のオブジェクトをここに割り当てる(Normal以外)")]
    private GameObject _targetObject;

    /// <summary>
    /// 射出角度
    /// </summary>
    [SerializeField, Tooltip("射出する速度(Turretのみ)")]
    private float _projectileSpeed = 20f;

    /// <summary>
    /// 射出する座標のオブジェクト
    /// </summary>
    [SerializeField, Tooltip("射出する座標(Turretのみ)")]
    private Transform _turretMuzzle;

    /// <summary>
    /// 射出する座標のオブジェクト
    /// </summary>
    [SerializeField, Tooltip("弾を発射する間隔")]
    private int _turretBulletRate;

    /// <summary>
    /// 高い弾道か低い弾道か。
    /// </summary>
    [SerializeField, Tooltip("ONの場合高い弾道になる")]
    private bool _highRange = false;

    float _turretRateTime = 0;
    GameObject _turretBase;
    GameObject _turretBarrel;
    Vector3 _enemyBulletDirection;

    [Header("共通設定")]
    [SerializeField] float _maxEenemyLife = 20;
    [SerializeField] EnemyType _enemyType;
    float _enemyLife;
    Rigidbody rb;
    MeshRenderer mesh;
    SpriteRenderer spriteRenderer;
    GameObject _enemyLifeGage;
    GameObject _enemyCanvas;
    Image _gage_image;


    // Start is called before the first frame update
    void Start()
    {
        _enemyLife = _maxEenemyLife;
        _enemyLifeGage = transform.Find("Canvas/EnemyLifeGageRoot/EnemyLifeGage").gameObject;
        
        if (_enemyType == EnemyType.Turret) // タレット状態の時のみ取得する。
        {
            _turretBase = transform.Find("TurretBase").gameObject;
            _turretBarrel = transform.Find("TurretBase/TurretBarrelBase").gameObject;
        }
        _enemyCanvas = transform.GetChild(0).gameObject;
        _gage_image = _enemyLifeGage.GetComponent<Image>();
        rb = GetComponent<Rigidbody>();
        _enemyCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        switch (_enemyType)
        {
            case EnemyType.Turret:
                Vector3 dirToTarget = _targetObject.transform.position - _turretBase.transform.position;
                Vector3 flatDir = new Vector3(dirToTarget.x, 0f, dirToTarget.z);
                _turretBase.transform.rotation = Quaternion.LookRotation(flatDir);

                Vector3 firingDirection;

                //TryCalculateBallisticVelocity
                if (TryGetVelocity(this.transform.position, _targetObject.transform.position, _projectileSpeed, out firingDirection))
                {
                    // barrelのforwardをfiringDirectionに近づける（X軸のみ変化）
                    Quaternion aimRotation = Quaternion.LookRotation(firingDirection);
                    Vector3 euler = aimRotation.eulerAngles;
                    _enemyBulletDirection = firingDirection;
                    // 回転制限: X軸だけ、YとZを固定
                    _turretBarrel.transform.rotation = Quaternion.LookRotation(firingDirection);

                    if (_turretRateTime == 0)
                    {
                        ThrowingBall(firingDirection);
                    }
                    _turretRateTime += Time.deltaTime;

                    if (_turretRateTime > _turretBulletRate)
                    {
                        _turretRateTime = 0;
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

        float g = Physics.gravity.y;

        float v2 = speed * speed;
        float underSqrt = v2 * v2 - g * (g * xz * xz + 2 * y * v2);

        if (underSqrt < 0)
        {
            velocity = Vector3.zero;
            return false; // 到達不能
        }

        float sqrt = Mathf.Sqrt(underSqrt);
        float lowAngle = Mathf.Atan2(v2 - sqrt, g * xz); // 低い弾道
        float highAngle = Mathf.Atan2(v2 + sqrt, g * xz); // 高い弾道
        float angle = _highRange ? highAngle : lowAngle;

        // direction (XZ plane)
        Vector3 dir = toTargetXZ.normalized;

        Debug.DrawRay(origin, dir, Color.yellow, 2f);

        velocity = dir * -1 * speed * Mathf.Cos(angle) + Vector3.up * speed * Mathf.Sin(angle);

        Debug.DrawRay(origin, velocity, Color.cyan, 2f);
        return true;
    }

    public void DealDamage_Heal(int change_HP)
    {
        _enemyCanvas?.SetActive(true);
        _enemyLife += change_HP;
        _gage_image.fillAmount = _enemyLife / _maxEenemyLife;
        //mesh.material.color = change_HP >= 0 ? new Color(0, 1, 0, 1) : new Color(1, 0, 0, 1);
        if (_enemyLife <= 0)
        {
            Destroy(gameObject, 1f);
        }
        else
        {
            //mesh.material.DOColor(new Color(1, 1, 1), 0.8f);
        }
    }

    /// <summary>
    /// ボールを射出する
    /// </summary>
    private void ThrowingBall(Vector3 shootVector)
    {
        if (_throwingObject != null && _targetObject != null && _enemyType == EnemyType.Turret)
        {
            // Ballオブジェクトの生成
            GameObject ball = Instantiate(_throwingObject, _turretMuzzle.position, Quaternion.identity);

            // 射出
            Rigidbody rid = ball.GetComponent<Rigidbody>();
            rid.linearVelocity = shootVector;
        }
        else
        {
            if (_enemyType != EnemyType.Turret)
            {
                throw new System.Exception("タレットではないため発射できません。");
            }
            else
            {
                throw new System.Exception("射出するオブジェクトまたは標的のオブジェクトが未設定です。");
            }
        }
    }
    private void OnDestroy()
    {
        GameManager.leftEnemyBox--;
    }
    /// <summary>
    /// 敵の状態(敵種類)
    /// </summary>
    enum EnemyType
    {
        Object,
        Turret,
        Wander,
        chase
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
