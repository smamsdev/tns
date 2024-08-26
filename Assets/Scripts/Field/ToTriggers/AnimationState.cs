using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationState : ToTrigger
{
    public Animator animator;
    public string customAnimationStateNumber;

    public bool revertstate;

    public override IEnumerator DoAction()
        //
        {

        Debug.Log("test");
            if (!revertstate)
            {
                animator.SetBool("State" + customAnimationStateNumber, true);
                FieldEvents.HasCompleted.Invoke(this.gameObject);

                yield return null;
            }

            if (revertstate)
            {
                animator.SetBool("State" + customAnimationStateNumber, false);
                FieldEvents.HasCompleted.Invoke(this.gameObject);

                yield return null;
            }
        }

    }
