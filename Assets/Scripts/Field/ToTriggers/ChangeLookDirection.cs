using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLookDirection : ToTrigger
{
    public Vector2 newLookDirection;

    public override IEnumerator DoAction()
    {
        FieldEvents.lookDirection = newLookDirection;
        yield return null;
        FieldEvents.HasCompleted.Invoke(this.gameObject);
    }
}
