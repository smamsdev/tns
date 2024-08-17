using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTrigger : ToTrigger
{
    public Animator animator;
    public string customAnimationTriggerNumber;

    public override IEnumerator DoAction()

    {
        animator.SetTrigger("Trigger" + customAnimationTriggerNumber);
        FieldEvents.HasCompleted.Invoke(this.gameObject);
        yield return null;
    }
}
