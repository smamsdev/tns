using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Walkways : MonoBehaviour
{
    public float speedBonus;

    private void OnTriggerStay2D (Collider2D other)
    {
        if (other.tag == "Player")

        {
            FieldEvents.IsWalkwayBoost?.Invoke(true, speedBonus) ;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")

        {
            FieldEvents.IsWalkwayBoost?.Invoke(false, speedBonus);
        }

    }
}
