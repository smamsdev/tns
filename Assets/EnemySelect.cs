using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySelect : State
{
    public SelectEnemyMenuScript selectEnemyMenuScript;
    [SerializeField] CombatManager combatManager;

    public override IEnumerator StartState()
    {
        if (combatManager.battleScheme.enemyGameObject.Length == 1)

        {
            combatManager.SetState(combatManager.attackTarget);
            yield break;
        }

        combatManager.CombatUIManager.ShowEnemySelectMenu(true);
        combatManager.CombatUIManager.ShowSecondMoveMenu(false);
        yield break;
    }

    public override void CombatOptionSelected(int moveValue)

    {
        combatManager.selectedEnemy = moveValue;
        combatManager.SetState(combatManager.attackTarget);
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))

        {
            combatManager.SetState(combatManager.secondMove);
        }
    }
}
