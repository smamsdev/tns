using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MovementSpeedChanger : MonoBehaviour
{
    public float speedBonus;

    private void OnTriggerStay2D (Collider2D other)
    {
        if (other.tag == "Player")

        {
            FieldEvents.movementSpeedMultiplier = speedBonus;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")

        {
            FieldEvents.movementSpeedMultiplier = 1;
        }
    }
}
