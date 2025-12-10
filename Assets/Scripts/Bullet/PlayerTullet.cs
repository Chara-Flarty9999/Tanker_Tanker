using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTullet : MonoBehaviour
{
    [SerializeField, Tooltip("射出するオブジェクトをここに割り当てる")]
    private GameObject _throwingObject;

    [SerializeField, Tooltip("射出する速度")]
    private float _projectileSpeed = 20f;

    [SerializeField, Tooltip("射出する座標")]
    private Transform _turretMuzzle;

    [SerializeField, Tooltip("発射インターバル")]
    private int _turretShotCooldown;

    [SerializeField, Tooltip("照準感度")]
    private float _reticleSensitivity = 5.0f;

    /// <summary>
    /// 高い弾道か低い弾道か。
    /// </summary>
    [SerializeField, Tooltip("ONの場合高い弾道になる")]
    private bool _highRange = false;

    float _turretRateTime = 0;
    [SerializeField] GameObject _turretBase;
    [SerializeField] GameObject _turretBarrel;
    float _enemyLife;
    GameObject _enemyCanvas;
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
        _turretBase.transform.localRotation = Quaternion.Euler(0, _turretBase.transform.localRotation.eulerAngles.y + _inputBuffer.inputReticleHorizontal * _reticleSensitivity, 0);

        float barrelAngle = Mathf.Clamp(_turretBarrel.transform.localRotation.eulerAngles.x + _inputBuffer.inputReticleVertical * _reticleSensitivity, 0, 45);

        _turretBarrel.transform.localRotation =
            Quaternion.Euler(barrelAngle, 0, 0);
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


    /// <summary>
    /// ボールを射出する
    /// </summary>
    private void ThrowingBall(Vector3 shootVector)
    {
        if (_throwingObject != null)
        {
            // Ballオブジェクトの生成
            GameObject ball = Instantiate(_throwingObject, _turretMuzzle.position, Quaternion.identity);
            Collider collider = ball.GetComponent<SphereCollider>();
            StartCoroutine(SwitchTrigger(0.25f, collider));

            // 射出
            Rigidbody rid = ball.GetComponent<Rigidbody>();
            rid.AddForce(shootVector, ForceMode.Impulse);
        }
        else
        {
            throw new System.Exception("射出するオブジェクトまたは標的のオブジェクトが未設定です。");
        }
    }

    IEnumerator SwitchTrigger(float delay, Collider collider)
    {
        yield return new WaitForSeconds(delay);
        collider.enabled = true;
    }

    private void OnDestroy()
    {
        GameManager.leftEnemyBox--;
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
