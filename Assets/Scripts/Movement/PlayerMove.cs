using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private const float rotationSpeed = 2f;

    [Header("Movement")]
    [Tooltip("移動時の加速力")]
    public float movementAcceleration = 90.0f;

    [SerializeField] private InputBuffer _inputBuffer;

    Rigidbody _parentRigidBody;
    Transform _cameraTransform;

    void Start()
    {
        if (transform.parent != null && transform.parent.TryGetComponent<Rigidbody>(out var rb))
        {
            _parentRigidBody = rb;
        }

        // Cache camera transform to avoid Camera.main lookup every frame
        if (Camera.main != null)
        {
            _cameraTransform = Camera.main.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Rotation and visual-related updates should run in Update for smoothness
        RotateTowardsMovementDir();

        // Keep camera cache up-to-date in case camera is created at runtime
        if (_cameraTransform == null && Camera.main != null)
        {
            _cameraTransform = Camera.main.transform;
        }
    }

    // Physics updates should happen in FixedUpdate to be independent from frame rate / window size
    void FixedUpdate()
    {
        if (_parentRigidBody == null || _inputBuffer == null)
            return;

        var dir = CameraRelativeInput();

        if (dir.sqrMagnitude > 0f)
        {
            // Apply acceleration in FixedUpdate using ForceMode.Acceleration so behavior is stable
            _parentRigidBody.AddForce(dir * movementAcceleration, ForceMode.Acceleration);
        }
    }

    /// <summary>
    /// Movement based off camera facing.
    /// </summary>
    private Vector3 CameraRelativeInput()
    {
        if (_cameraTransform == null)
            return Vector3.zero;

        // XZ平面上のカメラの前方ベクトルを取得します。
        Vector3 forward = _cameraTransform.forward; // directionをローカル空間からワールド空間へ変換する。
        forward.y = 0;
        forward = forward.normalized;

        // カメラに対する右ベクトルは常に前方ベクトルに対して直交します
        Vector3 right = new Vector3(forward.z, 0, -forward.x);
        Vector3 relativeDirection =
            _inputBuffer.inputHorizontal * right + _inputBuffer.inputVertical * forward;

        // Reduce input for diagonal movement.
        if (relativeDirection.magnitude > 1) { relativeDirection.Normalize(); }

        return relativeDirection;
    }
    private void RotateTowardsMovementDir()
    {
        var moveDir = CameraRelativeInput();

        if (moveDir != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(moveDir),
                Time.deltaTime * rotationSpeed);
        }
    }
}
