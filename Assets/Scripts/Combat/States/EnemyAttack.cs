using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : State
{
    [SerializeField] CombatManager combatManager;
    public Gear[] equippedGear;
    [SerializeField] FendScript fendScript;
    public int enemyIteration = 0;

    public override IEnumerator StartState()
    {
        //   equippedGear = combatManager.player.GetComponent<GearEquip>().equippedGear;
        //   int i;
        //
        //   for (i = 0; i < equippedGear.Length;)
        //
        //   {
        //       equippedGear[i].ApplyFendGear();
        //       i++;
        //   }

        foreach (Enemy enemy in combatManager.enemy)

        {
            //init
            var playerMovementScript = combatManager.player.GetComponent<MovementScript>();
            var storedLookDirection = playerMovementScript.lookDirection;
            var enemyLastLookDirection = enemy.GetComponent<MovementScript>().lookDirection;
            var enemyAnimator = enemy.gameObject.GetComponent<Animator>();

            //reset narrator
            CombatEvents.UpdateNarrator.Invoke("");
            yield return new WaitForSeconds(0.5f);
            CombatEvents.UpdateNarrator.Invoke(enemy.moveSelected.moveName);

            //begin move
            yield return enemy.moveSelected.EnemyAttack(combatManager);

            //move animation
            enemyAnimator.SetFloat("attackAnimationToUse", enemy.moveSelected.animtionIntTriggerToUse);
            enemyAnimator.SetTrigger("Attack");
            yield return new WaitForSeconds(0.2f);
            //allow half a sec for anim to complete.

            //return to fightingpos, and return look direct
            yield return enemy.moveSelected.EnemyReturn();
            enemyAnimator.SetTrigger("combatIdle");
            enemy.GetComponent<MovementScript>().lookDirection = enemyLastLookDirection;

            //tidy up
            CombatEvents.UpdateNarrator.Invoke("");

            var combatMovementInstanceGO = Instantiate(combatManager.combatMovementPrefab, this.transform);
            var combatMovementInstance = combatMovementInstanceGO.GetComponent<CombatMovement>();
            yield return (combatMovementInstance.MoveCombatant(combatManager.player.gameObject, combatManager.battleScheme.playerFightingPosition.transform.position));
            Destroy(combatMovementInstanceGO);

            playerMovementScript.lookDirection = storedLookDirection;
        }

        combatManager.SetState(combatManager.roundReset);
    }
}

