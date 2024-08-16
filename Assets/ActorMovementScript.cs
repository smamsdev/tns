using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorMovementScript : MovementScript
{
    Rigidbody2D actorRigidBody2d;
    Vector2 isAscending;
    public Vector2 newPosition;
    public Vector2 previousPosition;
    public Vector2 movementDirection;

    public Animator actorAnimator;

    private void Awake()
    {
        actorRigidBody2d = GetComponent<Rigidbody2D>();
        actorAnimator = GetComponent<Animator>();
    }

    private void Start()
    {
        movementSpeed = defaultMovementspeed;
        scriptedMovement = false;
        isAscending = Vector2.one;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))

        {
            verticalInput = 1;
        }

        if (Input.GetKeyUp(KeyCode.I))

        {
            verticalInput = 0;
        }

        if (Input.GetKeyDown(KeyCode.K))

        {
            verticalInput = -1;
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
            newPosition.y += movementSpeed * verticalInput * Time.deltaTime;

            actorRigidBody2d.MovePosition(newPosition);

            movementDirection = (newPosition - previousPosition);

            actorAnimator.SetFloat("horizontalInput", movementDirection.x);
            actorAnimator.SetFloat("verticalInput", movementDirection.y);
        }
    }
}
