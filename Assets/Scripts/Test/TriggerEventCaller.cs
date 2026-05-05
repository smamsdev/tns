using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEventCaller : ToTrigger
{
    [SerializeField] private UnityEvent onTriggered;

    public override IEnumerator TriggerFunction()
    {
        onTriggered?.Invoke();
        yield return null;
    }
}