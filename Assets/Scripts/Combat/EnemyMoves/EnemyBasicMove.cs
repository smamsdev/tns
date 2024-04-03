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

        enemy.attackTotal = Mathf.RoundToInt(enemy.attackTotal - enemy.injuryPenalty) + rng; //throw in some RNG for fun
    }


    public override void OnEnemyAttack()

    {
        CombatEvents.ApplyEnemyAttackToFend(enemy.EnemyAttackTotal());
    }

}
