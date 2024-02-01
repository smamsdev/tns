using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCompleteTrigger : ToTrigger
{


    public override IEnumerator DoAction()

    {
        FieldEvents.HasCompleted.Invoke(this.gameObject);
        yield return null;
    }

    public void AnimationComplete()

    {
        FieldEvents.HasCompleted.Invoke(this.gameObject);
    }

}
