using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovementScript : MonoBehaviour
{
    public float horizontalInput;
    public float verticalInput;
    public float defaultMovementspeed;
    public float movementSpeed;
    public bool scriptedMovement;
    public float sloping;

    public Animator animator;
    public Vector2 movementDirection;
    public Vector2 lookDirection;
    public Vector2 isReversing;
}
