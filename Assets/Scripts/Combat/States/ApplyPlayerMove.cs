using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyPlayerMove : State
{
    [SerializeField] CombatManager combatManager;

    public override IEnumerator StartState()
    {
        combatManager.CombatUIManager.DisableMenuState();

        //get a bunch of refs...
        var player = combatManager.playerCombat;
        var playerMovementScript = player.GetComponent<PlayerMovementScript>();
        var playerAnimator = playerMovementScript.animator;
        var playerLookDirection = playerMovementScript.lookDirection;

        //store enemy target look dir
        var enemyTargetMovementScript = player.targetToAttack.GetComponent<MovementScript>();
        var enemyTargetStoredLookDir = enemyTargetMovementScript.lookDirection;

        //store player look dir and camera focus
        var storedLookDir = playerLookDirection; //this needs to happen here to remember direction of last enemy attacked
        combatManager.cameraFollow.transformToFollow = player.transform;

        //reset narrator focus camera on allyToAct and wait
        CombatEvents.UpdateNarrator.Invoke("");
        combatManager.cameraFollow.transformToFollow = player.transform;
        yield return new WaitForSeconds(0.5f);

        //update potential
        PlayerMove moveSelected = combatManager.playerCombat.moveSelected as PlayerMove;
        moveSelected.combatManager = combatManager;
        combatManager.playerCombat.TotalPlayerAttackPower(moveSelected.attackMoveModPercent);
        CombatEvents.UpdatePlayerPot.Invoke(moveSelected.potentialChange);

        //display move name and move to position
        CombatEvents.UpdateNarrator.Invoke(player.moveSelected.moveName);
        yield return player.moveSelected.MoveToPosition(player.gameObject, player.moveSelected.AttackPositionLocation(player));

        //apply move effects to target
        var targetToAttackUI = player.targetToAttack.GetComponentInChildren<FendScript>();
        targetToAttackUI.ApplyAttackToFend(player, player.targetToAttack);

        //start animation
        playerAnimator.SetFloat("attackAnimationToUse", player.moveSelected.animtionIntTriggerToUse);
        playerAnimator.SetTrigger("Attack");
        yield return new WaitForSeconds(0.2f);

        //return player to fightingpos, and return look direct
        yield return player.moveSelected.ReturnFromPosition(player.gameObject, player.fightingPosition.transform.position);
        playerMovementScript.lookDirection = storedLookDir;
        playerAnimator.SetTrigger("CombatIdle"); //remember to blend the transition in animator settings or it will wiggle

        //reset narrator
        CombatEvents.UpdateNarrator.Invoke("");

        //check for player defeat
        if (combatManager.defeat.playerDefeated)
        {
            Debug.Log("player defeated");
            yield break;
        }

        //return enemy target to original pos and look dir
        yield return new WaitForSeconds(0.5f);
        yield return combatManager.PositionCombatant(player.targetToAttack.gameObject, player.targetToAttack.fightingPosition.transform.position);
        enemyTargetMovementScript.lookDirection = enemyTargetStoredLookDir;

        HideEnemyFends();
        yield return UpdateFendDisplay();
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
        combatManager.CombatUIManager.playerFendScript.UpdateFendText(combatManager.playerCombat.TotalPlayerFendPower(combatManager.playerCombat.moveSelected.fendMoveModPercent));

        if (combatManager.playerCombat.fendTotal > 0)
        {
            combatManager.CombatUIManager.playerFendScript.ShowFendDisplay(true);
            yield return new WaitForSeconds(1f);
        }

        else
        {
            combatManager.CombatUIManager.playerFendScript.ShowFendDisplay(false);
        }
    }
}