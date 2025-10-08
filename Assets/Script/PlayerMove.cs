using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private const float rotationSpeed = 2f;

    [Header("Movement")]
    
    [Tooltip("移動時の加速力")]
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
        // XZ平面上のカメラの前方ベクトルを取得します。
        Vector3 forward = Camera.main.transform.forward;//directionをローカル空間からワールド空間へ変換する。
        forward.y = 0;
        forward = forward.normalized;

        // カメラに対する右ベクトルは常に前方ベクトルに対して直交します…らしい
        Vector3 right = new Vector3(forward.z, 0, -forward.x);//正面ベクトルから右ベクトルがこれで取れるぞ
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
