using System.Collections;
using UnityEngine;

public abstract class PlayerMove : Move
{
    public Enemy enemy;
    public float damageToPartsMultiplier;
    public int potentialChange;
    public bool isFlaw;

    public virtual IEnumerator OnEnemyAttack(CombatManager _combatManager, Enemy _enemy)
    {
        yield break;
    }

}