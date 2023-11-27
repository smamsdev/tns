using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;


public class SceneTriggers : MonoBehaviour
{
    public bool isTriggerOnLoad;
    public ToTrigger triggerOnLoad;
    public Trigger[] trigger;

    private void OnEnable()
    {
        FieldEvents.HasCompleted += TriggerAction;
    }

    private void OnDisable()
    {
        FieldEvents.HasCompleted -= TriggerAction;
    }

    private void Start()
    {
        if (isTriggerOnLoad) 
        {
        CombatEvents.LockPlayerMovement();
        StartCoroutine(triggerOnLoad.DoAction());
        triggerOnLoad = null;
        }
    }

    void TriggerAction(GameObject gameObject)
    {
        CombatEvents.LockPlayerMovement();

        for (int i = 0; i < trigger.Length; i++)
        {
            if (trigger[i].ifTriggered == gameObject)

            {
                StartCoroutine(trigger[i].toTrigger.DoAction());
                trigger[i].ifTriggered = null;
                trigger[i].toTrigger = null;

                return;
            }
        }
    }

}