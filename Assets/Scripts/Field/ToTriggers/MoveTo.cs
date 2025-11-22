using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTo : ToTrigger
{
    public ActorMove actorMove;

    private static int activeMoveToCount = 0;

    public static int ActiveMoveToCount => activeMoveToCount;

    public override IEnumerator TriggerFunction()
    {
        activeMoveToCount++;

        var movementScript = actorMove.actorGO.GetComponent<MovementScript>();
        int i;

        CombatEvents.LockPlayerMovement();

        for (i = 0; i < actorMove.destination.Length;)
        {
            yield return MoveToPosition(movementScript, actorMove.destination[i]);
            i++;
        }

        activeMoveToCount--;

        if (activeMoveToCount == 0)
        {
            CombatEvents.UnlockPlayerMovement();
        }

        FieldEvents.HasCompleted.Invoke(this.gameObject);
    }

    private IEnumerator MoveToPosition(MovementScript movementScript, Vector3 targetPosition)
    {
        float startTime = Time.time;
        float timeoutDuration = 12f;

        float endPointDeltaX = Mathf.Abs(movementScript.transform.position.x - targetPosition.x);
        float endPointDeltaY = Mathf.Abs(movementScript.transform.position.y - targetPosition.y);

        while (endPointDeltaX > 0.03f)
        {
            if (Time.time - startTime > timeoutDuration)
            {
                Debug.LogError("actor stuck.");
                yield break; 
            }

            float directionX = targetPosition.x - movementScript.transform.position.x;
            movementScript.horizontalInput = Mathf.Sign(directionX);
            endPointDeltaX = Mathf.Abs(movementScript.transform.position.x - targetPosition.x);

            yield return null;
        }

        movementScript.horizontalInput = 0;
        movementScript.transform.position = new Vector3(targetPosition.x, movementScript.transform.position.y, movementScript.transform.position.z);

        startTime = Time.time;

        while (endPointDeltaY > 0.03f)
        {
            if (Time.time - startTime > timeoutDuration)
            {
                Debug.LogError("actor stuck.");
                yield break;
            }

            float directionY = targetPosition.y - movementScript.transform.position.y;
            movementScript.verticalInput = Mathf.Sign(directionY);
            endPointDeltaY = Mathf.Abs(movementScript.transform.position.y - targetPosition.y);

            yield return null;
        }

        movementScript.verticalInput = 0;
        movementScript.transform.position = new Vector3(targetPosition.x, targetPosition.y, movementScript.transform.position.z);
    }
}

[System.Serializable]
public class ActorMove
{
    public GameObject actorGO;
    public Vector3[] destination;
}