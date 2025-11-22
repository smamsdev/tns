using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorControllerSwap : ToTrigger
{
    public Animator animator;
    public AnimatorOverrideController animatorOverrideController;
    public float delay;

    public override IEnumerator TriggerFunction()
    {
        yield return new WaitForSeconds(delay);
        animator.runtimeAnimatorController = animatorOverrideController;
    }
}