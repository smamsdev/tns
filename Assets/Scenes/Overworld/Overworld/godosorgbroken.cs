using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class bkuop : MonoBehaviour
{
    [Header("References")]
    public Transform planet;           // Planet center
    public Transform playerCamera;     // Camera to follow player
    public Transform playerSprite;     // Flat sprite to rotate
    public Animator animator;          // Optional
    private Vector2 lookDirection;

    [Header("Movement")]
    public float moveSpeed = 5f;
    public float gravity = 9.81f;

    [Header("Camera")]
    public float cameraDistance = 5f;
    public float cameraHeight = 2f;
    public float cameraRotateSpeed = 50f; // Q/E rotation degrees/sec

    private Rigidbody rb;
    private float currentCameraAngle = 0f;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        if (playerSprite)
            spriteRenderer = playerSprite.GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        ApplyGravity();
        MovePlayer();
        AlignToPlanet();
    }

    void LateUpdate()
    {
        UpdateCamera();
        UpdateBillboard();
    }

    void ApplyGravity()
    {
        Vector3 gravityDir = (planet.position - transform.position).normalized;
        rb.AddForce(gravityDir * gravity, ForceMode.Acceleration);
    }

    void AlignToPlanet()
    {
        Vector3 up = (transform.position - planet.position).normalized;
        transform.rotation = Quaternion.FromToRotation(transform.up, up) * transform.rotation;
    }

    void MovePlayer()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector2 input = new Vector2(h, v);

        // Update animator lookDirection if moving
        if (input.sqrMagnitude > 0.01f)
        {
            lookDirection = input.normalized;
            if (animator)
            {
                animator.SetFloat("lookDirectionX", lookDirection.x);
                animator.SetFloat("lookDirectionY", lookDirection.y);
            }
        }

        if (animator)
            animator.SetFloat("sqrMagnitude", input.sqrMagnitude);

        // Planet up direction (normal)
        Vector3 up = (transform.position - planet.position).normalized;

        // === CAMERA-RELATIVE MOVEMENT ===

        // Flatten camera forward onto the planet surface
        Vector3 camForward = playerCamera.forward;
        camForward = Vector3.ProjectOnPlane(camForward, up).normalized;

        // Camera right axis on the planet surface
        Vector3 camRight = Vector3.Cross(up, camForward).normalized;

        // Movement direction
        Vector3 moveDir = camForward * v + camRight * h;
        if (moveDir.sqrMagnitude > 1f) moveDir.Normalize();

        // Keep gravity part of velocity
        Vector3 gravityVelocity = Vector3.Project(rb.linearVelocity, up);

        // Apply final velocity
        rb.linearVelocity = moveDir * moveSpeed + gravityVelocity;
    }


    void UpdateCamera()
    {
        if (playerCamera == null) return;

        float rotationInput = 0f;
        if (Input.GetKey(KeyCode.Q)) rotationInput = -1f;
        if (Input.GetKey(KeyCode.E)) rotationInput = 1f;

        currentCameraAngle += rotationInput * cameraRotateSpeed * Time.deltaTime;

        Vector3 up = (transform.position - planet.position).normalized;
        Vector3 offset = Quaternion.AngleAxis(currentCameraAngle, up) * (-transform.forward * cameraDistance + up * cameraHeight);

        playerCamera.position = transform.position + offset;
        playerCamera.LookAt(transform.position, up);
    }

    void UpdateBillboard()
    {
        if (!playerSprite || !spriteRenderer || playerCamera == null) return;

        Vector3 up = (transform.position - planet.position).normalized;

        // 1️⃣ Billboard horizontally toward the camera
        Vector3 camForward = playerCamera.forward;
        camForward = Vector3.ProjectOnPlane(camForward, up).normalized;
        playerSprite.forward = camForward;

        // 2️⃣ Sorting based on distance to camera
        float dist = Vector3.Distance(playerCamera.position, transform.position);
        spriteRenderer.sortingOrder = Mathf.RoundToInt(-dist * 1000f); // negative so closer = higher order
    }

}
