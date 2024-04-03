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
            if (enemy.attackTotal == 0 && enemy.fendTotal > 0)
            {
                combatManager.combatUIScript.playerFendScript.animator.SetTrigger("fendFade");
            }

            else if (enemy.attackTotal > 0)

            {
                enemy.enemyUI.enemyDamageTakenDisplay.DisableEnemyDamageDisplay();
                combatManager.UpdateFighterPosition(enemy.gameObject, new Vector2(combatManager.battleScheme.playerFightingPosition.transform.position.x + 0.3f, combatManager.battleScheme.playerFightingPosition.transform.position.y), 0.5f);

                yield return new WaitForSeconds(0.5f);

                enemy.moveSelected.OnEnemyAttack();
                StartCoroutine(combatManager.selectedPlayerMove.OnEnemyAttack(combatManager, enemy));

                yield return new WaitForSeconds(0.5f);

                combatManager.UpdateFighterPosition(enemy.gameObject, enemy.enemyFightingPosition.transform.position, 0.5f);

                yield return new WaitForSeconds(0.5f);
            }

            yield return new WaitForSeconds(0.5f);

        }
            combatManager.SetState(combatManager.roundReset);

    }

}

