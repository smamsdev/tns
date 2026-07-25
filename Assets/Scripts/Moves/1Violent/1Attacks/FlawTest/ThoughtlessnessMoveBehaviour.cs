using System.Collections;
using UnityEngine;

public class ThoughtlessnessMoveBehaviour : MoveBehaviour
{
    public override IEnumerator ApplyCustomMoveFunction()
    {
        yield return (combatantToAct.CombatHPChanged(combatantToAct.AttackTotal / -3, combatManager));
    }
}
