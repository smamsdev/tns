using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyMoveState : State
{
    [SerializeField] CombatManager combatManager;

public override IEnumerator StartState()

{
        foreach (Ally ally in combatManager.allies)
        {
            CombatEvents.UpdateNarrator.Invoke("");
            combatManager.cameraFollow.transformToFollow = ally.transform;

            yield return new WaitForSeconds(0.5f);
            CombatEvents.UpdateNarrator.Invoke(ally.moveSelected.moveName);
            yield return ally.moveSelected.AllyAttack(combatManager);



            yield return null;
        }





    //var allyMovementScript = combatManager.player.GetComponent<PlayerMovementScript>();
    //var playerLookDirection = playerMovementScript.lookDirection;
    //var moveSelected = combatManager.selectedPlayerMove;
    //
    //var enemySelected = combatManager.enemies[combatManager.selectedEnemy];
    //
    //combatManager.playerCombatStats.TotalPlayerAttackPower(moveSelected.attackMoveMultiplier);
    //CombatEvents.UpdateNarrator.Invoke(combatManager.selectedPlayerMove.moveName);
    //CombatEvents.UpdatePlayerPot.Invoke(combatManager.selectedPlayerMove.potentialChange);
    //
    //yield return combatManager.selectedPlayerMove.OnApplyMove(combatManager, enemySelected);
    //var storedLookDir = playerLookDirection; //this needs to happen here to remember direction of last enemy attacked
    //
    ////yield break;
    //
    //yield return combatManager.selectedPlayerMove.Return();
    //combatManager.player.GetComponent<PlayerMovementScript>().lookDirection = storedLookDir;
    //combatManager.playerAnimator.SetTrigger("CombatIdle");
    //
    //
    ////return enemy

    //
    //StartCoroutine(EndMove());
    //yield return null;
}

   IEnumerator EndMove()

   {
       foreach (Enemy enemy in combatManager.enemies)

       {
           enemy.enemyUI.enemyFendScript.enemyFendAnimator.SetTrigger("fendFade");
           enemy.enemyUI.enemyStatsDisplay.enemyStatsDisplayGameObject.SetActive(false);
       }

       combatManager.playerCombatStats.TotalPlayerFendPower(combatManager.selectedPlayerMove.fendMoveMultiplier);
       combatManager.CombatUIManager.playerFendScript.UpdateFendText(combatManager.playerCombatStats.playerFend);

       if (combatManager.playerCombatStats.playerFend > 0)
       {
           combatManager.CombatUIManager.playerFendScript.ShowFendDisplay(true);
           yield return new WaitForSeconds(1f);
       }
       combatManager.SetState(combatManager.enemyAttackState);
   }

}
