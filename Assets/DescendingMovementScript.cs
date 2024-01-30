using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DescendingMovementScript : MonoBehaviour
{
    float myHorizontalInput;
    float myVerticallInput;
    public EdgeCollider2D edgeCollider;

    [SerializeField] PlayerMovementScript playerMovementScript;

    public float movementSpeed = 1.75f;
    public Vector2 playerPosition;
    public Rigidbody2D playerRigidBody2d;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" )

        {
            CombatEvents.LockPlayerMovement?.Invoke();
            playerMovementScript.isDescending = true;
            this.GetComponent<Collider2D>().enabled = false;
            edgeCollider.enabled = true;
        }

    }

    void FixedUpdate()
    {
        if (playerMovementScript.isDescending)
        {
            Vector2 movePosition = playerRigidBody2d.position;
            myHorizontalInput = Input.GetAxis("Horizontal");
            myVerticallInput = Input.GetAxis("Vertical");

            movePosition.x = movePosition.x + movementSpeed * myHorizontalInput * Time.deltaTime;
            movePosition.y = movePosition.y + movementSpeed * -myVerticallInput * Time.deltaTime;

            playerRigidBody2d.MovePosition(movePosition);
            playerPosition = movePosition;

            Debug.Log("desc triggered");
        }
    }

}
