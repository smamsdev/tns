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

        foreach (Ally ally in combatManager.allies)
        {
            ally.combatantUI.attackDisplay.ShowAttackDisplay(false);
            ally.combatantUI.statsDisplay.statsDisplayGameObject.SetActive(false);
            ally.combatantUI.fendScript.ShowFendDisplay(ally, false);
        }

        foreach (Enemy enemy in combatManager.enemies)
        {
            enemy.combatantUI.attackDisplay.ShowAttackDisplay(false);
        }

        yield return ApplyMove();

        if (combatManager.allies.Count > 0)
        {
            combatManager.SetState(combatManager.allyMoveState);
        }

        else
        {
            combatManager.SetState(combatManager.enemyMoveState);
        }
        yield return null;
    }

    IEnumerator ApplyMove()
    {

        //Disable other combatant UI elements
        foreach (Enemy enemy in combatManager.enemies)
        {
            if (enemy != player.targetToAttack)

            {
                enemy.combatantUI.fendScript.ShowFendDisplay(enemy, false);
                enemy.combatantUI.statsDisplay.ShowStatsDisplay(false);
            }
        }

        var playerMovementScript = player.GetComponent<PlayerMovementScript>();
        var playerLookDirection = playerMovementScript.lookDirection;

        ////store enemy target look dir DO I NEED THIS
        //var enemyTargetMovementScript = player.targetToAttack.GetComponent<MovementScript>();
        //var enemyTargetStoredLookDir = enemyTargetMovementScript.lookDirection;

        //store player look dir and camera focus
        var playerLastLookDir = playerLookDirection;

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

        player.combatantUI.fendScript.fendTextMeshProUGUI.text = player.TotalPlayerFendPower(combatManager.playerCombat.moveSelected.fendMoveModPercent).ToString();

        var targetToAttackUI = player.targetToAttack.GetComponentInChildren<CombatantUI>();
        targetToAttackUI.statsDisplay.ShowStatsDisplay(true);

        yield return player.moveSelected.ApplyMove(player, player.targetToAttack);

        player.GetComponent<MovementScript>().lookDirection = playerLastLookDir;

        if (player.targetToAttack.CurrentHP == 0)
        {
            player.targetToAttack.Defeated();
        }

        //return target to original pos and look dir, if still alive
        else
        {
            yield return new WaitForSeconds(0.5f);
            yield return combatManager.PositionCombatant(player.targetToAttack.gameObject, player.targetToAttack.fightingPosition.transform.position);
        }

        //check for player defeat
        if (combatManager.defeat.playerDefeated)
        {
            Debug.Log("player defeated");
            yield break;
        }
    }
}