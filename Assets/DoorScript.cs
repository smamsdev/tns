using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public SpriteRenderer doorSprite;

    void OnTriggerEnter2D()

    {
        StartCoroutine(OpenDoor());
    }

    void OnTriggerExit2D()
    {
        StartCoroutine(CloseDoor());
    }

    IEnumerator OpenDoor()

    {
        CombatEvents.LockPlayerMovement.Invoke();
        yield return new WaitForSeconds(0.1f);
        doorSprite.enabled = false;
        yield return new WaitForSeconds(0.1f);
        CombatEvents.UnlockPlayerMovement.Invoke();
    }

    IEnumerator CloseDoor()

    {
        yield return new WaitForSeconds(0.1f);
        doorSprite.enabled = true;
        yield return new WaitForSeconds(0.1f);
    }

}
