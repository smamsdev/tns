using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBasicMove : EnemyMove
{
    Enemy enemy;
    public int rng;

    public override void LoadMove(Enemy _enemy)

    {
        enemy = _enemy;

        enemy.attackTotal = Mathf.RoundToInt(enemy.attackBase * attackMoveModPercent);
        enemy.fendTotal = Mathf.RoundToInt(enemy.fendBase * fendMoveModPercent);

        rng = Mathf.RoundToInt(enemy.attackTotal * Random.Range(-0.3f, 0.3f));

        enemy.attackTotal = Mathf.RoundToInt(enemy.attackTotal - enemy.injuryPenalty) + rng;
    }


    public override IEnumerator OnEnemyAttack()

    {
        enemy.enemyUI.enemyDamageTakenDisplay.DisableEnemyDamageDisplay();

        yield return combatManager.combatMovement.MoveCombatant(enemy.gameObject, combatManager.player.transform.position);

        combatManager.cameraFollow.transformToFollow = combatManager.player.transform;
        CombatEvents.ApplyEnemyAttackToFend(enemy.EnemyAttackTotal());

        StartCoroutine(combatManager.selectedPlayerMove.OnEnemyAttack(combatManager, enemy));

        yield return new WaitForSeconds(0.5f);

        yield return combatManager.combatMovement.MoveCombatant(enemy.gameObject, enemy.enemyFightingPosition.transform.position);

        var enemyMovementScript = enemy.GetComponent<ActorMovementScript>();
        enemyMovementScript.lookDirection = enemy.forceLookDirection;
  

        yield return combatManager.combatMovement.MoveCombatant(combatManager.player.gameObject, combatManager.battleScheme.playerFightingPosition.transform.position);

        yield return new WaitForSeconds(0.5f);
    }

}
