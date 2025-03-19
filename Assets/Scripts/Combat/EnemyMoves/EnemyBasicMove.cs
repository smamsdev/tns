using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBasicMove : EnemyMove
{
    //this should be set at state level
    //vvvvv

   //public override IEnumerator EnemyAttack(CombatManager _combatManager)
   //
   //{
   //    combatManager = _combatManager;
   //    enemy.combatantUI.enemyDamageTakenDisplay.DisableEnemyDamageDisplay();
   //    yield return MoveToAttackPosition();
   //    combatManager.cameraFollow.transformToFollow = combatManager.player.transform;
   //    var enemyLookDirection = enemy.GetComponent<MovementScript>().lookDirection;
   //
   //    CombatEvents.ApplyEnemyAttackToFend(enemy.EnemyAttackTotal(), enemyLookDirection, enemy.moveSelected.attackPushStrength);
   //    StartCoroutine(combatManager.selectedPlayerMove.OnEnemyAttack(combatManager, enemy));
   //
   //    yield return null;
   //}
}
