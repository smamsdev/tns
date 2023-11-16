using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovementScript : MonoBehaviour
{

    public Rigidbody2D playerRigidBody2d;
    public RaycastHit2D raycastHit2D;
    public Vector2 playerPosition;

    float myHorizontalInput;
    float myVerticallInput;

    public bool isWalkwayBoost;

    public float movementSpeed = 1.75f;
    public float baseMovementSpeed;

    public bool movementLocked = false;

    public GameObject playerObject;

    public Vector2 lookDirection = new Vector2(1, 0);

    private void OnEnable()
    {
        CombatEvents.UpdatePlayerPosition += UpdatePlayerPosition;
        CombatEvents.LockPlayerMovement += LockPlayerMovement;
        CombatEvents.UnlockPlayerMovement += UnlockPlayerMovement;
        FieldEvents.IsWalkwayBoost += IsWalkwayBoost;
    }

    private void OnDisable()
    {
        CombatEvents.UpdatePlayerPosition -= UpdatePlayerPosition;
        CombatEvents.LockPlayerMovement -= LockPlayerMovement;
        CombatEvents.UnlockPlayerMovement -= UnlockPlayerMovement;
        FieldEvents.IsWalkwayBoost -= IsWalkwayBoost;
    }



    private void Awake()
    {
        playerPosition = playerRigidBody2d.position;
        playerRigidBody2d = GetComponent<Rigidbody2D>();
        baseMovementSpeed = movementSpeed;
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

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            this.GetComponent<BoxCollider2D>().enabled = false;


            raycastHit2D = Physics2D.BoxCast(gameObject.transform.position, new Vector2(0.2f, 0.5f), 0f, lookDirection, 0.10f, Physics.AllLayers);

            if (raycastHit2D.collider != null)


            {
                FieldEvents.PlayerRayCastHit?.Invoke(raycastHit2D);
            }

            this.GetComponent<BoxCollider2D>().enabled = true;
        }
    }


    public void UpdatePlayerPosition(Vector2 end, float seconds) //call the coroutine using a function because you can't call coroutines when invoking events

    { 
        StartCoroutine(UpdatePlayerPositionCoRoutine(end, seconds));
        CombatEvents.LockPlayerMovement?.Invoke();
    }

            public IEnumerator UpdatePlayerPositionCoRoutine(Vector2 end, float seconds)
            {
                float elapsedTime = 0;
                Vector2 startingPos = playerObject.transform.position;
                while (elapsedTime < seconds)
                {
                    playerObject.transform.position = Vector2.Lerp(startingPos, end, (elapsedTime / seconds));
                    elapsedTime += Time.deltaTime;
                    yield return new WaitForEndOfFrame();
                }
                 playerObject.transform.position = end;
            }

    public void LockPlayerMovement()

    {
        playerPosition = playerObject.transform.position;
        movementLocked = true;
    }

    public void UnlockPlayerMovement()

    {
        playerPosition = playerObject.transform.position;
        movementLocked = false;
        //Debug.Log("unlocked") ;
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
