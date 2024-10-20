using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slope : MonoBehaviour
{
    public float SlopePercentage = 1.0f;
    public MovementScript movementScript;

    //we need to ignore NPC collider layers or its a big mess
    int ignoreLayer = 10;

    bool isOnSlope = false;

    private void OnTriggerEnter2D(Collider2D collisionWith)
    {
        if (collisionWith.gameObject.layer != ignoreLayer)
        {
            var collisionObject = collisionWith;
            movementScript = collisionObject.GetComponent<MovementScript>();
            isOnSlope = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collisionWith)
    {
        if (collisionWith.gameObject.layer != ignoreLayer)
        {
            isOnSlope = false;
            movementScript.sloping = 0;
        }
    }

    private void Update()
    {
        if (isOnSlope)
        {
            if (movementScript.horizontalInput != 0)
            {
                movementScript.sloping = SlopePercentage * Mathf.Sign(movementScript.horizontalInput);
            }
            else
            {
                movementScript.sloping = 0;
            }
        }
    }
}