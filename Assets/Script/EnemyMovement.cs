using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    
    [Header("Mouse�p�ݒ���e"), SerializeField, Tooltip("�ړ����a")] float wanderRadius = 10f;
    [SerializeField, Tooltip("�ړ���ύX�܂ł̕b��")] float wanderTimer = 3f;
    float _timer;

    Transform _player; // �v���C���[��Transform
    [SerializeField] Transform _movementTarget;
    NavMeshAgent _navAgent;

    EnemyParam _param;
    float _maxSpeed;

    [SerializeField]static event Action<EnemyMovement> _OnEnemyDeath;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _navAgent = GetComponent<NavMeshAgent>();
        _player = GameObject.FindWithTag("Player").transform;
        //_movementTarget.SetParent(transform,true);
    }

    // Update is called once per frame
    void Update()
    {
        _navAgent.speed = _param.MaxMoveSpeed;
        switch (_param.BehaviorType)
        {

            case Enums.BehaviorType.Assault:
                _navAgent.SetDestination(_player.position);
                
                break;

            case Enums.BehaviorType.Tactical:
                Vector3 targetPosition = Vector3.zero;
                if (Vector3.Distance(transform.position, _player.transform.position) > _param.AttackRange)
                {
                    // �v���C���[�Ƃ̋������U���͈͂��傫���ꍇ�A�߂Â�
                    targetPosition = _player.position;
                }
                else if (Vector3.Distance(transform.position, _player.transform.position) < _param.AttackRange - 10)
                {
                    // �v���C���[�Ƃ̋������U���͈͈ȓ��̏ꍇ�A��ނ���
                    targetPosition = transform.position - (_player.position - transform.position).normalized * Vector3.Distance(transform.position, _player.transform.position); // ��ނ��鋗��
                }
                else
                {
                    int prob = UnityEngine.Random.Range(0, 100);
                    if (prob <= 20)
                    {
                        int randomRotate = UnityEngine.Random.Range(-5, 5);
                        _movementTarget.RotateAround(_player.position, Vector3.up,randomRotate);
                        targetPosition = _movementTarget.position;
                    }
                }
                    _navAgent.SetDestination(targetPosition);
                break;

            case Enums.BehaviorType.Mouse:
                _timer += Time.deltaTime;
                if (_timer >= wanderTimer)
                {
                    Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
                }
                break;
        }
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float distance, int layermask)
    {
        Vector3 randDirection = UnityEngine.Random.insideUnitSphere * distance;
        randDirection += origin;

        NavMesh.SamplePosition(randDirection, out NavMeshHit navHit, distance, layermask);

        return navHit.position;
    }

    public void Initialize(EnemyParam enemyParam)
    {
        _param = enemyParam;
        
    }
}
