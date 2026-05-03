using UnityEngine;

public class VehiclesMovementScript : PlayerMovementScript
{
    public override void FixedUpdateMethod()
    {
        if (!FieldEvents.movementLocked)
        {
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");
        }

        Vector2 input = new Vector2(horizontalInput, verticalInput + sloping);

        Vector2 newPosition = rigidBody2d.position + input * movementSpeed * Time.fixedDeltaTime;
        rigidBody2d.MovePosition(newPosition);

        delta = rigidBody2d.position - previousRigidPosition;
        distanceTravelled += delta.magnitude;
        previousRigidPosition = rigidBody2d.position;

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
