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
        isSequenceRunning = false;
    }

    private void OnDisable()
    {
        FieldEvents.HasCompleted -= TriggerAction;
    }

    public override IEnumerator TriggerFunction()
    {
        isSequenceRunning = true;
        if (i < toTrigger.Length)
        {
            yield return (toTrigger[i].Triggered());
        }
        yield return null;
    }

    void TriggerAction(GameObject gameObject)
    {
        if (isSequenceRunning)
        {
            if (i < toTrigger.Length && toTrigger[i].gameObject == gameObject)
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
                    StartCoroutine(toTrigger[i].Triggered());
                }
            }
        }
    }

    void EndSequence()
    {
        if (i > 0)
        {
            toTrigger[i - 1] = null;
        }

        isSequenceRunning = false;
        FieldEvents.HasCompleted.Invoke(this.gameObject);
    }

}