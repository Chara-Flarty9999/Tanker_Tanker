using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTurret : MonoBehaviour
{
    [Header("タレットの設定")]

    [SerializeField, Tooltip("射出するオブジェクトをここに割り当てる")]
    private GameObject _throwingObject;

    [SerializeField, Tooltip("射出する速度")]
    private float _projectileSpeed = 20f;

    [SerializeField, Tooltip("射出する座標")]
    private Transform _turretMuzzle;

    [SerializeField, Tooltip("発射インターバル")]
    private int _turretShotCooldown;

    [SerializeField,Tooltip("照準感度")]
    private float _reticleSensitivity = 5.0f;

    /// <summary>
    /// 高い弾道か低い弾道か。
    /// </summary>
    [SerializeField, Tooltip("ONの場合高い弾道になる")]
    private bool _highRange = false;

    float _turretRateTime = 0;
    [SerializeField] GameObject _turretBase;
    [SerializeField] GameObject _turretBarrel;
    
    [Header("共通設定")]
    [SerializeField] float _maxEenemyLife = 20;
    //[SerializeField] EnemyType _enemyType;
    float _enemyLife;
    GameObject enemyCanvas;
    Image gage_image;
    Vector3 _firingDirection;
    [SerializeField] private InputBuffer _inputBuffer;


    // Start is called before the first frame update
    void Start()
    {   

    }

    // Update is called once per frame
    void Update()
    {
        GetVelocity(_turretBarrel.transform.forward, _turretBarrel.transform.rotation.eulerAngles.x, _projectileSpeed, out _firingDirection);

        // 回転制限: X軸だけ、YとZを固定


        _turretRateTime += Time.deltaTime;

        if (_inputBuffer.inputAttack)
        {

            if (_turretRateTime > _turretShotCooldown)
            {
                _turretRateTime = 0;
            }

            if (_turretRateTime == 0)
            {
                ThrowingBall(_firingDirection);
            }
        }
        DrawTrajectory(_turretBarrel.transform.position, _firingDirection);
    }

    void FixedUpdate()
    {
        Debug.Log(_inputBuffer.inputReticleHorizontal + " " + _inputBuffer.inputReticleVertical);
        _turretBase.transform.localRotation = Quaternion.Euler(0, _turretBase.transform.localRotation.eulerAngles.y + _inputBuffer.inputReticleHorizontal * _reticleSensitivity, 0);

        float barrelAngle = _turretBarrel.transform.localRotation.eulerAngles.x + _inputBuffer.inputReticleVertical * _reticleSensitivity;

        _turretBarrel.transform.localRotation = 
            Quaternion.Euler(Mathf.Clamp(barrelAngle, 0, 180), 0, 0);
    }
    /// <summary>
    /// 自身から対象への距離を測定し、一定の速度で弾が対象に届く向きベクトルを検索する。
    /// 平たく言うと斜方投射の改造。
    /// </summary>
    /// <param name="direction">射出するXZ平面の角度</param>
    /// <param name="barrelAngleDeg">バレルの仰角</param>
    /// <param name="speed">射出速度</param>
    /// <param name="velocity">最終的なベクトル</param>
    /// <returns>ベクトルを出力する。</returns>
    void GetVelocity(Vector3 direction, float barrelAngleDeg, float speed, out Vector3 velocity)
    {
        float y = direction.y; // Y成分を取得
        float xz = new Vector3(direction.x, 0, direction.z).magnitude;// XZ平面上の成分を取得
        float g = Physics.gravity.y;
        float v2 = speed * speed;
        float sqrt = Mathf.Sqrt(v2 * v2 - g * (g * xz * xz + 2 * y * v2));

        direction = new Vector3(direction.x, 0, direction.z).normalized; // Y軸を無視してXZ平面上の方向を取得
        float barrelAngleRad = barrelAngleDeg * Mathf.Deg2Rad; // 角度をラジアンに変換 
        Debug.DrawRay(transform.position, direction, Color.yellow, 2f);

        velocity = Mathf.Cos(barrelAngleRad) * speed * direction * -1 + Vector3.up * speed * Mathf.Sin(barrelAngleRad);

        Debug.DrawRay(_turretMuzzle.transform.position, velocity, Color.cyan, 2f);
    }

    public void DealDamage_Heal(int change_HP)
    {
        enemyCanvas?.SetActive(true);
        _enemyLife += change_HP;
        gage_image.fillAmount = _enemyLife / _maxEenemyLife;
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
        if (_throwingObject != null)
        {
            // Ballオブジェクトの生成
            GameObject ball = Instantiate(_throwingObject, _turretMuzzle.position, Quaternion.identity);

            // 射出
            Rigidbody rid = ball.GetComponent<Rigidbody>();
            rid.AddForce(shootVector,ForceMode.Impulse);
        }
        else
        {
                throw new System.Exception("射出するオブジェクトまたは標的のオブジェクトが未設定です。");
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
