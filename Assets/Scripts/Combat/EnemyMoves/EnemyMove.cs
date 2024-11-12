using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyMove : MonoBehaviour
{
    public float attackMoveModPercent;
    public float fendMoveModPercent;
    public int moveWeighting;
    public float animtionIntTriggerToUse = 0;
    public float distanceToCoverPercent = 80f;
    public Enemy enemy;
    public float attackPushStrength = 0.2f;


    public string moveName;
    [HideInInspector] public CombatManager combatManager;

    public abstract IEnumerator EnemyAttack(CombatManager _combatManager);

    public virtual IEnumerator EnemyReverse()

    {
        yield return new WaitForSeconds(0.5f);
        yield return combatManager.combatMovement.MoveCombatant(enemy.gameObject, enemy.enemyFightingPosition.transform.position);
    }

    public abstract void LoadMove(Enemy enemy);
}
