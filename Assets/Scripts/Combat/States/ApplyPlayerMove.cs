using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ApplyPlayerMove : State
{
    PlayerCombat player;

    public override IEnumerator StartState()
    {
        combatManager.combatMenuManager.DisableMenuState();
        player = combatManager.playerCombat;

        foreach (Enemy enemy in combatManager.enemies)
        {
            enemy.combatantUI.attackDisplay.ShowAttackDisplay(false);
        }

        yield return ApplyMove();

        if (combatManager.enemies.Count == 0)
        {
            combatManager.SetState(combatManager.victory);
            yield break;
        }

        if (player.fendTotal > 0)
        {
            player.combatantUI.fendScript.ShowFendDisplay(player, true);
            yield return new WaitForSeconds(1f);
        }

        if (combatManager.allies.Count > 0 && combatManager.enemies.Count > 0)
        {
            combatManager.SetState(combatManager.allyMoveState);
            yield break;
        }

        else
        {
            combatManager.SetState(combatManager.enemyMoveState);
            yield break;
        }
    }

    IEnumerator ApplyMove()
    {

        //Disable other combatant UI elements. dont know if we still need
       // foreach (Enemy enemy in combatManager.enemies)
       // {
       //     if (enemy != player.targetToAttack)
       //
       //     {
       //         enemy.combatantUI.fendScript.ShowFendDisplay(enemy, false);
       //         enemy.combatantUI.statsDisplay.ShowStatsDisplay(false);
       //     }
       // }

        //reset narrator focus camera on allyToAct and wait
        CombatEvents.UpdateNarrator("");
        combatManager.cameraFollow.transformToFollow = player.transform;

        yield return new WaitForSeconds(0.5f);

        //update potential
        PlayerMove moveSelected = combatManager.playerCombat.moveSelected as PlayerMove;
        moveSelected.combatManager = combatManager;
        combatManager.playerCombat.TotalPlayerAttackPower(moveSelected.attackMoveModPercent);
        CombatEvents.UpdatePlayerPot.Invoke(moveSelected.potentialChange);

        player.moveSelected.LoadMoveStats(player, combatManager);

        yield return player.moveSelected.ApplyMove(player, player.targetToAttack);

        if (player.targetToAttack !=null)
        {
            if (player.targetToAttack.CurrentHP == 0)
            {
                combatManager.CombatantDefeated(player.targetToAttack);
            }

            else         //return target to original pos if still alive
            {
                yield return new WaitForSeconds(0.5f);
                yield return combatManager.PositionCombatant(player.targetToAttack.gameObject, player.targetToAttack.fightingPosition.transform.position);
            }
        }

        player.combatantUI.fendScript.fendTextMeshProUGUI.text = player.TotalPlayerFendPower(combatManager.playerCombat.moveSelected.fendMoveModPercent).ToString();
    }
}