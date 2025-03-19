using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundReset : State
{
    [SerializeField] CombatManager combatManager;

    public override IEnumerator StartState()
    {
        combatManager.playerCombat.combatantUI.fendScript.animator.SetTrigger("fendFade");
        var playerMovementScript = combatManager.player.GetComponent<PlayerMovementScript>();
        
        //combatManager.player.GetComponent<EquippedGear>().equippedGear[0].ResetAttackGear();
        //combatManager.player.GetComponent<EquippedGear>().equippedGear[0].ResetFendGear();

        combatManager.playerMoveManager.firstMoveIs = 0;
        combatManager.playerMoveManager.secondMoveIs = 0;

        combatManager.roundCount++;

        foreach (Enemy enemy in combatManager.enemies)
        {
            enemy.combatantUI.fendScript.ResetAllAnimationTriggers(); //its just easier this way 

            var enemyMovementScript = enemy.GetComponent<ActorMovementScript>();
            enemyMovementScript.actorRigidBody2d.bodyType = RigidbodyType2D.Kinematic;

            enemy.SelectMove();

            if (enemy.attackTotal > 0)
            {
                enemy.combatantUI.attackDisplay.UpdateAttackDisplay(enemy.attackTotal);
                enemy.combatantUI.attackDisplay.ShowAttackDisplay(true);
            }

            if (enemy.fendTotal > 0)
            {
                enemy.combatantUI.fendScript.UpdateFendText(enemy.fendTotal);
            }
        }

        combatManager.playerCombat.attackPower = 0;
        combatManager.playerCombat.playerFend = 0;

        combatManager.playerCombat.combatantUI.fendScript.UpdateFendText(0);

        combatManager.playerCombat.combatantUI.fendScript.ResetAllAnimationTriggers(); //its just easier this way 


        combatManager.SetState(combatManager.firstMove);

        yield break;
    }

}
