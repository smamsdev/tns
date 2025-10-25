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

    private void Start()
    {
        rigidBody2d.bodyType = RigidbodyType2D.Dynamic;

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
        Vector2 input = new Vector2(horizontalInput, verticalInput + sloping);

        Vector2 newPosition = rigidBody2d.position + input * movementSpeed * Time.fixedDeltaTime;
        rigidBody2d.MovePosition(newPosition);

        if (input.sqrMagnitude > 0.001f)
        {
            lookDirection = input.normalized;
        }

        animator.SetFloat("sqrMagnitude", input.sqrMagnitude);
        animator.SetFloat("lookDirectionX", lookDirection.x);
        animator.SetFloat("lookDirectionY", lookDirection.y);
    }
}
