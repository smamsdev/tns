using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViolentBotch : ViolentMove
{


    public override IEnumerator OnEnemyAttack(CombatManager _combatManager, Enemy _enemy)

    {
        yield break;
    }

    public override IEnumerator Return()
    {
        Debug.Log("todo");
        yield return null;
    }
}
