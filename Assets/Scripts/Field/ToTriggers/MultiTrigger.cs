using System.Collections;
using UnityEngine;

public class MultiTrigger : ToTrigger
{
    public ToTrigger[] toMultiTrigger;
    int multiCompleted = 0;

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
        foreach (var trigger in toMultiTrigger)
        {
            StartCoroutine(trigger.Triggered());
        }

        yield return null;
    }

    void TriggerAction(GameObject gameObject)

    {
        foreach (var trigger in toMultiTrigger)
        {
            if (trigger.gameObject == gameObject)
            {
                multiCompleted++;
            }
        }

        if (multiCompleted == toMultiTrigger.Length)

        {
            multiCompleted = 0;
            FieldEvents.HasCompleted.Invoke(this.gameObject);
        }
    }
}