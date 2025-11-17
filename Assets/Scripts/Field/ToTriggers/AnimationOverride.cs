using System.Collections;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class AnimationOverride : ToTrigger
{
    public Animator animator;
    [SerializeField] private AnimationClip animationToOverride;
    [SerializeField] private AnimationClip customAnimation;
    [SerializeField] private string stateName = "Custom";
    public bool isOneShotThenRevert;
    public Vector2 optionalUpdateLookDirection;
    RuntimeAnimatorController original;

[Header("Will populate once triggered")]
    public AnimatorOverrideController newControllerToCreate;
    public bool readyToRevert;

    public override IEnumerator DoAction()
    {
        readyToRevert = true;

        original = animator.runtimeAnimatorController;
        newControllerToCreate = new AnimatorOverrideController(animator.runtimeAnimatorController);
        newControllerToCreate[animationToOverride] = customAnimation;
        animator.runtimeAnimatorController = newControllerToCreate;

        animator.Play(stateName);  // Play the state name, not the clip name

        if (optionalUpdateLookDirection != Vector2.zero)
        {
            var movementScript = animator.GetComponent<MovementScript>();
            movementScript.lookDirection = optionalUpdateLookDirection;
            animator.SetFloat("lookDirectionX", movementScript.lookDirection.x);
            animator.SetFloat("lookDirectionY", movementScript.lookDirection.y);
        }

        if (isOneShotThenRevert)
        {
            yield return new WaitForSeconds(customAnimation.length);
            animator.Play("Idle");
            animator.runtimeAnimatorController = original;
        }

        FieldEvents.HasCompleted.Invoke(this.gameObject);
    }

    public IEnumerator RevertToOriginalAnimator()
    {
        if (!readyToRevert)
        {
            Debug.Log("animatino hasn't trigered yet");
        }

        animator.runtimeAnimatorController = original;
        animator.Play("Idle");
        readyToRevert = false;
        yield return null;
    }
}