using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slerp : ToTrigger
{

    public ActorJump[] actorJump;
    int i;
    public float overTime;

    public override IEnumerator TriggerFunction()
    {
        for (i = 0; i < actorJump.Length;)
        {
            CombatEvents.LockPlayerMovement();

            float elapsedTime = 0;
            Vector2 startingPos = actorJump[i].actorGO.transform.position;
            while (elapsedTime < overTime)
            {
                actorJump[i].actorGO.transform.localPosition = Vector2.Lerp(startingPos, actorJump[i].locationToAppear, (elapsedTime / overTime));
                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            actorJump[i].actorGO.transform.localPosition = actorJump[i].locationToAppear;

            i++;

        }

        if (i == actorJump.Length)
        {
            CombatEvents.UnlockPlayerMovement();
            FieldEvents.HasCompleted.Invoke(this.gameObject);

        }

        yield return null;
    }

}

