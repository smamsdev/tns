using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CautiousBasic : CautiousMove
{
 

    public override IEnumerator OnEnemyAttack(CombatManager _combatManager, Enemy _enemy)

    {
        yield break;
    }

    public override IEnumerator Return()
    {
        yield return null;
    }
}
