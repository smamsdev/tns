using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : ToTrigger
{

    public ToTrigger[] toTrigger;
    int i = 0;
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
        isSequenceRunning = true;
        StartCoroutine(toTrigger[i].DoAction());

        yield return null;
    }

    void TriggerAction(GameObject gameObject) 
    {
        if (isSequenceRunning) 
        {
            if (toTrigger[i].gameObject == gameObject)

            {
                toTrigger[i] = null;
                i++;

                if (i == toTrigger.Length)
                { 
                    EndSequence();
                    return;
                }

                if (i < toTrigger.Length)
                {
                    StartCoroutine(toTrigger[i].DoAction());
                }
            }
        }
    }

    void EndSequence()
    {
        toTrigger[i - 1] = null;
                  
        isSequenceRunning = false;
        FieldEvents.HasCompleted.Invoke(this.gameObject);
    }

}
