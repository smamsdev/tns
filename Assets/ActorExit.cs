using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class ActorExit : ToTrigger
{
    public Transform actorToLeave;
    public ToTrigger optionalTriggerToLeave;

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
            StartCoroutine(Triggered());
        } 
    }

    public override IEnumerator TriggerFunction()
    {
        CombatEvents.LockPlayerMovement();
        var sr = actorToLeave.GetComponent<SpriteRenderer>();

        yield return (FieldEvents.LerpValuesCoRo(1f, 0f, 1f, 
            alpha =>
            {
                var color = sr.color;
                color.a = alpha;
                sr.color = color;
            })
        );

        actorToLeave.position = new Vector3(0, 69000, 0);

        var color = sr.color;
        color.a = 1;
        sr.color = color;
        actorToLeave.gameObject.SetActive( false );
    }
}
