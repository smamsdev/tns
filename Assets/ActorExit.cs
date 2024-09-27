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
        if (triggerToLeave != null && triggerToLeave.gameObject == gameObject)

        {
            actorToLeave.position = new Vector3(1000, 1000, 0);
            TriggerComplete();
        } 
    }

    void TriggerComplete()

    {
        triggerToLeave = null;
        FieldEvents.HasCompleted.Invoke(this.gameObject);
    }

    public override IEnumerator DoAction()

    {
        actorToLeave.position = new Vector3(1000, 1000, 0);
        TriggerComplete();

        yield break;
    }
}
