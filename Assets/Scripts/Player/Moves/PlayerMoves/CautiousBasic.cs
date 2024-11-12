using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CautiousBasic : CautiousMove
{
    public override IEnumerator OnApplyMove(CombatManager _combatManager,Enemy _enemy)

    {
        combatManager = _combatManager;
        var playerMovementScript = combatManager.player.GetComponent<PlayerMovementScript>();
        var enemyPosition = combatManager.battleScheme.enemyGameObject[combatManager.selectedEnemy].transform.position;
        var moveSelected = combatManager.selectedPlayerMove;

        combatManager.CombatUIManager.playerFendScript.ShowFendDisplay(true);

        if (isAttack)
        {

            //combatManager.UpdateFighterPosition(combatManager.player, new Vector2(combatManager.battleScheme.enemyGameObject[combatManager.selectedEnemy].transform.position.x - 0.3f, combatManager.battleScheme.enemyGameObject[combatManager.selectedEnemy].transform.position.y), 0.5f);
            yield return new WaitForSeconds(0.5f);
            combatManager.enemy[combatManager.selectedEnemy].enemyUI.enemyFendScript.ApplyPlayerAttackToFend(combatManager.playerCombatStats.attackPower, playerMovementScript.lookDirection, moveSelected.attackPushStrength);

            yield return new WaitForSeconds(0.3f);
            //combatManager.UpdateFighterPosition(combatManager.player, combatManager.battleScheme.playerFightingPosition.transform.position, 0.5f);

            yield return new WaitForSeconds(1);

        }
    }

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
