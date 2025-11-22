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
        movementScript.lookDirection = newLookDirection;
        yield return null;

        FieldEvents.HasCompleted.Invoke(this.gameObject);
    }
}
