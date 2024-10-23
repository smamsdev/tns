using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public SpriteRenderer doorSprite;
    public MovementScript movementScript;
    bool disablingDoor;
    int ignoreLayer = 10;

    private void OnEnable()
    {
        disablingDoor = false;
    }

    private void OnDisable()
    {
        disablingDoor = true;
    }

    void OnTriggerEnter2D(Collider2D collisionWith)

    {
        if (collisionWith.gameObject.layer != ignoreLayer)
        { 
            movementScript = collisionWith.gameObject.GetComponent<MovementScript>();
            StartCoroutine(OpenDoor());
        }
    }

    void OnTriggerExit2D(Collider2D collisionWith)
    {
        if (!disablingDoor & collisionWith.gameObject.layer != ignoreLayer)
        {
            StartCoroutine(CloseDoor());
        }
    }

    IEnumerator OpenDoor()

    {
        movementScript.movementSpeed = 0;
        yield return new WaitForSeconds(0.1f);
        doorSprite.enabled = false;
        yield return new WaitForSeconds(0.1f);
        movementScript.movementSpeed = movementScript.defaultMovementspeed;
    }

    IEnumerator CloseDoor()

    {
        yield return new WaitForSeconds(0.1f);
        doorSprite.enabled = true;
        yield return new WaitForSeconds(0.1f);
    }

}
