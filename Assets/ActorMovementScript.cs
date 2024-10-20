using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorMovementScript : MovementScript
{
    public Rigidbody2D actorRigidBody2d;
    public Vector2 newPosition;
    public Vector2 previousPosition;
    public Vector2 forceLookDirectionOnLoad;

    private void Awake()
    {
        actorRigidBody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        movementSpeed = defaultMovementspeed;
        scriptedMovement = false;
        isAscending = Vector2.one;

        if (forceLookDirectionOnLoad != Vector2.zero ) 
        
        { 
            lookDirection = forceLookDirectionOnLoad;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))

        {
            verticalInput = 1 * isAscending.y;
        }

        if (Input.GetKeyUp(KeyCode.I))

        {
            verticalInput = 0;
        }

        if (Input.GetKeyDown(KeyCode.K))

        {
            verticalInput = -1 * isAscending.y;
        }

        if (Input.GetKeyUp(KeyCode.K))

        {
            verticalInput = 0;
        }

        if (Input.GetKeyDown(KeyCode.J))

        {
            horizontalInput = -1;
        }

        if (Input.GetKeyUp(KeyCode.J))

        {
            horizontalInput = 0;
        }
        if (Input.GetKeyDown(KeyCode.L))

        {
            horizontalInput = 1;
        }

        if (Input.GetKeyUp(KeyCode.L))

        {
            horizontalInput = 0;
        }
    }

    void FixedUpdate()
    {
        {
            newPosition = actorRigidBody2d.position;
            previousPosition = actorRigidBody2d.position;

            newPosition.x += movementSpeed * horizontalInput * Time.deltaTime;
            newPosition.y += movementSpeed * (verticalInput + sloping) * Time.deltaTime;

            actorRigidBody2d.MovePosition(newPosition);

            movementDirection = (newPosition - previousPosition);

            if (movementDirection.magnitude > 0)
            {
                //actorRigidBody2d.bodyType = RigidbodyType2D.Dynamic;
                animator.SetBool("isMoving", true);
            }

            else
            {
                //actorRigidBody2d.bodyType = RigidbodyType2D.Kinematic;
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

            if (verticalInput > 0)
            {
                lookDirection = Vector2.up;
            }

            if (verticalInput < 0)
            {
                lookDirection = Vector2.down;
            }

            animator.SetFloat("horizontalInput", movementDirection.x);
            animator.SetFloat("verticalInput", movementDirection.y);
            animator.SetFloat("lookDirectionX", lookDirection.x);
            animator.SetFloat("lookDirectionY", lookDirection.y);
        }
    }
}
