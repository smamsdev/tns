using System.Collections;
using UnityEngine;

public class ChangeCameraFollow : ToTrigger
{
    CameraFollow cameraFollow;

    public Transform newTransformToFollow;
    public float delaybeforeTransitionDuration;
    public float transitionDuration;

    public override IEnumerator TriggerFunction()
    {
        FieldEvents.isCameraFollow = false;
        CombatEvents.LockPlayerMovement();

        Debug.Log("update camera.main");
        cameraFollow = GameObject.FindWithTag("MainCamera").GetComponent<CameraFollow>();

        yield return new WaitForSeconds(delaybeforeTransitionDuration);

        Vector3 startPosition = cameraFollow.transform.position;
        var newPos = new Vector3(newTransformToFollow.position.x + cameraFollow.xOffset, newTransformToFollow.position.y + cameraFollow.yOffset, -10f);

        float elapsedTime = 0;

        while (elapsedTime < transitionDuration)
        {
            Vector3 newPosition = Vector3.Lerp(startPosition, newPos, elapsedTime / transitionDuration);
            cameraFollow.transform.position = newPosition;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        cameraFollow.transform.position = newPos;

        cameraFollow.transformToFollow = newTransformToFollow;
        FieldEvents.isCameraFollow = true;
        CombatEvents.UnlockPlayerMovement();
        FieldEvents.HasCompleted.Invoke(this.gameObject);

        yield return null;
    }
}