using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ascendingStairs : MonoBehaviour
{

    [SerializeField] GameObject descend;
    public BoxCollider2D stairsResetTrigger;
    [SerializeField] PlayerMovementScript playerMovementScript;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && playerMovementScript.isDescending)

        {
            CombatEvents.UnlockPlayerMovement?.Invoke();
            descend.GetComponent<BoxCollider2D>().enabled = false; 
            playerMovementScript.isDescending = false;
            stairsResetTrigger.enabled = true;
            this.GetComponent<BoxCollider2D>().enabled = false;

        }

    }
}
