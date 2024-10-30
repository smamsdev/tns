using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BooleanCheckTrigger : ToTrigger
{
    public bool conditionMet;
    public ToTrigger toTriggerOnCheck;

    public override IEnumerator DoAction()
    {
        if (conditionMet)
        {
            StartCoroutine(toTriggerOnCheck.DoAction());
        }

        FieldEvents.HasCompleted.Invoke(this.gameObject);
        yield return null;
    }
}
