using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class VehiclesMovementScript : PlayerMovementScript
{
    public float accelerationDelta;
    public float distance;
    public VehicleInstance vehicleInstance;

    [SerializeField, Range(0f, 1f)]
    private float accelerationFactor;

    Vector2 input;


    public override void FixedUpdateMethod()
    {
        if (vehicleInstance.playerDriving == null)
            return;

        HandleMovement();
        HandleDistanceTravelled();
        HandleLookDirection();
    }

    void HandleMovement()
    {
        if (!FieldEvents.movementLocked)
        {
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");
        }

        bool hasInput = Mathf.Abs(horizontalInput) == 1 || Mathf.Abs(verticalInput) == 1;

        if (hasInput && vehicleInstance.batteryCharge > 0f)
        {
            accelerationFactor += accelerationDelta * Time.fixedDeltaTime;
        }
        else
        {
            accelerationFactor *= 0.98f;
        }

        accelerationFactor = Mathf.Clamp01(accelerationFactor);

        input = new Vector2((horizontalInput * accelerationFactor), (verticalInput * accelerationFactor) + sloping);

        Vector2 newPosition = rigidBody2d.position + input * movementSpeed * Time.fixedDeltaTime;
        rigidBody2d.MovePosition(newPosition);
    }

    void HandleDistanceTravelled()
    {
        delta = (Vector2)transform.position - previousPosition;
        distance = delta.magnitude;
        distanceTravelled += distance;
        previousPosition = transform.position;
    }

    void HandleLookDirection()
    {
        Vector2 inputDir = input;

        float deadZone = 0.01f;
        float bias = 1.5f; // horizontal strength

        // default: keep last known direction
        Vector2 newDir = lookDirection;

        if (inputDir.sqrMagnitude > deadZone)
        {
            inputDir = inputDir.normalized;

            float x = Mathf.Abs(inputDir.x) * bias;
            float y = Mathf.Abs(inputDir.y);

            if (x >= y)
            {
                newDir = new Vector2(Mathf.Sign(inputDir.x), 0);
            }
            else
            {
                newDir = new Vector2(0, Mathf.Sign(inputDir.y));
            }

            lookDirection = newDir;
        }
    }
}
