using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private const float rotationSpeed = 2f;

    [Header("Movement")]
    
    [Tooltip("�ړ����̉�����")]
    public float movementAcceleration = 90.0f;

    [SerializeField] private EnemyBullet enemy;
    [SerializeField] private InputBuffer _inputBuffer;

    Rigidbody _parentRigidBody;
    void Start()
    {
        if (transform.parent.TryGetComponent<Rigidbody>(out var rb))
        {
            _parentRigidBody = rb;
        }
    }

    // Update is called once per frame
    void Update()
    {
        var dir = CameraRelativeInput();

        if (!Mathf.Approximately(dir.magnitude, 0f))
        {
            _parentRigidBody.AddForce(dir * movementAcceleration);
        }

        RotateTowardsMovementDir();
    }

    /// <summary>
    /// Movement based off camera facing.
    /// </summary>
    private Vector3 CameraRelativeInput()
    {
        // XZ���ʏ�̃J�����̑O���x�N�g�����擾���܂��B
        Vector3 forward = Camera.main.transform.forward;//direction�����[�J����Ԃ��烏�[���h��Ԃ֕ϊ�����B
        forward.y = 0;
        forward = forward.normalized;

        // �J�����ɑ΂���E�x�N�g���͏�ɑO���x�N�g���ɑ΂��Ē������܂��c�炵��
        Vector3 right = new Vector3(forward.z, 0, -forward.x);//���ʃx�N�g������E�x�N�g��������Ŏ��邼
        Vector3 relativeDirection =
            _inputBuffer.inputHorizontal * right + _inputBuffer.inputVertical * forward;

        // Reduce input for diagonal movement.
        if (relativeDirection.magnitude > 1) { relativeDirection.Normalize(); }

        return relativeDirection;
    }
    private void RotateTowardsMovementDir()
    {
        var moveDir = CameraRelativeInput();

        if (moveDir!= Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, 
                Quaternion.LookRotation(moveDir), 
                Time.deltaTime * rotationSpeed);
        }
    }
}
