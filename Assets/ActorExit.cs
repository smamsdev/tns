using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorExit : ToTrigger
{
    public ToTrigger triggerToLeave;
    public Transform actorToLeave;
    
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
        if (triggerToLeave.gameObject == gameObject)

        {
            Debug.Log("putiton!!");
            actorToLeave.position = new Vector3(1000, 1000, 0);
            FieldEvents.HasCompleted.Invoke(this.gameObject);
        } 
    }
}
