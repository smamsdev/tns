using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLookDirection : ToTrigger
{
    public Vector2 newLookDirection;
    public GameObject ActorGameObject;

    public override IEnumerator TriggerFunction()
    {
        var movementScript = ActorGameObject.GetComponent<MovementScript>();

        yield return null;

        if (!movementScript.animator.enabled)
            movementScript.animator.enabled = true;

        movementScript.lookDirection = newLookDirection;
        Debug.Log("treh" + movementScript.lookDirection);

        yield return null;

        movementScript.animator.enabled = false;
        FieldEvents.HasCompleted.Invoke(this.gameObject);
    }
}
