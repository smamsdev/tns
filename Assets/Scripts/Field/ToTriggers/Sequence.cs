using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : ToTrigger
{

    public ToTrigger[] toTrigger;
    int i;
    bool isSequenceRunning;

    private void OnEnable()
    {
        FieldEvents.HasCompleted += TriggerAction;
    }

    private void OnDisable()
    {
        FieldEvents.HasCompleted -= TriggerAction;
    }

    public override IEnumerator DoAction()

    {
        CombatEvents.LockPlayerMovement();
        isSequenceRunning = true;
        StartCoroutine(toTrigger[i].DoAction());
        i = 1;

        yield return null;
    }

    void TriggerAction(GameObject gameObject) 
    {

        if (isSequenceRunning) 
        {
            if (toTrigger[(i-1)].gameObject == gameObject && (i) < toTrigger.Length)

            { 
                toTrigger[i-1] = null;

                StartCoroutine(toTrigger[i].DoAction());
                i++;
            }

            else
            {
                toTrigger[i - 1] = null;
                isSequenceRunning = false;
                FieldEvents.HasCompleted.Invoke(this.gameObject);
            }
        }
    }

}
