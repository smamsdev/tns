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

    public float movementSpeed = 1.75f;

    public bool movementLocked = false;

    public GameObject playerObject;
    public GameObject loadButton;

    private void OnEnable()
    {
        CombatEvents.UpdatePlayerPosition += UpdatePlayerPosition;

    }

    private void OnDisable()
    {
        CombatEvents.UpdatePlayerPosition -= UpdatePlayerPosition;

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

    public void UpdatePlayerPosition(Vector2 end, float seconds) //call the coroutine using a function because you can't call coroutines when invoking events

    { 
        StartCoroutine(UpdatePlayerPositionCoRoutine(end, seconds)); 
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

}
