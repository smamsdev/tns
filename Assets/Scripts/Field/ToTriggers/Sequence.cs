using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : ToTrigger
{
    public ToTrigger[] toTrigger;
    bool isSequenceRunning;
    public bool isLoopable;

    public ToTrigger[] reload;
    int i = 0;

    private void OnEnable()
    {
        FieldEvents.HasCompleted += TriggerAction;
        isSequenceRunning = false;
        reload = (ToTrigger[])toTrigger.Clone();
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
            StartCoroutine(toTrigger[i].Triggered());
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
        if (isLoopable)
        {
            toTrigger = (ToTrigger[])reload.Clone();
            i = 0;
        }

        isSequenceRunning = false;
        FieldEvents.HasCompleted.Invoke(this.gameObject);
    }

    protected override void TriggerComplete()
    {
        //Do nothing, EndSequence will manage
    }
}