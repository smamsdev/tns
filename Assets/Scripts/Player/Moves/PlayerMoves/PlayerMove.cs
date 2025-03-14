using System.Collections;
using UnityEngine;

public abstract class PlayerMove : Move
{
    public Enemy enemy;
    public float damageToPartsMultiplier;
    public int potentialChange;
    public bool isFlaw;

    public abstract IEnumerator OnApplyMove(CombatManager _combatManager, Enemy _enemy);

    public abstract IEnumerator Return();

    public abstract IEnumerator OnEnemyAttack(CombatManager _combatManager, Enemy _enemy);

}