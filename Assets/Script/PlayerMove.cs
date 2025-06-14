using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    [Header("Movement")]
    [Tooltip("ˆÚ“®Žž‚Ì‰Á‘¬—Í")]public float movementAcceleration = 90.0f;
    public float runSpeed = 6f;
    private readonly float rotationSpeed = 10f;
    [SerializeField] private Enemy enemy;
    [HideInInspector] public Vector3 currentVelocity;

    [HideInInspector] public bool inputAttack;
    [HideInInspector] public bool inputJump;
    [HideInInspector] public float inputHorizontal = 0;
    [HideInInspector] public float inputVertical = 0;
    [HideInInspector] public float inputReticleHorizontal = 0;
    [HideInInspector] public float inputReticleVertical = 0;
    public Vector3 moveInput { get { return CameraRelativeInput(inputHorizontal, inputVertical); } }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        FindAnyObjectByType(typeof(PlayerMove));
    }

    // Update is called once per frame
    void Update()
    {
        currentVelocity = Vector3.MoveTowards(currentVelocity, moveInput * runSpeed, movementAcceleration * Time.deltaTime);
    }

    /// <summary>
    /// Movement based off camera facing.
    /// </summary>
    private Vector3 CameraRelativeInput(float inputX, float inputZ)
    {
        // Forward vector relative to the camera along the x-z plane.  
        Vector3 forward = Camera.main.transform.TransformDirection(Vector3.forward);
        forward.y = 0;
        forward = forward.normalized;

        // Right vector relative to the camera always orthogonal to the forward vector.
        Vector3 right = new Vector3(forward.z, 0, -forward.x);
        Vector3 relativeVelocity = inputHorizontal * right + inputVertical * forward;

        // Reduce input for diagonal movement.
        if (relativeVelocity.magnitude > 1) { relativeVelocity.Normalize(); }

        return relativeVelocity;
    }
    private void Inputs()
    {
        inputAttack = Input.GetButtonDown("Fire2");
        inputJump = Input.GetButtonDown("Jump");
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        inputVertical = Input.GetAxisRaw("Vertical");
        inputReticleHorizontal = Input.GetAxisRaw("Reticle Horizontal");
        inputReticleVertical = Input.GetAxisRaw("Reticle Vertical");
    }
}
