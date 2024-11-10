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
            var playerMovementScript = combatManager.player.GetComponent<MovementScript>();
            var storedLookDirection = playerMovementScript.lookDirection;

            CombatEvents.UpdateNarrator.Invoke("");

            yield return new WaitForSeconds(0.5f);
            CombatEvents.UpdateNarrator.Invoke(enemy.moveSelected.moveName);

            yield return enemy.moveSelected.EnemyAttack(combatManager);

            var enemyAnimator = enemy.gameObject.GetComponent<Animator>();


            enemyAnimator.SetFloat("attackAnimationToUse", enemy.moveSelected.animtionIntTriggerToUse);
            enemyAnimator.SetTrigger("Attack");
            yield return new WaitForSeconds(0.5f);
            //allow half a sec for anim to complete.

            enemyAnimator.SetTrigger("combatIdle");
            yield return enemy.moveSelected.EnemyReverse();
            enemy.GetComponent<MovementScript>().lookDirection = enemy.forceLookDirection;


            CombatEvents.UpdateNarrator.Invoke("");

            yield return combatManager.combatMovement.MoveCombatant(combatManager.player.gameObject, combatManager.battleScheme.playerFightingPosition.transform.position);

            playerMovementScript.lookDirection = storedLookDirection;

        }

        combatManager.SetState(combatManager.roundReset);
    }
}

