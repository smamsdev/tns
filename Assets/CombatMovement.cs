using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatMovement : MonoBehaviour
{
    public ActorMove actorMove;
    private MovementScript movementScript;

    public IEnumerator MoveCombatant(GameObject gameObject, Vector3 targetPosition, float stoppingPercentage = 100f, bool useTimeout = false)
    {
        actorMove.actorGO = gameObject;
        actorMove.destination[0] = targetPosition;

        movementScript = actorMove.actorGO.GetComponent<MovementScript>();

        // Calculate stopping distances
        Vector3 stoppingPosition = Vector3.Lerp(movementScript.transform.position, targetPosition, stoppingPercentage / 100f);

        float startTime = Time.time;
        float timeoutDuration = 4f; // Overall timeout duration
        float moveTimeoutDuration = 0.5f; // Duration for the timeout if enabled

        float endPointDeltaY = Mathf.Abs(movementScript.transform.position.y - stoppingPosition.y);

        // Handle vertical movement
        while (endPointDeltaY > 0.03f)
        {
            if (useTimeout && Time.time - startTime > moveTimeoutDuration)
            {
                yield break;
            }

            if (Time.time - startTime > timeoutDuration)
            {
                Debug.LogError("Actor stuck.");
                yield break;
            }

            float directionY = stoppingPosition.y - movementScript.transform.position.y;
            movementScript.verticalInput = Mathf.Sign(directionY);
            endPointDeltaY = Mathf.Abs(movementScript.transform.position.y - stoppingPosition.y);

            yield return null;
        }

        movementScript.verticalInput = 0;
        movementScript.transform.position = new Vector3(movementScript.transform.position.x, stoppingPosition.y, movementScript.transform.position.z);

        // Handle horizontal movement
        startTime = Time.time; // Reset the start time for horizontal movement
        float endPointDeltaX = Mathf.Abs(movementScript.transform.position.x - stoppingPosition.x);

        while (endPointDeltaX > 0.03f)
        {
            if (useTimeout && Time.time - startTime > moveTimeoutDuration)
            {
                yield break;
            }

            if (Time.time - startTime > timeoutDuration)
            {
                Debug.LogError("Actor stuck.");
                yield break;
            }

            float directionX = stoppingPosition.x - movementScript.transform.position.x;
            movementScript.horizontalInput = Mathf.Sign(directionX);
            endPointDeltaX = Mathf.Abs(movementScript.transform.position.x - stoppingPosition.x);

            yield return null;
        }

        movementScript.horizontalInput = 0;
        movementScript.transform.position = new Vector3(stoppingPosition.x, stoppingPosition.y, movementScript.transform.position.z);
    }
}