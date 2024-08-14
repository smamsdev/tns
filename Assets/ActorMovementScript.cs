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

    void FixedUpdate()
    {
        {
            newPosition = actorRigidBody2d.position;
            previousPosition = actorRigidBody2d.position;

            newPosition.x += movementSpeed * horizontalInput * Time.deltaTime;
            newPosition.y += movementSpeed * verticalInput * Time.deltaTime;

            actorRigidBody2d.MovePosition(newPosition);

            movementDirection = (newPosition - previousPosition);
        }
    }
}
