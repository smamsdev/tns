using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : State
{
    [SerializeField] CombatManager combatManager;
    public Gear[] equippedGear;
    [SerializeField] FendScript fendScript;
    public int enemyIteration = 0;
    public CameraFollow cameraFollow;

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
            yield return combatManager.combatMovement.MoveCombatant(enemy.gameObject, enemy.enemyFightingPosition.transform.position);
            cameraFollow.transformToFollow = enemy.transform;

            yield return new WaitForSeconds(0.5f);

            if (enemy.attackTotal == 0 && enemy.fendTotal > 0)
            {
                combatManager.CombatUIManager.playerFendScript.animator.SetTrigger("fendFade");
            }

            else if (enemy.attackTotal > 0)

            {
                enemy.enemyUI.enemyDamageTakenDisplay.DisableEnemyDamageDisplay();
                yield return combatManager.combatMovement.MoveCombatant(enemy.gameObject, combatManager.player.transform.position, 50f);
                yield return combatManager.combatMovement.MoveCombatant(enemy.gameObject, combatManager.player.transform.position);


                cameraFollow.transformToFollow = combatManager.player.transform;
                enemy.moveSelected.OnEnemyAttack();
                StartCoroutine(combatManager.selectedPlayerMove.OnEnemyAttack(combatManager, enemy));

                yield return new WaitForSeconds(0.5f);

                yield return combatManager.combatMovement.MoveCombatant(enemy.gameObject, enemy.enemyFightingPosition.transform.position);

                var enemyMovementScript = enemy.GetComponent<ActorMovementScript>();
                enemyMovementScript.lookDirection = enemy.forceLookDirection;

                yield return new WaitForSeconds(0.5f);
            }

            yield return new WaitForSeconds(0.5f);

        }
        combatManager.SetState(combatManager.roundReset);
    }
}

