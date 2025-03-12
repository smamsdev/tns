using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViolentBasic : ViolentMove
{
    public override IEnumerator OnApplyMove(CombatManager _combatManager, Enemy _enemy)

    {
        combatManager = _combatManager;
        var playerMovementScript = combatManager.player.GetComponent<PlayerMovementScript>();
        var enemyPosition = combatManager.battleScheme.enemies[combatManager.selectedEnemy].transform.position;
        var moveSelected = combatManager.selectedPlayerMove;

        combatManager.CombatUIManager.playerFendScript.ShowFendDisplay(true);

        if (isAttack)
        {
            //move to attack position

            var combatMovementInstanceGO = Instantiate(combatManager.combatMovementPrefab, this.transform);
            var combatMovementInstance = combatMovementInstanceGO.GetComponent<CombatMovement>();
            yield return (combatMovementInstance.MoveCombatant(combatManager.player.gameObject, enemyPosition, 85f));
            Destroy(combatMovementInstanceGO);

            combatManager.enemies[combatManager.selectedEnemy].enemyUI.enemyFendScript.ApplyPlayerAttackToFend(combatManager.playerCombatStats.attackPower, playerMovementScript.lookDirection, moveSelected.attackPushStrength);

            combatManager.playerAnimator.SetTrigger("Attack");
            yield return new WaitForSeconds(0.5f);
        }

        //if we just want to use this as a basic fend
        if (!isAttack)

        {
            yield return new WaitForSeconds(0.5f);
        }
    }

    public override IEnumerator OnEnemyAttack(CombatManager _combatManager, Enemy _enemy)

    {
        yield break; //null as it's a basic move only
    }

    public override IEnumerator Return()
    {
        var combatMovementInstanceGO = Instantiate(combatManager.combatMovementPrefab, this.transform);
        var combatMovementInstance = combatMovementInstanceGO.GetComponent<CombatMovement>();
        yield return (combatMovementInstance.MoveCombatant(combatManager.player.gameObject, combatManager.battleScheme.playerFightingPosition.transform.position));
        Destroy(combatMovementInstanceGO);
    }
}
