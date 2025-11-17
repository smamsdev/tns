using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationRevert : ToTrigger
{
    [SerializeField] AnimationOverride overrideToRevert;
    public Vector2 optionalUpdateLookDirection;

    public override IEnumerator DoAction()
    {
        yield return(overrideToRevert.RevertToOriginalAnimator());

        if (optionalUpdateLookDirection != Vector2.zero)
        {
            var movementScript = overrideToRevert.animator.GetComponent<MovementScript>();
            movementScript.lookDirection = optionalUpdateLookDirection;
            overrideToRevert.animator.SetFloat("lookDirectionX", movementScript.lookDirection.x);
            overrideToRevert.animator.SetFloat("lookDirectionY", movementScript.lookDirection.y);
        }

        FieldEvents.HasCompleted.Invoke(this.gameObject);
    }
}
