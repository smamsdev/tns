using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BooleanCheckTrigger : ToTrigger
{
    public bool conditionMet;
    public ToTrigger toTriggerOnCheck;

    private void OnEnable()
    {
        FieldEvents.HasCompleted += TriggerAction;
    }

    private void OnDisable()
    {
        FieldEvents.HasCompleted -= TriggerAction;
    }

    public override IEnumerator TriggerFunction()
    {
        if (conditionMet)
        {
            StartCoroutine(toTriggerOnCheck.Triggered());
        }

        else
        {
            FieldEvents.HasCompleted.Invoke(this.gameObject);
        }

        yield return null;
    }

    void TriggerAction(GameObject gameObject)
    {
        if (conditionMet && toTriggerOnCheck.gameObject == gameObject)

        {
            FieldEvents.HasCompleted.Invoke(this.gameObject);
        }
    }
}
