using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreciseBasic : PreciseMove
{
    public override IEnumerator OnApplyMove(CombatManager _combatManager, Enemy _enemy)

    {
        combatManager = _combatManager;
        var playerMovementScript = combatManager.player.GetComponent<PlayerMovementScript>();
        var enemyPosition = combatManager.battleScheme.enemies[combatManager.selectedEnemy].transform.position;
        var moveSelected = combatManager.selectedPlayerMove;

        enemy = _enemy;

        combatManager.CombatUIManager.playerFendScript.ShowFendDisplay(true);


        {
          ////combatManager.UpdateFighterPosition(combatManager.player, new Vector2(combatManager.battleScheme.enemyGameObject[combatManager.selectedEnemy].transform.position.x - 0.3f, combatManager.battleScheme.enemyGameObject[combatManager.selectedEnemy].transform.position.y), 0.5f);
          //yield return new WaitForSeconds(0.5f);
          //combatManager.enemies[combatManager.selectedEnemy].enemyUI.fendScript.UpdateFendText(combatManager.playerCombat.attackPower, playerMovementScript.lookDirection, moveSelected.attackPushStrength);
          //
          //yield return new WaitForSeconds(0.3f);
          ////combatManager.UpdateFighterPosition(combatManager.player, combatManager.battleScheme.playerFightingPosition.transform.position, 0.5f);
          //
          //yield return new WaitForSeconds(1);

            //fix this bullshit
        }


        {
            yield return new WaitForSeconds(0.5f);
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
