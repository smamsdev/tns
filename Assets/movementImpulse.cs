using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTo : ToTrigger
{
    public ActorMove actorMove;

    public override IEnumerator DoAction()
    {
        var movementScript = actorMove.actorGO.GetComponent<MovementScript>();
        int i;
        CombatEvents.LockPlayerMovement();

        for (i = 0; i < actorMove.destination.Length;)
        {
            float startTime = Time.time;
            float timeoutDuration = 4f; // Timeout duration in seconds

            float endPointDeltaX = Mathf.Abs(actorMove.actorGO.transform.position.x - actorMove.destination[i].x);
            float endPointDeltaY = Mathf.Abs(actorMove.actorGO.transform.position.y - actorMove.destination[i].y);

            while (endPointDeltaX > 0.03f)
            {
                if (Time.time - startTime > timeoutDuration)
                {
                    Debug.LogError("actor stuck.");
                    yield break; 
                }

                movementScript.scriptedMovement = true;

                float directionX = actorMove.destination[i].x - actorMove.actorGO.transform.position.x;
                movementScript.horizontalInput = Mathf.Sign(directionX);
                endPointDeltaX = Mathf.Abs(actorMove.actorGO.transform.position.x - actorMove.destination[i].x);

                yield return null;
            }

            movementScript.horizontalInput = 0;
            actorMove.actorGO.transform.position = new Vector3(actorMove.destination[i].x, actorMove.actorGO.transform.position.y, actorMove.actorGO.transform.position.z);

            startTime = Time.time;

            while (endPointDeltaY > 0.03f)
            {
                if (Time.time - startTime > timeoutDuration)
                {
                    Debug.LogError("actor stuck.");
                    yield break;
                }

                float directionY = actorMove.destination[i].y - actorMove.actorGO.transform.position.y;
                movementScript.verticalInput = Mathf.Sign(directionY);
                endPointDeltaY = Mathf.Abs(actorMove.actorGO.transform.position.y - actorMove.destination[i].y);

                yield return null;
            }

            movementScript.verticalInput = 0;
            actorMove.actorGO.transform.position = new Vector3(actorMove.destination[i].x, actorMove.destination[i].y, actorMove.actorGO.transform.position.z);

            movementScript.scriptedMovement = false;
            i++;
        }

        CombatEvents.UnlockPlayerMovement();
        FieldEvents.HasCompleted.Invoke(this.gameObject);
    }
}

[System.Serializable]

public class ActorMove
{
    public GameObject actorGO;
    public Vector3[] destination; //vector3 so i can copy paste :3
}
