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
            CombatEvents.UpdateNarrator.Invoke("");

            yield return new WaitForSeconds(0.5f);
            CombatEvents.UpdateNarrator.Invoke(enemy.moveSelected.moveName);

            yield return enemy.moveSelected.EnemyAttack(combatManager);

            var enemyAnimator = enemy.gameObject.GetComponent<Animator>();

            enemyAnimator.SetFloat("attackAnimationToUse", enemy.moveSelected.animtionIntTriggerToUse);
            enemyAnimator.SetTrigger("Attack");

            yield return enemy.moveSelected.EnemyReverse();
            enemyAnimator.SetTrigger("combatIdle");

            CombatEvents.UpdateNarrator.Invoke("");
        }

        combatManager.SetState(combatManager.roundReset);
    }
}

