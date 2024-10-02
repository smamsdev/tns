using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slope : MonoBehaviour
{
    public float SlopePercentage = 1.0f;
    private MovementScript movementScript;

    bool isOnSlope = false;

    private void OnTriggerEnter2D(Collider2D collisionWith)
    {
        movementScript = collisionWith.GetComponent<MovementScript>();
        isOnSlope = true;
    }

    private void OnTriggerExit2D(Collider2D collisionWith)
    {
        isOnSlope = false;
        movementScript.sloping = 0;
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