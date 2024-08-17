using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationState : ToTrigger
{
    public Animator animator;
    public string customAnimationStateNumber;

    public override IEnumerator DoAction()

    {
        animator.SetBool("State"+ customAnimationStateNumber, true);
        FieldEvents.HasCompleted.Invoke(this.gameObject);
        yield return null;
    }
}
