using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovementScript : MonoBehaviour
{

    public Rigidbody2D playerRigidBody2d;

    public Vector2 playerPosition;

    float horizontalInput;
    float verticalInput;
    float horizontalInputRaw;
    float verticalInputRaw;

    public bool isWalkwayBoost;
    public bool isDescending = false;

    public float movementSpeed = 1.75f;
    public float defaultMovementspeed;

    float fastMovementspeed;


    public bool movementLocked = false;

    public GameObject playerObject;
    public Animator animator;

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
        defaultMovementspeed = movementSpeed;

        FieldEvents.lookDirection = new Vector2(1, 0);
    }

    private void Start()
    {
        fastMovementspeed = movementSpeed * 3;
        defaultMovementspeed = movementSpeed;
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.LeftShift))

        {
            movementSpeed = fastMovementspeed;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))

        {
            movementSpeed = defaultMovementspeed;
        }
    }

    void FixedUpdate()
    {
        if (!movementLocked) 
        {
        Vector2 movePosition = playerRigidBody2d.position;
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

            horizontalInputRaw = Input.GetAxisRaw("Horizontal");
            verticalInputRaw = Input.GetAxisRaw("Vertical");

            movePosition.x = movePosition.x + movementSpeed * horizontalInput * Time.deltaTime;
        movePosition.y = movePosition.y + movementSpeed * verticalInput * Time.deltaTime;

        playerRigidBody2d.MovePosition(movePosition);
        playerPosition = movePosition;

            animator.SetFloat("horizontalInput", horizontalInput);
            animator.SetFloat("verticalInput", verticalInput);
            animator.SetFloat("horizontalInputRaw", horizontalInputRaw);
            animator.SetFloat("verticalInputRaw", verticalInputRaw);

            if (horizontalInput > 0)
            {
                FieldEvents.lookDirection = Vector2.right;
            }

            if (horizontalInput < 0)
            {
                FieldEvents.lookDirection = Vector2.left;
            }

            if (horizontalInput > 0)
            {
                FieldEvents.lookDirection = Vector2.up;
            }

            if (horizontalInput < 0)
            {
                FieldEvents.lookDirection = Vector2.down;
            }
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

            float boostedMovementSpeed = defaultMovementspeed + (defaultMovementspeed * SpeedBonus);

            movementSpeed = boostedMovementSpeed;

        }

        if (!isWalkwayBoost)

        {
            movementSpeed = defaultMovementspeed;          
        }

  
    }




}
