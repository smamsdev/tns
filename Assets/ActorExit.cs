using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorExit : ToTrigger
{
    public ToTrigger optionalTriggerToLeave;
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
        if (optionalTriggerToLeave != null && optionalTriggerToLeave.gameObject == gameObject)

        {
            StartCoroutine(DoAction());
        } 
    }

    public override IEnumerator DoAction()

    {
        actorToLeave.position = new Vector3(1000, 1000, 0);
        FieldEvents.HasCompleted.Invoke(this.gameObject);

        yield return null;
    }
}
