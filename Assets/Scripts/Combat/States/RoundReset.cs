using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundReset : State
{
    [SerializeField] CombatManager combatManager;

    public override IEnumerator StartState()
    {
        combatManager.CombatUIManager.playerFendScript.animator.SetTrigger("fendFade");
        var playerMovementScript = combatManager.player.GetComponent<PlayerMovementScript>();
        
        //combatManager.player.GetComponent<EquippedGear>().equippedGear[0].ResetAttackGear();
        //combatManager.player.GetComponent<EquippedGear>().equippedGear[0].ResetFendGear();

        combatManager.playerMoveManager.firstMoveIs = 0;
        combatManager.playerMoveManager.secondMoveIs = 0;

        combatManager.roundCount++;

        foreach (Enemy enemy in combatManager.enemy)
        {
            enemy.enemyUI.enemyFendScript.ResetAllAnimationTriggers(); //its just easier this way 

            var enemyMovementScript = enemy.GetComponent<ActorMovementScript>();
            enemyMovementScript.actorRigidBody2d.bodyType = RigidbodyType2D.Kinematic;

            enemy.SelectEnemyMove();

            if (enemy.attackTotal > 0)
            {
                enemy.enemyUI.enemyAttackDisplay.UpdateEnemyAttackDisplay(enemy.EnemyAttackTotal());
                enemy.enemyUI.enemyAttackDisplay.ShowAttackDisplay(true);
            }

            if (enemy.fendTotal > 0)
            {
                enemy.enemyUI.enemyFendScript.UpdateFendDisplay(enemy.fendTotal);
            }
        }

        combatManager.playerCombatStats.attackPower = 0;
        combatManager.playerCombatStats.playerFend = 0;

        combatManager.CombatUIManager.playerFendScript.UpdateFendText(0);

        combatManager.CombatUIManager.playerFendScript.ResetAllAnimationTriggers(); //its just easier this way 


        combatManager.SetState(combatManager.firstMove);

        yield break;
    }

}
