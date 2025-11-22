using System.Collections;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class CustomAnimationOverride : ToTrigger
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

    public override IEnumerator TriggerFunction()
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
    }

    public IEnumerator RevertToOriginalAnimator()
    {
        if (!readyToRevert)
        {
            Debug.Log("animation hasn't trigered yet");
        }

        animator.runtimeAnimatorController = original;
        animator.Play("Idle");
        readyToRevert = false;
        yield return null;
    }
}