using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wait : ToTrigger
{

    public float waitTime;

    public override IEnumerator TriggerFunction()
    {
        CombatEvents.LockPlayerMovement();
        yield return new WaitForSeconds(waitTime);
        CombatEvents.UnlockPlayerMovement();
        FieldEvents.HasCompleted.Invoke(this.gameObject);
    }
}

