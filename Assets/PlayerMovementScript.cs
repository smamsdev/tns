using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovementScript : MonoBehaviour
{

    Rigidbody2D playerRigidBody2d;
    public Vector2 playerPosition;

    float myHorizontalInput;
    float myVerticallInput;

    public float movementSpeed;

    private void Start()
    {
        playerRigidBody2d = GetComponent<Rigidbody2D>();

    }


    void FixedUpdate()
    {

        myHorizontalInput = Input.GetAxis("Horizontal");
        myVerticallInput = Input.GetAxis("Vertical");


        playerPosition = GetComponent<Rigidbody2D>().position;

        playerPosition.x = playerPosition.x + movementSpeed * myHorizontalInput * Time.deltaTime;
        playerPosition.y = playerPosition.y + movementSpeed * myVerticallInput * Time.deltaTime;

        GetComponent<Rigidbody2D>().MovePosition(playerPosition);

        
    }
}
