using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayDefaultAnimation : ToTrigger
{
    public Animator animator;

    private void Awake()
    {
        animator.enabled = false;
    }

    public override IEnumerator TriggerFunction()
    {
        var clipLength = animator.runtimeAnimatorController.animationClips[0].length;

        CombatEvents.LockPlayerMovement();
        animator.enabled = true;
        yield return new WaitForSeconds(clipLength);

        animator.enabled = false;
        CombatEvents.UnlockPlayerMovement();
    }
}
