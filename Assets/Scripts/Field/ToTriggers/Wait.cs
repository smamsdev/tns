using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wait : ToTrigger
{

    public float waitTime;

    public override IEnumerator DoAction()
    {
        CombatEvents.LockPlayerMovement();
        yield return new WaitForSeconds(waitTime);
        CombatEvents.UnlockPlayerMovement();
        FieldEvents.HasCompleted.Invoke(this.gameObject);
    }
}

