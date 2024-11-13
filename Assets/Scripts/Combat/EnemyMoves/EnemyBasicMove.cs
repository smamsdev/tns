using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBasicMove : EnemyMove
{
    public int rng;

    public override void LoadMove(Enemy _enemy)

    {
        enemy = _enemy;

        enemy.attackTotal = Mathf.RoundToInt(enemy.attackBase * attackMoveModPercent);
        enemy.fendTotal = Mathf.RoundToInt(enemy.fendBase * fendMoveModPercent);

        rng = Mathf.RoundToInt(enemy.attackTotal * Random.Range(-0.3f, 0.3f));

        enemy.attackTotal = Mathf.RoundToInt(enemy.attackTotal - enemy.injuryPenalty) + rng;
    }

    public override IEnumerator EnemyAttack(CombatManager _combatManager)

    {
        combatManager = _combatManager;
        var distance = enemy.moveSelected.distanceToCoverPercent;

        enemy.enemyUI.enemyDamageTakenDisplay.DisableEnemyDamageDisplay();

        var combatMovementInstanceGO = Instantiate(combatManager.combatMovementPrefab, this.transform);
        var combatMovementInstance = combatMovementInstanceGO.GetComponent<CombatMovement>();
        yield return (combatMovementInstance.MoveCombatant(enemy.gameObject, combatManager.player.transform.position, stoppingPercentage: distance));
        Destroy(combatMovementInstanceGO);

        combatManager.cameraFollow.transformToFollow = combatManager.player.transform;

        var enemyLookDirection = enemy.GetComponent<MovementScript>().lookDirection;

        CombatEvents.ApplyEnemyAttackToFend(enemy.EnemyAttackTotal(), enemyLookDirection, enemy.moveSelected.attackPushStrength);

        StartCoroutine(combatManager.selectedPlayerMove.OnEnemyAttack(combatManager, enemy));
        yield return null;
    }
}
