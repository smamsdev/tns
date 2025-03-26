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
        FieldEvents.StartScene += StartScene;    
    }

    private void OnDisable()
    {
        FieldEvents.HasCompleted -= TriggerAction;
        FieldEvents.StartScene -= StartScene;
    }

    private void StartScene()
    {
        if (isTriggerOnLoad) 
        {
        StartCoroutine(triggerOnLoad.DoAction());
        }
    }

    void TriggerAction(GameObject gameObject)
    {
       if (trigger != null)
       {
           for (int i = 0; i < trigger.Length; i++)
           {
                if (trigger[i].ifTriggered == gameObject)

               {
                   StartCoroutine(trigger[i].toTrigger.DoAction());
                  //trigger[i].ifTriggered = null;
                  //trigger[i].toTrigger = null;

                   return;
               }
           }
       }
    }

}