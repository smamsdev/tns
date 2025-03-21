using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyPlayerMove : State
{
    [SerializeField] CombatManager combatManager;

    public override IEnumerator StartState()
    {
        combatManager.combatMenuManager.DisableMenuState();

        //get a bunch of refs...
        var player = combatManager.playerCombat;
        var playerMovementScript = player.GetComponent<PlayerMovementScript>();
        var playerAnimator = playerMovementScript.animator;
        var playerLookDirection = playerMovementScript.lookDirection;

        //store enemy target look dir
        var enemyTargetMovementScript = player.targetToAttack.GetComponent<MovementScript>();
        var enemyTargetStoredLookDir = enemyTargetMovementScript.lookDirection;

        //store player look dir and camera focus
        var playerLastLookDir = playerLookDirection; //this needs to happen here to remember direction of last enemy attacked
        combatManager.cameraFollow.transformToFollow = player.transform;

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

        var targetToAttackUI = player.targetToAttack.GetComponentInChildren<CombatantUI>();
        targetToAttackUI.statsDisplay.ShowStatsDisplay(true);
        yield return player.moveSelected.ApplyMove(player, player.targetToAttack);
        player.GetComponent<MovementScript>().lookDirection = playerLastLookDir;

        //return enemy target to original pos and look dir
        Animator targetToAttackAnimator = player.targetToAttack.GetComponent<Animator>();
        targetToAttackAnimator.SetTrigger("CombatIdle");
        yield return new WaitForSeconds(0.5f);
        yield return combatManager.PositionCombatant(player.targetToAttack.gameObject, player.targetToAttack.fightingPosition.transform.position);
        enemyTargetMovementScript.lookDirection = enemyTargetStoredLookDir;

        HideEnemyFends();
        yield return UpdateFendDisplay();

        //check for player defeat
        if (combatManager.defeat.playerDefeated)
        {
            Debug.Log("player defeated");
            yield break;
        }

        combatManager.SetState(combatManager.enemyMoveState);
        yield return null;
    }

    void HideEnemyFends()
    {
        foreach (Enemy enemy in combatManager.enemies)

        {
            enemy.combatantUI.fendScript.animator.SetTrigger("fendFade");
        }
    }

    IEnumerator UpdateFendDisplay()
    {
        combatManager.playerCombat.combatantUI.fendScript.UpdateFendText(combatManager.playerCombat.TotalPlayerFendPower(combatManager.playerCombat.moveSelected.fendMoveModPercent));

        if (combatManager.playerCombat.fendTotal > 0)
        {
            combatManager.playerCombat.combatantUI.fendScript.ShowFendDisplay(true);
            yield return new WaitForSeconds(1f);
        }
        
        else
        {
            combatManager.playerCombat.combatantUI.fendScript.ShowFendDisplay(false);
        }
    }
}