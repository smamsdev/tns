using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTo : ToTrigger
{
    public ActorMove actorMove;

    private static int activeMoveToCount = 0;

    public static int ActiveMoveToCount => activeMoveToCount; // Property to access the count

    public override IEnumerator DoAction()
    {
        // Increment the counter when starting a MoveTo
        activeMoveToCount++;

        var movementScript = actorMove.actorGO.GetComponent<MovementScript>();
        int i;

        // Lock player movement
        CombatEvents.LockPlayerMovement();

        for (i = 0; i < actorMove.destination.Length;)
        {
            yield return MoveToPosition(movementScript, actorMove.destination[i]);
            i++;
        }

        // Decrement the counter when finished
        activeMoveToCount--;

        // Only unlock if there are no more MoveTo instances active
        if (activeMoveToCount == 0)
        {
            CombatEvents.UnlockPlayerMovement();
            Debug.Log("complete");
        }

        FieldEvents.HasCompleted.Invoke(this.gameObject);
    }

    private IEnumerator MoveToPosition(MovementScript movementScript, Vector3 targetPosition)
    {
        float startTime = Time.time;
        float timeoutDuration = 5f; // Timeout duration in seconds

        float endPointDeltaX = Mathf.Abs(movementScript.transform.position.x - targetPosition.x);
        float endPointDeltaY = Mathf.Abs(movementScript.transform.position.y - targetPosition.y);

        while (endPointDeltaX > 0.03f)
        {
            if (Time.time - startTime > timeoutDuration)
            {
                Debug.LogError("actor stuck.");
                yield break; // Optionally handle stuck condition
            }

            movementScript.scriptedMovement = true;

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
                yield break; // Optionally handle stuck condition
            }

            float directionY = targetPosition.y - movementScript.transform.position.y;
            movementScript.verticalInput = Mathf.Sign(directionY);
            endPointDeltaY = Mathf.Abs(movementScript.transform.position.y - targetPosition.y);

            yield return null;
        }

        movementScript.verticalInput = 0;
        movementScript.transform.position = new Vector3(targetPosition.x, targetPosition.y, movementScript.transform.position.z);
        movementScript.scriptedMovement = false;
    }
}

[System.Serializable]
public class ActorMove
{
    public GameObject actorGO;
    public Vector3[] destination; // Vector3 so I can copy-paste :3
}