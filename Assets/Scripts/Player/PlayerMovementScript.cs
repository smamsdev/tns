using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovementScript : MovementScript
{
    public Rigidbody2D playerRigidBody2d;

    public Vector2 playerPosition; //WHAT WERE THESE FOR?

    public bool isWalkwayBoost;
    public bool isDescending = false;

    public bool movementLocked = false;

    public Vector2 isAscending;

    public Animator animator;

    public Vector2 newPosition;
    public Vector2 previousPosition;
    public Vector2 movementDirection;

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
        animator.SetFloat("sceneEntryDirection", FieldEvents.lookDirection.x);
        isAscending = Vector2.one;
    }

    private void Update()
    {

       // movementSpeed = defaultMovementspeed * FieldEvents.movementSpeedMultiplier;

        if (Input.GetKeyDown(KeyCode.LeftShift))

        {
            FieldEvents.movementSpeedMultiplier = 3;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))

        {
            FieldEvents.movementSpeedMultiplier = 1;
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

            animator.SetFloat("horizontalInput", movementDirection.x);
            animator.SetFloat("verticalInput", movementDirection.y);


            if (horizontalInput > 0)
            {
                FieldEvents.lookDirection = Vector2.right;
            }

            if (horizontalInput < 0)
            {
                FieldEvents.lookDirection = Vector2.left;
            }

            if (verticalInput > 0)
            {
                FieldEvents.lookDirection = Vector2.up;
            }

            if (verticalInput < 0)
            {
                FieldEvents.lookDirection = Vector2.down;
            }
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
