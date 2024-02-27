using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovementScript : MonoBehaviour
{

    public Rigidbody2D playerRigidBody2d;

    public Vector2 playerPosition;

    float myHorizontalInput;
    float myVerticallInput;

    public bool isWalkwayBoost;
    public bool isDescending = false;

    public float movementSpeed = 1.75f;
    public float baseMovementSpeed;

    public bool movementLocked = false;

    public GameObject playerObject;

    private void OnEnable()
    {
        CombatEvents.LockPlayerMovement += LockPlayerMovement;
        CombatEvents.UnlockPlayerMovement += UnlockPlayerMovement;
        FieldEvents.IsWalkwayBoost += IsWalkwayBoost;
    }

    private void OnDisable()
    {
        CombatEvents.LockPlayerMovement -= LockPlayerMovement;
        CombatEvents.UnlockPlayerMovement -= UnlockPlayerMovement;
        FieldEvents.IsWalkwayBoost -= IsWalkwayBoost;
    }



    private void Awake()
    {
        playerPosition = playerRigidBody2d.position;
        playerRigidBody2d = GetComponent<Rigidbody2D>();
        baseMovementSpeed = movementSpeed;

        FieldEvents.lookDirection = new Vector2(1, 0);
    }

    void FixedUpdate()
    {
        if (!movementLocked) 
        {
        Vector2 movePosition = playerRigidBody2d.position;
        myHorizontalInput = Input.GetAxis("Horizontal");
        myVerticallInput = Input.GetAxis("Vertical");

        movePosition.x = movePosition.x + movementSpeed * myHorizontalInput * Time.deltaTime;
        movePosition.y = movePosition.y + movementSpeed * myVerticallInput * Time.deltaTime;

        playerRigidBody2d.MovePosition(movePosition);
        playerPosition = movePosition;
        } 
    }

    private void Update()
    {
        Vector2 move = new Vector2(myHorizontalInput, myVerticallInput);

        if (myHorizontalInput > 0)
        {
            FieldEvents.lookDirection = Vector2.right;
        }

        if (myHorizontalInput < 0)
        {
            FieldEvents.lookDirection = Vector2.left;
        }

        if (myVerticallInput > 0)
        {
            FieldEvents.lookDirection = Vector2.up;
        }

        if (myVerticallInput < 0)
        {
            FieldEvents.lookDirection = Vector2.down;
        }

    }

    public void LockPlayerMovement()

    {
        playerPosition = playerObject.transform.position;
        movementLocked = true;
       // Debug.Log("locked");
    }

    public void UnlockPlayerMovement()

    {
        playerPosition = playerObject.transform.position;
        movementLocked = false;
    }

    void IsWalkwayBoost(bool _isWalkwayBoost, float _speedBonus)

    {
        isWalkwayBoost = _isWalkwayBoost;

        if (isWalkwayBoost)

        {
            float SpeedBonus = _speedBonus;

            float boostedMovementSpeed = baseMovementSpeed + (baseMovementSpeed * SpeedBonus);

            movementSpeed = boostedMovementSpeed;

        }

        if (!isWalkwayBoost)

        {
            movementSpeed = baseMovementSpeed;          
        }

  
    }




}
