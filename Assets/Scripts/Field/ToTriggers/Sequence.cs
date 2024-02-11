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
        CombatEvents.LockPlayerMovement();
        isSequenceRunning = true;
        StartCoroutine(toTrigger[i].DoAction());
        i++;


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

                if (i == toTrigger.Length)
                { EndSequence(); }
            }
        }

    }

    void EndSequence()
    {
        toTrigger[i - 1] = null;
            
        Debug.Log("ending");
            
        isSequenceRunning = false;
        FieldEvents.HasCompleted.Invoke(this.gameObject);
    }

}
