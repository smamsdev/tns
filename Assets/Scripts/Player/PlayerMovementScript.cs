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
        scriptedMovement = false;
        FieldEvents.movementSpeedMultiplier = 1;
        isReversing = Vector2.one;
        rigidBody2d.bodyType = RigidbodyType2D.Dynamic;
        movementLocked = FieldEvents.movementLocked;
        distanceTravelled = 0;
    }

    private void Update()
    {
        if (FieldEvents.isMovementSpeedMultiplier)
        {
            movementSpeed = defaultMovementspeed * FieldEvents.movementSpeedMultiplier;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))

        {
            FieldEvents.movementSpeedMultiplier = 3;
            FieldEvents.isMovementSpeedMultiplier = true;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))

        {
            FieldEvents.movementSpeedMultiplier = 1;
            movementSpeed = defaultMovementspeed;
            FieldEvents.isMovementSpeedMultiplier = false;
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
            verticalInput = Input.GetAxis("Vertical") * isReversing.y + sloping;
        }

        Vector2 newPosition = rigidBody2d.position;
        newPosition.x += movementSpeed * horizontalInput * Time.deltaTime;
        newPosition.y += movementSpeed * verticalInput * Time.deltaTime;

        rigidBody2d.MovePosition(newPosition);

        delta = rigidBody2d.position - previousRigidPosition;

        if (delta.sqrMagnitude > 0.0001f)
            distanceTravelled += delta.magnitude;

        movementDirection = newPosition - previousRigidPosition;

        if (movementDirection.sqrMagnitude > 0.0001f)
            animator.SetBool("isMoving", true);
        else
            animator.SetBool("isMoving", false);

        if (verticalInput > 0 && horizontalInput == 0)
            lookDirection.y = 1 * isReversing.y;
        else if (verticalInput < 0)
            lookDirection.y = -1 * isReversing.y;

        if (horizontalInput > 0)
        {
            lookDirection.x = 1 * isReversing.x;
            lookDirection.y = 0;
        }
        else if (horizontalInput < 0)
        {
            lookDirection.x = -1 * isReversing.x;
            lookDirection.y = 0;
        }

        animator.SetFloat("horizontalInput", horizontalInput * isReversing.x);
        animator.SetFloat("verticalInput", verticalInput * isReversing.y);
        animator.SetFloat("lookDirectionX", lookDirection.x);
        animator.SetFloat("lookDirectionY", lookDirection.y);

        previousRigidPosition = rigidBody2d.position;
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
