using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovementScript : MonoBehaviour
{
    public Rigidbody2D playerRigidBody2d;

    public Vector2 playerPosition; //WHAT WERE THESE FOR?

    public bool isWalkwayBoost;
    public bool isDescending = false;

    public float movementSpeed = 1.75f;
    public float defaultMovementspeed;

    public bool movementLocked = false;

    public Vector2 isAscending;

    public Animator animator;

    public float horizontalInput;
    public float verticalInput;
    public float horizontalInputRaw;
    public float verticalInputRaw;

    public Vector2 movePosition;

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
        playerRigidBody2d = GetComponent<Rigidbody2D>();
        defaultMovementspeed = movementSpeed;
    }

    private void Start()
    {
        defaultMovementspeed = movementSpeed;
        Vector2 movePosition = playerRigidBody2d.position;
        animator.SetFloat("sceneEntryDirection", FieldEvents.lookDirection.x);
        isAscending = Vector2.one;
    }

    private void Update()
    {


        if (Input.GetKeyDown(KeyCode.LeftShift))

        {
            FieldEvents.specialMovementSpeed = defaultMovementspeed * 3;
            movementSpeed = FieldEvents.specialMovementSpeed;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))

        {
            movementSpeed = defaultMovementspeed;
        }

        if (Input.GetKeyDown(KeyCode.L))

        {
            LockPlayerMovement();
        }

        if (Input.GetKeyUp(KeyCode.L))

        {
            UnlockPlayerMovement();
        }
    }

    void FixedUpdate()
    {
        if (!movementLocked)

        {
            movePosition = playerRigidBody2d.position;

            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical") * isAscending.y;
            horizontalInputRaw = Input.GetAxisRaw("Horizontal");
            verticalInputRaw = Input.GetAxisRaw("Vertical");

            movePosition.x += movementSpeed * horizontalInput * Time.deltaTime;
            movePosition.y += movementSpeed * verticalInput * Time.deltaTime;

            playerRigidBody2d.MovePosition(movePosition);

            animator.SetFloat("horizontalInput", horizontalInput);
            animator.SetFloat("verticalInput", verticalInput);
            animator.SetFloat("horizontalInputRaw", horizontalInputRaw);
            animator.SetFloat("verticalInputRaw", verticalInputRaw);

            if (horizontalInputRaw > 0)
            {
                FieldEvents.lookDirection = Vector2.right;
            }

            if (horizontalInputRaw < 0)
            {
                FieldEvents.lookDirection = Vector2.left;
            }

            if (verticalInputRaw > 0)
            {
                FieldEvents.lookDirection = Vector2.up;
            }

            if (verticalInputRaw < 0)
            {
                FieldEvents.lookDirection = Vector2.down;
            }
        }
    }

    public void LockPlayerMovement()

    {
        // playerPosition = this.transform.position;  WHAT WERE THESE FOR?
        movementLocked = true;
    }

    public void UnlockPlayerMovement()

    {
        //  playerPosition = this.transform.position;
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
