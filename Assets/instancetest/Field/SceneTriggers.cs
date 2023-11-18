using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;


public class SceneTriggers : MonoBehaviour
{
    public Trigger[] trigger;

    private void OnEnable()
    {
        FieldEvents.HasCompleted += TriggerAction;
    }

    private void OnDisable()
    {
        FieldEvents.HasCompleted -= TriggerAction;
    }

    void TriggerAction(GameObject gameObject)
    {
        for (int i = 0; i < trigger.Length; i++)
        {
            if (trigger[i].ifTriggered == gameObject)

            {
              StartCoroutine(trigger[i].toTrigger.DoAction());
            }
        }
    }

}