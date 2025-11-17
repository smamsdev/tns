using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovementScript : MonoBehaviour
{
    public float horizontalInput;
    public float verticalInput;
    public float defaultMovementspeed;
    public float movementSpeed;

    public float sloping;
    public Vector2 forceLookDirectionOnLoad;

    public Animator animator;
    public Vector2 movementDirection;
    public Vector2 lookDirection;
    public int descendingFactor = 1;
    public Rigidbody2D rigidBody2d;
}
