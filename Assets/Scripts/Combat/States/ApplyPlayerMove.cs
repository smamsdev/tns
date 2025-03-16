using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyPlayerMove : State
{
    [SerializeField] CombatManager combatManager;

    public override IEnumerator StartState()
    {
        combatManager.CombatUIManager.DisableMenuState();

        //store player look dir and camera focus
        var playerMovementScript = combatManager.player.GetComponent<PlayerMovementScript>();
        var playerLookDirection = playerMovementScript.lookDirection;
        combatManager.cameraFollow.transformToFollow = combatManager.player.transform;

        var enemySelected = combatManager.enemies[combatManager.selectedEnemy];

        //   var equippedGear = combatManager.player.GetComponent<EquippedGear>().equippedGear;
        //  int i;
        //
        //  for (i = 0; i < equippedGear.Length;)
        //
        //  {
        //      equippedGear[i].ApplyAttackGear();
        //      i++;
        //  }

        //update narrator and change potential
        var moveSelected = combatManager.selectedPlayerMove;
        combatManager.playerCombat.TotalPlayerAttackPower(moveSelected.attackMoveModPercent);
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
        yield return (combatMovementInstance.MoveCombatant(enemySelected.gameObject, enemySelected.fightingPosition.transform.position));
        Destroy(combatMovementInstanceGO);

        StartCoroutine(EndMove());
        yield return null;
    }

    IEnumerator EndMove()

    {
        foreach (Enemy enemy in combatManager.enemies)

        {
            enemy.enemyUI.fendScript.animator.SetTrigger("fendFade");
            enemy.enemyUI.enemyStatsDisplay.enemyStatsDisplayGameObject.SetActive(false);
        }
        combatManager.CombatUIManager.playerFendScript.UpdateFendText(combatManager.playerCombat.TotalPlayerFendPower(combatManager.selectedPlayerMove.fendMoveModPercent));

        if (combatManager.playerCombat.fendTotal > 0)
        {
            combatManager.CombatUIManager.playerFendScript.ShowFendDisplay(true);
            yield return new WaitForSeconds(1f);
        }

        else
        {
            combatManager.CombatUIManager.playerFendScript.ShowFendDisplay(false);
        }

            combatManager.SetState(combatManager.enemyMoveState);
    }
}