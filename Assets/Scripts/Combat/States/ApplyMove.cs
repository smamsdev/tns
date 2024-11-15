using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyMove : State
{
    [SerializeField] CombatManager combatManager;

    public override IEnumerator StartState()
    {
        combatManager.CombatUIManager.ChangeMenuState(false);
        var playerMovementScript = combatManager.player.GetComponent<PlayerMovementScript>();
        var playerLookDirection = playerMovementScript.lookDirection;
        var moveSelected = combatManager.selectedPlayerMove;

        foreach (Enemy enemy in combatManager.enemy)

        {
            enemy.enemyUI.enemyDamageTakenDisplay.DisableEnemyDamageDisplay();
            enemy.enemyUI.enemyAttackDisplay.ShowAttackDisplay(false);
        }

        var enemySelected = combatManager.enemy[combatManager.selectedEnemy];

        //   var equippedGear = combatManager.player.GetComponent<EquippedGear>().equippedGear;
        //  int i;
        //
        //  for (i = 0; i < equippedGear.Length;)
        //
        //  {
        //      equippedGear[i].ApplyAttackGear();
        //      i++;
        //  }

        combatManager.playerCombatStats.TotalPlayerAttackPower(moveSelected.attackMoveMultiplier);
        CombatEvents.UpdateNarrator.Invoke(combatManager.selectedPlayerMove.moveName);
        CombatEvents.UpdatePlayerPot.Invoke(combatManager.selectedPlayerMove.potentialChange);

        yield return combatManager.selectedPlayerMove.OnApplyMove(combatManager, enemySelected);
        var storedLookDir = playerLookDirection; //this needs to happen here to remember direction of last enemy attacked

        //yield break;

        yield return combatManager.selectedPlayerMove.Return();
        combatManager.player.GetComponent<PlayerMovementScript>().lookDirection = storedLookDir;
        combatManager.playerAnimator.SetTrigger("CombatIdle");


        //return enemy
        var combatMovementInstanceGO = Instantiate(combatManager.combatMovementPrefab, this.transform);
        var combatMovementInstance = combatMovementInstanceGO.GetComponent<CombatMovement>();
        yield return (combatMovementInstance.MoveCombatant(enemySelected.gameObject, enemySelected.enemyFightingPosition.transform.position));
        Destroy(combatMovementInstanceGO);

        StartCoroutine(EndMove());
        yield return null;
    }

    IEnumerator EndMove()

    {
        foreach (Enemy enemy in combatManager.enemy)

        {
            enemy.enemyUI.enemyFendScript.enemyFendAnimator.SetTrigger("fendFade");
            enemy.enemyUI.enemyStatsDisplay.enemyStatsDisplayGameObject.SetActive(false);
        }

        combatManager.playerCombatStats.TotalPlayerFendPower(combatManager.selectedPlayerMove.fendMoveMultiplier);
        combatManager.CombatUIManager.playerFendScript.UpdateFendText(combatManager.playerCombatStats.playerFend);

        if (combatManager.playerCombatStats.playerFend >0)
        {
            combatManager.CombatUIManager.playerFendScript.ShowFendDisplay(true);
            yield return new WaitForSeconds(1f);
        }
            combatManager.SetState(combatManager.enemyAttack);
    }
}
