using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViolentBasic : ViolentMove
{
    public override IEnumerator OnApplyMove(CombatManager _combatManager, Enemy _enemy)

    {
        combatManager = _combatManager;
        var playerMovementScript = combatManager.player.GetComponent<PlayerMovementScript>();
        var enemyPosition = combatManager.battleScheme.enemyGameObject[combatManager.selectedEnemy].transform.position;
        combatManager.CombatUIManager.playerFendScript.ShowFendDisplay(true);

        if (isAttack)
        {
            //move to attack position
            yield return combatManager.combatMovement.MoveCombatant(combatManager.player.gameObject, enemyPosition, 90f);
            combatManager.enemy[combatManager.selectedEnemy].enemyUI.enemyFendScript.ApplyPlayerAttackToFend(combatManager.playerCombatStats.attackPower);
            combatManager.playerAnimator.SetTrigger("Attack");
            yield return new WaitForSeconds(0.5f);

            //fall back

            yield return combatManager.combatMovement.MoveCombatant(combatManager.player.gameObject, combatManager.battleScheme.playerFightingPosition.transform.position, isReversing: true);

            //tidy up and end move
            combatManager.applyMove.EndMove();
        }

        //if we just want to use this as a basic fend
        if (!isAttack)

        {
            yield return new WaitForSeconds(0.5f);
            combatManager.applyMove.EndMove();
        }
    }

    public override IEnumerator OnEnemyAttack(CombatManager _combatManager, Enemy _enemy)

    {
        yield break;
    }
}
