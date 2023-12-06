using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shift : ToTrigger
{
    public ActorShift[] actorShift;
    Vector2 startingPos;
    Vector2 endPos;
    int i;

    public override IEnumerator DoAction()
    {
        for (i = 0; i < actorShift.Length;)
        {
            CombatEvents.LockPlayerMovement();
            actorShift[i].actorGO = transform.parent.transform.parent.gameObject;
            CalculateDistance();

            float elapsedTime = 0;
            startingPos = actorShift[i].actorGO.transform.localPosition;

            //if seconds value in inspector is above 0 use this method. Fixed Time method.
            if (actorShift[i].optionalOverSeconds > 0)
            {
                while (elapsedTime < actorShift[i].optionalOverSeconds)
                {
                    actorShift[i].actorGO.transform.position = Vector2.Lerp(startingPos, endPos, (elapsedTime / actorShift[i].optionalOverSeconds));
                    elapsedTime += Time.deltaTime;
                    yield return new WaitForEndOfFrame();
                }

                CombatEvents.UnlockPlayerMovement();

                actorShift[i].actorGO.transform.position = endPos;
                i++;

                if (i == actorShift.Length)
                {
                    FieldEvents.HasCompleted.Invoke(this.gameObject);

                }
            }

            // if seconds is null us this method. Fixed speed method, inifite time.
            else
            {
                while (Vector3.Distance(actorShift[i].actorGO.transform.position, endPos) > 0)
                {
                    actorShift[i].actorGO.transform.position = Vector3.MoveTowards(actorShift[i].actorGO.transform.position, endPos, Time.deltaTime * actorShift[i].speed);
                    elapsedTime += Time.deltaTime;
                    yield return new WaitForEndOfFrame();
                }
                actorShift[i].actorGO.transform.position = endPos;
                i++;
                CombatEvents.UnlockPlayerMovement();

                if (i == actorShift.Length)
                {
                    FieldEvents.HasCompleted.Invoke(this.gameObject);

                }
            }


        }
    }

    void CalculateDistance()
    {
        switch (actorShift[i].direction)
        {
            case Direction.Left: endPos = new Vector2((actorShift[i].actorGO.transform.position.x - actorShift[i].distance), actorShift[i].actorGO.transform.position.y); break;
            case Direction.Up: endPos = new Vector2(actorShift[i].actorGO.transform.position.x, (actorShift[i].actorGO.transform.position.y + actorShift[i].distance)); break;
            case Direction.Down: endPos = new Vector2(actorShift[i].actorGO.transform.position.x, (actorShift[i].actorGO.transform.position.y - actorShift[i].distance)); break;
            case Direction.Right: endPos = new Vector2((actorShift[i].actorGO.transform.position.x + actorShift[i].distance), actorShift[i].actorGO.transform.position.y); break;
        }
    }
}

public enum Direction { Left, Up, Down, Right };
[System.Serializable]

public class ActorShift
{
    [HideInInspector] public GameObject actorGO;
    public Direction direction;
    public float distance;
    public float speed;
    public float optionalOverSeconds;
}
