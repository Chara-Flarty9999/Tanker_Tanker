using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTurret : MonoBehaviour
{
    [Header("�^���b�g�̐ݒ�")]

    [SerializeField, Tooltip("�ˏo����I�u�W�F�N�g�������Ɋ��蓖�Ă�")]
    private GameObject _throwingObject;

    [SerializeField, Tooltip("�ˏo���鑬�x")]
    private float _projectileSpeed = 20f;

    [SerializeField, Tooltip("�ˏo������W")]
    private Transform _turretMuzzle;

    [SerializeField, Tooltip("���˃C���^�[�o��")]
    private int _turretShotCooldown;

    [SerializeField,Tooltip("�Ə����x")]
    private float _reticleSensitivity = 5.0f;

    /// <summary>
    /// �����e�����Ⴂ�e�����B
    /// </summary>
    [SerializeField, Tooltip("ON�̏ꍇ�����e���ɂȂ�")]
    private bool _highRange = false;

    float _turretRateTime = 0;
    [SerializeField] GameObject _turretBase;
    [SerializeField] GameObject _turretBarrel;
    
    [Header("���ʐݒ�")]
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

        // ��]����: X�������AY��Z���Œ�


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
    /// ���g����Ώۂւ̋����𑪒肵�A���̑��x�Œe���Ώۂɓ͂������x�N�g������������B
    /// �����������ƎΕ����˂̉����B
    /// </summary>
    /// <param name="direction">�ˏo����XZ���ʂ̊p�x</param>
    /// <param name="barrelAngleDeg">�o�����̋p</param>
    /// <param name="speed">�ˏo���x</param>
    /// <param name="velocity">�ŏI�I�ȃx�N�g��</param>
    /// <returns>�x�N�g�����o�͂���B</returns>
    void GetVelocity(Vector3 direction, float barrelAngleDeg, float speed, out Vector3 velocity)
    {
        float y = direction.y; // Y�������擾
        float xz = new Vector3(direction.x, 0, direction.z).magnitude;// XZ���ʏ�̐������擾
        float g = Physics.gravity.y;
        float v2 = speed * speed;
        float sqrt = Mathf.Sqrt(v2 * v2 - g * (g * xz * xz + 2 * y * v2));

        direction = new Vector3(direction.x, 0, direction.z).normalized; // Y���𖳎�����XZ���ʏ�̕������擾
        float barrelAngleRad = barrelAngleDeg * Mathf.Deg2Rad; // �p�x�����W�A���ɕϊ� 
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
    /// �{�[�����ˏo����
    /// </summary>
    private void ThrowingBall(Vector3 shootVector)
    {
        if (_throwingObject != null)
        {
            // Ball�I�u�W�F�N�g�̐���
            GameObject ball = Instantiate(_throwingObject, _turretMuzzle.position, Quaternion.identity);

            // �ˏo
            Rigidbody rid = ball.GetComponent<Rigidbody>();
            rid.AddForce(shootVector,ForceMode.Impulse);
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
    /// <summary>
    /// �G�̏��(�G���)
    /// </summary>
    enum EnemyType
    {
        Object,
        Turret,
        Wander,
        chase
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
