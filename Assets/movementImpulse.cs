using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTo : ToTrigger
{

    public ActorMove actorMove;

    void Start()
    {
        StartCoroutine(DoAction());
    }

    IEnumerator ApplyArtificialInput(float duration, float artificialInput)
    {
        float timer = 0;
        while (timer < duration)
        {
            CombatEvents.LockPlayerMovement();
          //  playerMovementScript.horizontalInput = artificialInput;
            yield return null;
            timer += Time.deltaTime;
        }
        CombatEvents.UnlockPlayerMovement();
    }

    public override IEnumerator DoAction()
    {
        var movementScript = actorMove.actorGO.GetComponent<MovementScript>();
        int i;

        for (i = 0; i < actorMove.destination.Length;)
        {
            float startTime = Time.time;
            float timeoutDuration = 5f; // Timeout duration in seconds

            while (Mathf.Abs(actorMove.actorGO.transform.position.x - actorMove.destination[i].x) > 0.01f)
            {
                if (Time.time - startTime > timeoutDuration)
                {
                    Debug.LogError("actor stuck.");
                    yield break; 
                }

                CombatEvents.LockPlayerMovement();

                float directionX = actorMove.destination[i].x - actorMove.actorGO.transform.position.x;
                movementScript.horizontalInput = Mathf.Sign(directionX);

                yield return null;
            }

            movementScript.horizontalInput = 0;
            actorMove.actorGO.transform.position = new Vector3(actorMove.destination[i].x, actorMove.actorGO.transform.position.y, actorMove.actorGO.transform.position.z);

            startTime = Time.time;

            while (Mathf.Abs(actorMove.actorGO.transform.position.y - actorMove.destination[i].y) > 0.01f)
            {
                if (Time.time - startTime > timeoutDuration)
                {
                    Debug.LogError("actor stuck.");
                    yield break;
                }

                float directionY = actorMove.destination[i].y - actorMove.actorGO.transform.position.y;
                movementScript.verticalInput = Mathf.Sign(directionY);

                yield return null;
            }

            movementScript.verticalInput = 0;
            actorMove.actorGO.transform.position = new Vector3(actorMove.destination[i].x, actorMove.destination[i].y, actorMove.actorGO.transform.position.z);

            CombatEvents.UnlockPlayerMovement();
            i++;
        }
    }
}

[System.Serializable]

public class ActorMove
{
    public GameObject actorGO;
    public Vector3[] destination; //vector so i can copy paste :3
}
