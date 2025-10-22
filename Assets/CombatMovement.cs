using System.Collections;
using UnityEngine;

public class CombatMovement : MonoBehaviour
{
    public ActorMove actorMove;
    public MovementScript movementScript;

    public IEnumerator MoveCombatant(GameObject gameObject, Vector3 targetPosition, float stoppingPercentage = 100f, bool useTimeout = false)
    {
        actorMove.actorGO = gameObject;
        actorMove.destination[0] = targetPosition;

        movementScript = actorMove.actorGO.GetComponent<MovementScript>();

        // Calculate stopping distance based on the stopping percentage
        Vector3 stoppingPosition = Vector3.Lerp(movementScript.transform.position, targetPosition, stoppingPercentage / 100f);

        float startTime = Time.time;
        float timeoutDuration = 4f; // Timeout if actor is stuck
        float moveTimeoutDuration = 0.5f; // Timeout duration if enabled
        float endPointDeltaY = Mathf.Abs(movementScript.transform.position.y - stoppingPosition.y);

        // Vertical movement
        while (endPointDeltaY > 0.05f)
        {
            if (useTimeout && Time.time - startTime > moveTimeoutDuration) yield break;
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

        // Horizontal movement
        startTime = Time.time;
        float endPointDeltaX = Mathf.Abs(movementScript.transform.position.x - stoppingPosition.x);

        while (endPointDeltaX > 0.035f)
        {
            if (useTimeout && Time.time - startTime > moveTimeoutDuration) yield break;
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

    public IEnumerator MoveCombatantFixedTime(GameObject gameObject, Vector3 targetPosition, float _fixedDuration, bool isReversingX = false)
    {
        actorMove.actorGO = gameObject;
        actorMove.destination[0] = targetPosition;

        movementScript = actorMove.actorGO.GetComponent<MovementScript>();

        float fixedDuration = _fixedDuration / 2;
        Vector3 startPosition = movementScript.transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < fixedDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fixedDuration;

            movementScript.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            float directionX = targetPosition.x - movementScript.transform.position.x;

            movementScript.horizontalInput = Mathf.Sign(directionX);
            yield return null;
        }

        movementScript.transform.position = targetPosition;
        movementScript.horizontalInput = 0f;
    }
}