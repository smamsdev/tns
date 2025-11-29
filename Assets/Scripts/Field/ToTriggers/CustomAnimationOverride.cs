using System.Collections;
using UnityEngine;

public class CustomAnimationOverride : ToTrigger
{
    public Animator animator;
    [SerializeField] private AnimationClip animationToOverride;
    [SerializeField] private AnimationClip customAnimation;
    [SerializeField] private string stateName = "Custom";
    public bool isOneShotThenRevert;
    public bool isPlayingOverride;
    public Vector2 optionalLookDirOnExit;

    private RuntimeAnimatorController original;
    private AnimatorOverrideController overrideController;

    private void Awake()
    {
        original = animator.runtimeAnimatorController;
        overrideController = new AnimatorOverrideController(original);
    }

    public override IEnumerator TriggerFunction()
    {
        if (isPlayingOverride)
        {
            yield return RevertToOriginalAnimator();
            if (optionalLookDirOnExit != Vector2.zero)
            {
                var movementScript = animator.GetComponent<MovementScript>();
                movementScript.lookDirection = optionalLookDirOnExit;
                animator.SetFloat("lookDirectionX", optionalLookDirOnExit.x);
                animator.SetFloat("lookDirectionY", optionalLookDirOnExit.y);
            }
            yield break;
        }

        isPlayingOverride = true;

        overrideController[animationToOverride] = customAnimation;
        animator.runtimeAnimatorController = overrideController;

        if (optionalLookDirOnExit != Vector2.zero)
        {
            var movementScript = animator.GetComponent<MovementScript>();
            movementScript.lookDirection = optionalLookDirOnExit;
            animator.SetFloat("lookDirectionX", optionalLookDirOnExit.x);
            animator.SetFloat("lookDirectionY", optionalLookDirOnExit.y);
        }

        animator.Play(stateName, -1, 0f);

        if (isOneShotThenRevert)
        {
            yield return new WaitForSeconds(customAnimation.length);
            yield return RevertToOriginalAnimator();
        }
    }

    public IEnumerator RevertToOriginalAnimator()
    {
        if (!animator) yield break;

        animator.runtimeAnimatorController = original;
        animator.Play("Idle", -1, 0f);
        isPlayingOverride = false;
        yield return null;
    }
}
