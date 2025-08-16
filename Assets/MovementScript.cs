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
    public Vector2 forceLookDirectionOnLoad;

    public Animator animator;
    public Vector2 movementDirection;
    public Vector2 lookDirection;
    public Vector2 isReversing;

    private void OnDisable()
    {
        movementDirection = Vector2.zero;
        lookDirection = Vector2.zero;
        animator.SetFloat("lookDirectionX", 0);
        animator.SetFloat("verticalInput", 0);
    }
}
