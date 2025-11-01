using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KatanaSlice : PreciseMove
{
    public override IEnumerator TriggerMoveAnimation()
    {
        combatantToActAnimator.SetFloat("MoveAnimationFloat", moveAnimationFloat);

        combatantToActAnimator.speed = 0;
        combatantToActAnimator.Play("Attack", 0, 0);
        yield return new WaitForSeconds(0.5f);
        combatantToActAnimator.Play("Attack", 0, 0.2f);
        yield return new WaitForSeconds(0.5f);
        combatantToActAnimator.Play("Attack", 0, 0.7f);
        combatantToActAnimator.speed = 1;
    }
}
