using System.Collections;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class CustomAnimationOneShotToTrigger : ToTrigger
{
    [SerializeField] private Animator animator;
    [SerializeField] private AnimationClip animationToOverride;
    [SerializeField] private AnimationClip customAnimation;
    [SerializeField] private string stateName = "Custom";

    public override IEnumerator DoAction()
    {
        var original = animator.runtimeAnimatorController;
        var overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        overrideController[animationToOverride] = customAnimation;
        animator.runtimeAnimatorController = overrideController;

        animator.Play(stateName);  // Play the state, not the clip

        yield return new WaitForSeconds(customAnimation.length);
        animator.Play("Idle");
        animator.runtimeAnimatorController = original;
    }
}