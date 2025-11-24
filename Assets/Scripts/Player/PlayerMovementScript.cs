using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovementScript : MovementScript
{
    public Vector2 newPosition;
    public Vector2 previousPosition;
    public bool movementLocked;
    public float distanceTravelled;

    public float previousPositionmag;
    public float newPositionmag;
    private Vector2 previousRigidPosition;
    public Vector2 delta;

    private void OnEnable()
    {
        CombatEvents.LockPlayerMovement += LockPlayerMovement;
        CombatEvents.UnlockPlayerMovement += UnlockPlayerMovement;
    }

    private void OnDisable()
    {
        CombatEvents.LockPlayerMovement -= LockPlayerMovement;
        CombatEvents.UnlockPlayerMovement -= UnlockPlayerMovement;
    }

    private void Start()
    {
        previousRigidPosition = rigidBody2d.position;
        movementSpeed = defaultMovementspeed;
        rigidBody2d.bodyType = RigidbodyType2D.Dynamic;
        movementLocked = FieldEvents.movementLocked;
        distanceTravelled = 0;
    }

    private void OnValidate()
    {
        // Prevent NaN from ever sticking to the serialized value
        if (float.IsNaN(lookDirection.x) || float.IsNaN(lookDirection.y))
        {
            lookDirection = Vector2.right;
            Debug.Log($"{name}: lookDirection was NaN, reset to Vector2.right");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))

        {
            movementSpeed = defaultMovementspeed * 4;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))

        {
            movementSpeed = defaultMovementspeed;
        }

        if (Input.GetKeyDown(KeyCode.KeypadPlus))

        {
            Time.timeScale = 10;
        }

        if (Input.GetKeyUp(KeyCode.KeypadPlus))

        {
            Time.timeScale = 1;
        }
    }

    void FixedUpdate()
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

        if (input.sqrMagnitude > 0.01f)
        {
            lookDirection = input.normalized;
            animator.SetFloat("lookDirectionX", lookDirection.x);
            animator.SetFloat("lookDirectionY", lookDirection.y);
        }

        animator.SetFloat("sqrMagnitude", input.sqrMagnitude);
    }

    public void LockPlayerMovement()
    {
        FieldEvents.movementLocked = true;
        horizontalInput = 0;
        verticalInput = 0;
        movementLocked = FieldEvents.movementLocked;

        //Debug.Log("locked");
    }

    public void UnlockPlayerMovement()
    {
        if (!FieldEvents.isDialogueActive)
        {
            FieldEvents.movementLocked = false;
            movementLocked = FieldEvents.movementLocked;

           // Debug.Log("unlocked");
        }
    }
}
