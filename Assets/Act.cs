using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Act : MonoBehaviour
{
    public ActorAction actorAction;

    public void StartAct()
    {
        StartCoroutine(PositionChange());
    }

    IEnumerator PositionChange()
    {
        actorAction.actID = this.gameObject.name;
        actorAction.isStarted = true;
        FieldEvents.ActorActionHasStarted?.Invoke(actorAction.actID, actorAction.isStarted);
        CombatEvents.LockPlayerMovement();
        {
            float elapsedTime = 0;
            Vector2 startingPos = actorAction.actor.transform.position;

            if (actorAction.overSeconds > 0)
            //if seconds value in inspector is above 0 use this method. Fixed Time method.

            {
                while (elapsedTime < actorAction.overSeconds)
                {
                    actorAction.actor.transform.position = Vector2.Lerp(startingPos, actorAction.moveTo, (elapsedTime / actorAction.overSeconds));
                    elapsedTime += Time.deltaTime;
                    yield return new WaitForEndOfFrame();
                }
                actorAction.actor.transform.position = actorAction.moveTo;
                actorAction.isComplete = true;
                CombatEvents.UnlockPlayerMovement();
                FieldEvents.ActorActionHasCompleted?.Invoke(actorAction.actID, actorAction.isComplete);

            }

            else
            // if seconds is null us this method. Fixed speed method, inifite time.
            {
                {
                    while (Vector3.Distance(actorAction.actor.transform.position, actorAction.moveTo) > 0)
                    {
                        actorAction.actor.transform.position = Vector3.MoveTowards(actorAction.actor.transform.position, actorAction.moveTo, Time.deltaTime * actorAction.speed);
                        elapsedTime += Time.deltaTime;
                        yield return new WaitForEndOfFrame();
                    }
                    actorAction.actor.transform.position = actorAction.moveTo;
                    actorAction.isComplete = true;
                    CombatEvents.UnlockPlayerMovement();
                    FieldEvents.ActorActionHasCompleted?.Invoke(actorAction.actID, actorAction.isComplete);
                }
            }
        }
    }

}

[System.Serializable]

public class ActorAction

{
    public GameObject actor;
    public Vector2 moveTo;
    public float speed;
    public float overSeconds;
    [HideInInspector] public string actID;
    [HideInInspector] public bool isStarted;
    [HideInInspector] public bool isComplete;

}
