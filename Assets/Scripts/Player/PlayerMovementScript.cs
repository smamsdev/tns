using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovementScript : MovementScript
{
    public Rigidbody2D playerRigidBody2d;

    public bool isWalkwayBoost;
    public bool isDescending = false;

    public bool movementLocked = false;
    public Vector2 newPosition;
    public Vector2 previousPosition;

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
        movementSpeed = defaultMovementspeed;
        scriptedMovement = false;
        FieldEvents.movementSpeedMultiplier = 1;
        animator.SetFloat("sceneEntryDirection", lookDirection.x);
        isAscending = Vector2.one;
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
        if (!movementLocked)

        {
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical") * isAscending.y;
        }

            newPosition = playerRigidBody2d.position;
            previousPosition = playerRigidBody2d.position;

            newPosition.x += movementSpeed * horizontalInput * Time.deltaTime;
            newPosition.y += movementSpeed * verticalInput * Time.deltaTime;

            playerRigidBody2d.MovePosition(newPosition);

            movementDirection = (newPosition - previousPosition);

        if (movementDirection.magnitude > 0)
        {
            animator.SetBool("isMoving", true);
        }

        else if (movementDirection.magnitude < 0.015f)
        {
            animator.SetBool("isMoving", false);
        }

        if (horizontalInput > 0)
            {
                lookDirection = Vector2.right;
            }

            if (horizontalInput < 0)
            {
                lookDirection = Vector2.left;
            }

            if (verticalInput > 0 && horizontalInput == 0)
            {
                lookDirection = Vector2.up;
            }

            if (verticalInput < 0)
            {
                lookDirection = Vector2.down;
            }

        animator.SetFloat("horizontalInput", movementDirection.x);
        animator.SetFloat("verticalInput", movementDirection.y * isAscending.y);
        animator.SetFloat("lookDirectionX", lookDirection.x);
        animator.SetFloat("lookDirectionY", lookDirection.y * isAscending.y);
    }

    public void LockPlayerMovement()

    {
        movementLocked = true;
        horizontalInput = 0;
        verticalInput = 0;

        //Debug.Log("locked");
    }

    public void UnlockPlayerMovement()

    {
        movementLocked = false;

        //Debug.Log("unlocked");
    }
}
