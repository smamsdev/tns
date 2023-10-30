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

    public float movementSpeed = 1.75f;

    public bool movementLocked = false;

    public GameObject playerObject;

    public Vector2 lookDirection = new Vector2(1, 0);

    private void OnEnable()
    {
        CombatEvents.UpdatePlayerPosition += UpdatePlayerPosition;
        CombatEvents.LockPlayerMovement += LockPlayerMovement;
        CombatEvents.UnlockPlayerMovement += UnlockPlayerMovement;
    }

    private void OnDisable()
    {
        CombatEvents.UpdatePlayerPosition -= UpdatePlayerPosition;
        CombatEvents.LockPlayerMovement -= LockPlayerMovement;
        CombatEvents.UnlockPlayerMovement -= UnlockPlayerMovement;
    }



    private void Awake()
    {
        playerPosition = playerRigidBody2d.position;
        playerRigidBody2d = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (!movementLocked) 
        {
        playerRigidBody2d.MovePosition(playerPosition);

        myHorizontalInput = Input.GetAxis("Horizontal");
        myVerticallInput = Input.GetAxis("Vertical");

        playerPosition.x = playerPosition.x + movementSpeed * myHorizontalInput * Time.deltaTime;
        playerPosition.y = playerPosition.y + movementSpeed * myVerticallInput * Time.deltaTime;

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
            raycastHit2D = Physics2D.Raycast(playerPosition , lookDirection, 0.4f, LayerMask.GetMask("NPC"));
            if (raycastHit2D.collider != null)
            {
                FieldEvents.PlayerRayCastHit?.Invoke(raycastHit2D);  
            }
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
}
