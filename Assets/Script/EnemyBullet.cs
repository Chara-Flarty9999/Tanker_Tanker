using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class EnemyBullet : MonoBehaviour
{
    [Header("�^���b�g��p�̐ݒ�")]
    [SerializeField, Tooltip("�ˏo����I�u�W�F�N�g�������Ɋ��蓖�Ă�(Normal�ȊO)")]
    private GameObject _throwingObject;

    [SerializeField, Tooltip("�W�I�̃I�u�W�F�N�g�������Ɋ��蓖�Ă�(Normal�ȊO)")]
    private GameObject _targetObject;

    [SerializeField, Tooltip("�ˏo���鑬�x(Turret�̂�)")]
    private float _projectileSpeed = 20f;

    [SerializeField, Tooltip("�ˏo������W(Turret�̂�)")]
    private Transform _turretMuzzle;

    [SerializeField, Tooltip("�e�𔭎˂���Ԋu")]
    private float _turretBulletRate;

    [SerializeField, Tooltip("ON�̏ꍇ�����e���ɂȂ�")]
    private bool _highRange = false;

    float _turretRateTime = 0;
    [SerializeField] GameObject _turretBase;
    [SerializeField] GameObject _turretBarrel;
    Vector3 _enemyBulletDirection;

    [Header("���ʐݒ�")]
    [SerializeField] float _maxEnemyLife = 20;
    [SerializeField] EnemyType _enemyType;
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

        _enemyCanvas = transform.GetChild(0).gameObject;
        _gage_image = _enemyLifeGage.GetComponent<Image>();
        rb = GetComponent<Rigidbody>();
        _enemyCanvas.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        switch (_enemyType)
        {
            case EnemyType.Cannon:
                Vector3 dirToTarget = _targetObject.transform.position - _turretBase.transform.position;
                Vector3 flatDir = new Vector3(dirToTarget.x, 0f, dirToTarget.z);
                _turretBase.transform.rotation = Quaternion.LookRotation(flatDir);

                Vector3 firingDirection;

                //TryCalculateBallisticVelocity
                if (TryGetVelocity(this.transform.position, _targetObject.transform.position, _projectileSpeed, out firingDirection))
                {
                    // barrel��forward��firingDirection�ɋ߂Â���iX���̂ݕω��j
                    Quaternion aimRotation = Quaternion.LookRotation(firingDirection);
                    Vector3 eular = aimRotation.eulerAngles;
                    _enemyBulletDirection = firingDirection;
                    // ��]����: X�������AY��Z���Œ�
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
    /// ���g����Ώۂւ̋����𑪒肵�A���̑��x�Œe���Ώۂɓ͂������x�N�g������������B
    /// �����������ƎΕ����˂̉����B
    /// </summary>
    /// <param name="origin">�ˏo�ꏊ</param>
    /// <param name="target">�ݒ肳�ꂽ�W�I</param>
    /// <param name="speed">�ˏo���x</param>
    /// <param name="velocity">�ݒ肳�ꂽ�ˏo���x�Œe���Ώۂɓ͂��悤�Ȍ����x�N�g��</param>
    /// <returns>�Ώۂɓ͂����ۂ���Bool�^�ŕԂ��B���łɃx�N�g�����o�͂���B</returns>
    bool TryGetVelocity(Vector3 origin, Vector3 target, float speed, out Vector3 velocity)
    {
        Vector3 toTarget = target - origin;
        Vector3 toTargetXZ = new Vector3(toTarget.x, 0, toTarget.z);
        float y = toTarget.y;
        float xz = toTargetXZ.magnitude;
        //�����܂łŎ�������^�[�Q�b�g�܂ł̋����𑪂�B

        float g = Physics.gravity.y;

        float v2 = speed * speed;
        float underSqrt = v2 * v2 - g * (g * xz * xz + 2 * y * v2);

        if (underSqrt < 0)
        {
            velocity = Vector3.zero;
            return false; // ���B�s�\
        }

        float sqrt = Mathf.Sqrt(underSqrt);
        float lowAngle = Mathf.Atan2(v2 - sqrt, g * xz); // �Ⴂ�e��
        float highAngle = Mathf.Atan2(v2 + sqrt, g * xz); // �����e��
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
        _gage_image.fillAmount = _enemyLife / _maxEnemyLife;
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
    /// �{�[�����ˏo����
    /// </summary>
    private void ThrowingBall(Vector3 shootVector)
    {
        if (_throwingObject != null && _targetObject != null)
        {
            // Ball�I�u�W�F�N�g�̐���
            GameObject ball = Instantiate(_throwingObject, _turretMuzzle.position, Quaternion.identity);
            Collider collider = ball.GetComponent<SphereCollider>();
            StartCoroutine(SwitchTrigger(0.25f,collider));
            // �ˏo
            Rigidbody rid = ball.GetComponent<Rigidbody>();
            rid.linearVelocity = shootVector;
        }
        else
        {

            throw new System.Exception("�ˏo����I�u�W�F�N�g�܂��͕W�I�̃I�u�W�F�N�g�����ݒ�ł��B");

        }
    }
    private void OnDestroy()
    {
        GameManager.leftEnemyBox--;
    }
    public void Initialize(EnemyParam enemyParam)
    {
        _param = enemyParam;
        _turretBulletRate = _param.WeaponFireRate;
    }
    /// <summary>
    /// �e�̏��
    /// </summary>
    enum EnemyType
    {
        Cannon,
        MachineGun,
        Wander,
        chase
    }

    IEnumerator SwitchTrigger(float delay, Collider collider)
    {
        yield return new WaitForSeconds(delay);
        collider.enabled = true;
    }



    #region Ballistic/Liner

    [Header("�e���\��")]
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private int _resolution = 30; // �_�̐�
    [SerializeField] private float _timeStep = 0.1f; // ���b���݂œ_��ł�

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
