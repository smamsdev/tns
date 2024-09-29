using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiTrigger : ToTrigger
{
    public ToTrigger[] toMultiTrigger;
    private int completedCount = 0;


    public override IEnumerator DoAction()
    {
        List<Coroutine> activeCoroutines = new List<Coroutine>();

        foreach (var trigger in toMultiTrigger)
        {
            activeCoroutines.Add(StartCoroutine(TriggerAndCount(trigger)));
        }

        // Optionally wait for all to finish
        foreach (var coroutine in activeCoroutines)
        {
            yield return coroutine;
        }

        // All triggers completed
        FieldEvents.HasCompleted.Invoke(this.gameObject);
    }

    private IEnumerator TriggerAndCount(ToTrigger trigger)
    {
        Debug.Log($"Starting DoAction for {trigger.name}");

        yield return StartCoroutine(trigger.DoAction());

        Debug.Log($"Completed DoAction for {trigger.name}");
        completedCount++;

        if (completedCount == toMultiTrigger.Length)
        {
            Debug.Log("All triggers completed.");
        }
    }
}