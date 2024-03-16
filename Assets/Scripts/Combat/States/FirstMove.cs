using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstMove : State
{
    [SerializeField] CombatManager combatManager;
    [SerializeField] GameObject enemyAttackDisplay;
    [SerializeField] GameObject firstMoveContainer;

    public override IEnumerator StartState()
    {
        yield return new WaitForSeconds(0.1f);
        firstMoveContainer.SetActive(true);

        combatManager.combatUIScript.ShowFirstMoveMenu();
        combatManager.playerMoveManager.firstMoveIs = 0;

        combatManager.combatUIScript.playerFendScript.ShowHideFendDisplay(true);

        combatManager.enemy.SelectEnemyMove();

        CombatEvents.UpdateEnemyFendDisplay?.Invoke(combatManager.enemy.fendTotal);

        if (combatManager.enemy.attackTotal > 0)
        {
            CombatEvents.UpdateEnemyAttackDisplay?.Invoke(combatManager.enemy.EnemyAttackTotal());
            enemyAttackDisplay.SetActive(true);
        }

        yield break;
    }

}
