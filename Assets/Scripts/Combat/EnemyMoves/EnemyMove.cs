using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyMove : Move
{
    public float targetPositionHorizontalOffset;
    public bool targetIsSelf;

    public void LoadMoveStats(Combatant combatant, CombatManager combatManager)
    {
        this.combatManager = combatManager;

        combatant.attackTotal = Mathf.RoundToInt(combatant.attackBase * attackMoveModPercent);
        combatant.fendTotal = Mathf.RoundToInt(combatant.fendBase * fendMoveModPercent);

        var rng = Mathf.RoundToInt(combatant.attackTotal * Random.Range(-0.3f, 0.3f));

        combatant.attackTotal -= combatant.injuryPenalty;
        combatant.attackTotal = Mathf.RoundToInt(combatant.attackTotal + rng);
    }

    public virtual Vector3 AttackPositionLocation(Enemy enemy)
    {
        Vector3 targetPosition;
        float lookDirX = enemy.GetComponent<MovementScript>().lookDirection.x;

        if (targetIsSelf)
        {
            targetPosition = new Vector3 (enemy.transform.position.x + (targetPositionHorizontalOffset * lookDirX), enemy.transform.position.y);
        }

        else
        {
            targetPosition = new Vector3(enemy.targetToAttack.transform.position.x - (targetPositionHorizontalOffset * lookDirX), enemy.targetToAttack.transform.position.y);
        }

        return targetPosition;
    }
}
