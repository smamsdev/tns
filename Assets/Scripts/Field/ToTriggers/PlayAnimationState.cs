using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimationState : ToTrigger
{
    public Animator animator;
    public string animationStateName;
    public bool waitForAnimationToComplete;

    public override IEnumerator TriggerFunction()
    {
        animator.Play(animationStateName);

        // Wait 1 frame so Animator updates to the new state
        yield return null;

        var state = animator.GetCurrentAnimatorStateInfo(0);
        float length = state.length;

        yield return new WaitForSeconds(length);
    }
}
