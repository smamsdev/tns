using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorMovementScript : MovementScript
{
    public Vector2 newPosition;
    public Vector2 previousPosition;

    private void Awake()
    {
        movementSpeed = defaultMovementspeed;
        rigidBody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    private void OnDisable()
    {
        animator.SetFloat("lookDirectionX", 0);
        animator.SetFloat("verticalInput", 0);
    }

    private void Start()
    {
        rigidBody2d.bodyType = RigidbodyType2D.Dynamic;
        scriptedMovement = false;

        if (forceLookDirectionOnLoad != Vector2.zero ) 
        
        { 
            lookDirection = forceLookDirectionOnLoad;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))

        {
            verticalInput = 1 * descendingFactor;
        }

        if (Input.GetKeyUp(KeyCode.I))

        {
            verticalInput = 0;
        }

        if (Input.GetKeyDown(KeyCode.K))

        {
            verticalInput = -1 * descendingFactor;
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
            newPosition = rigidBody2d.position;
            previousPosition = rigidBody2d.position;

            newPosition.x += movementSpeed * horizontalInput * Time.deltaTime;
            newPosition.y += movementSpeed * (verticalInput + sloping) * Time.deltaTime;

            rigidBody2d.MovePosition(newPosition);

            movementDirection = (newPosition - previousPosition);

            if (movementDirection.magnitude > 0)
            {
                animator.SetBool("isMoving", true);
            }

            else
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

            if (verticalInput > 0)
            {
                lookDirection = Vector2.up * descendingFactor;
            }

            if (verticalInput < 0)
            {
                lookDirection = Vector2.down * descendingFactor;
            }

            animator.SetFloat("horizontalInput", movementDirection.x);
            animator.SetFloat("verticalInput", movementDirection.y * descendingFactor);
            animator.SetFloat("lookDirectionX", lookDirection.x);
            animator.SetFloat("lookDirectionY", lookDirection.y);
        }
    }
}
