using System.Collections;
using UnityEngine;

public class ChangeCameraFollow : ToTrigger
{
    CameraFollow cameraFollow;

    public Transform newTransformToFollow;
    public float delaybeforeTransitionDuration;
    public float transitionFactor;

    public override IEnumerator TriggerFunction()
    {
        CombatEvents.LockPlayerMovement();

        cameraFollow = GameObject.FindWithTag("MainCamera").GetComponent<CameraFollow>();
        float defaultSmoothTime = cameraFollow.smoothTime;
        cameraFollow.smoothTime = transitionFactor;

        yield return new WaitForSeconds(delaybeforeTransitionDuration);

        cameraFollow.transformToFollow = newTransformToFollow;

        yield return new WaitForSeconds(transitionFactor);

        while (cameraFollow.velocity.sqrMagnitude > 0.0001f)
        {
            yield return null;
        }

        cameraFollow.smoothTime = defaultSmoothTime;

        CombatEvents.UnlockPlayerMovement();
        FieldEvents.HasCompleted.Invoke(this.gameObject);

        yield return null;
    }
}