using System.Collections;
using UnityEngine;

public class CombatMovement : MonoBehaviour
{
    public ActorMove actorMove;
    public MovementScript movementScript;

    public IEnumerator MoveCombatant(GameObject gameObject, Vector3 targetPosition)
    {
        actorMove.actorGO = gameObject;
        actorMove.destination[0] = targetPosition;

        movementScript = actorMove.actorGO.GetComponent<MovementScript>();

        float startTime = Time.time;
        float timeoutDuration = 4f; // Timeout if actor is stuck
        float endPointDeltaY = Mathf.Abs(movementScript.transform.position.y - targetPosition.y);

        // Vertical movement
        while (endPointDeltaY > 0.05f)
        {
            if (Time.time - startTime > timeoutDuration)
            {
                Debug.LogError("Actor stuck.");
                yield break;
            }

            float directionY = targetPosition.y - movementScript.transform.position.y;
            movementScript.verticalInput = Mathf.Sign(directionY);
            endPointDeltaY = Mathf.Abs(movementScript.transform.position.y - targetPosition.y);

            yield return null;
        }

        movementScript.verticalInput = 0;
        movementScript.transform.position = new Vector3(movementScript.transform.position.x, targetPosition.y, movementScript.transform.position.z);

        // Horizontal movement
        startTime = Time.time;
        float endPointDeltaX = Mathf.Abs(movementScript.transform.position.x - targetPosition.x);

        while (endPointDeltaX > 0.037f)
        {
            if (Time.time - startTime > timeoutDuration)
            {
                Debug.LogError("Actor stuck.");
                yield break;
            }

            float directionX = targetPosition.x - movementScript.transform.position.x;
            movementScript.horizontalInput = Mathf.Sign(directionX);
            endPointDeltaX = Mathf.Abs(movementScript.transform.position.x - targetPosition.x);

            yield return null;
        }

        movementScript.horizontalInput = 0;
        movementScript.transform.position = new Vector3(targetPosition.x, targetPosition.y, movementScript.transform.position.z);
    }

    public IEnumerator LerpPositionBySpeed(GameObject gameObject, Vector3 targetPosition, float moveSpeed)
    {
        actorMove.actorGO = gameObject;
        actorMove.destination[0] = targetPosition;

        movementScript = actorMove.actorGO.GetComponent<MovementScript>();

        Vector3 startPosition = movementScript.transform.position;
        float distance = Vector3.Distance(startPosition, targetPosition);
        float duration = distance / moveSpeed;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            movementScript.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        movementScript.transform.position = targetPosition;
    }

    public IEnumerator LerpPositionFixedTime(GameObject gameObject, Vector3 targetPosition, float fixedDuration)
    {
        actorMove.actorGO = gameObject;
        actorMove.destination[0] = targetPosition;

        movementScript = actorMove.actorGO.GetComponent<MovementScript>();

        Vector3 startPosition = movementScript.transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < fixedDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fixedDuration;

            movementScript.transform.position = Vector3.Lerp(startPosition, targetPosition, t);

            yield return null;
        }

        movementScript.transform.position = targetPosition;
    }
}