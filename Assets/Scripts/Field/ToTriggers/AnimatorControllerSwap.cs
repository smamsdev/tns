using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorControllerSwap : ToTrigger
{
    public Animator animator;
    public AnimatorOverrideController animatorOverrideController;
    public float delay;

    public override IEnumerator DoAction()
    {
        yield return new WaitForSeconds(delay);

        animator.runtimeAnimatorController = animatorOverrideController;

        FieldEvents.HasCompleted.Invoke(this.gameObject);
        yield return null;
    }
}