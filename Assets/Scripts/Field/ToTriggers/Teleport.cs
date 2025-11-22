using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : ToTrigger
{
    public ActorJump[] actorJump;
    int i;

    public override IEnumerator TriggerFunction()
    {
        for (i = 0; i < actorJump.Length;)
        {

            if (actorJump[i].actorGO == null)
            { actorJump[i].actorGO = transform.parent.transform.parent.gameObject; }

            actorJump[i].actorGO.transform.position = actorJump[i].locationToAppear;

            i++;
            yield return null;
        }

        CombatEvents.UnlockPlayerMovement();

        if (i == actorJump.Length)
        {
            FieldEvents.HasCompleted.Invoke(this.gameObject);
        }
    }
}

[System.Serializable]

public class ActorJump
{
    public GameObject actorGO;
    public Vector3 locationToAppear;
}

