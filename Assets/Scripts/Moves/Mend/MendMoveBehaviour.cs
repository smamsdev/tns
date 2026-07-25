using System.Collections;
using UnityEngine;

public class MendMoveBehaviour : MoveBehaviour
{
    public override IEnumerator ApplyCustomMoveFunction()
    {
        int healAMount = Random.Range(15, 30);

        yield return (combatantToAct.CombatHPChanged(healAMount, combatManager));
    }
}
 